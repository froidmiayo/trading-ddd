using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TradingEngineDDD.Application;
using TradingEngineDDD.Models.DomainEvent;
using TradingEngineDDD.Models.ValueObject;
using TradingEngineDDD.Repository;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TradingEngineDDDTest
{
    public class UnitTestServiceBase
    {
        public IFixture Fixture { get; set; }
        public UnitTestServiceBase()
        {
            Fixture = new Fixture().Customize(new AutoMoqCustomization());
        }
    }
    [TestFixture]
    public class UnitTest1: UnitTestServiceBase
    {
        private readonly Random _rnd = new Random();
        [Test]
        public void GetBalance_Should_ReturnAccounts()
        {
            var data = new List<Account>
            {
                new Account(new Currency(new Guid().ToString()), new AccountId(_rnd.Next(1, 100)),
                    new AccountMoney(_rnd.Next(1000, 2000)), new ClientId(1))
            };
            var mock = Fixture.Freeze<Mock<IAccountRepository>>();
            var service = Fixture.Create<AccountDetailsService>();
            var clientId = Fixture.Create<ClientId>();
            mock.Setup(s => s.FindByClientId(It.IsAny<ClientId>())).Returns(data);

            var result = service.GetAccountsUsingClientId(clientId.Value);


            Assert.IsTrue(result.Any());

        }

        [Test]
        public void GetBalance_Should_ReturnEmpty()
        {
            var data = new List<Account>
            {
            };
            var mock = Fixture.Freeze<Mock<IAccountRepository>>();
            var service = Fixture.Create<AccountDetailsService>();
            var clientId = Fixture.Create<ClientId>();
            mock.Setup(s => s.FindByClientId(It.IsAny<ClientId>())).Returns(data);

            var result = service.GetAccountsUsingClientId(clientId.Value);


            Assert.AreEqual(0,result.Count);

        }

        [Test]
        public void CurrencyExchange_ShouldProperlyCompute()
        {
            var service = Fixture.Create<AccountDetailsService>();
            var clientId = Fixture.Create<ClientId>();

            var c1 = Fixture.Create<Currency>();
            var c2 = Fixture.Create<Currency>();
            var data = new List<Account>
            {
                new Account(new Currency("PHP"), new AccountId(1),
                    new AccountMoney(0), clientId),
                new Account(new Currency("USD"), new AccountId(2),
                    new AccountMoney(2000), clientId)
            };
            


            var currencyMock = Fixture.Freeze<Mock<ICurrencyConversionRepository>>();

            currencyMock.Setup(x => x.GetCurrencyUsdConversion(c1)).Returns(
                new CurrencyConversionUnit(c1, new AccountMoney(0.02m)));
            currencyMock.Setup(x => x.GetCurrencyUsdConversion(c2)).Returns(
                new CurrencyConversionUnit(c2, new AccountMoney(1)));
            var eventMock = Fixture.Freeze<Mock<IDomainEventPublisher>>();
            var accountMock = Fixture.Freeze<Mock<IAccountRepository>>();
            accountMock.Setup(s => s.FindByClientId(It.IsAny<ClientId>())).Returns(data);
            var result = service.CurrencyExchangeRequest(1, c1.Code, c2.Code, 1000);

            var php = result.FirstOrDefault(x => x.AccountId.Value == 1);
            var usd = result.FirstOrDefault(x => x.AccountId.Value == 2);


            Assert.AreEqual(php?.Balance.Value,50000);
            Assert.AreEqual(usd?.Balance.Value,1000);
        }
    }
}
