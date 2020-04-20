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
using TradingEngineDDD.Models.Entity;
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
        public void CurrencyExchange_ShouldProperlyConvert()
        {
            
            var clientId = Fixture.Create<ClientId>();

            var c1 = Fixture.Create<Currency>();
            var c2 = Fixture.Create<Currency>();
            var data = new List<Account>
            {
                new Account(c1, new AccountId(1),
                    new AccountMoney(2000), clientId),
                new Account(c2, new AccountId(2),
                    new AccountMoney(0), clientId)
            };

            var currencyMock = Fixture.Freeze<Mock<ICurrencyConversionRepository>>();
            
            var eventMock = Fixture.Freeze<Mock<IDomainEventPublisher>>();
            var accountMock = Fixture.Freeze<Mock<IAccountRepository>>();
            accountMock.Setup(s => s.FindByClientId(It.IsAny<ClientId>())).Returns(data);

            var service = Fixture.Create<AccountDetailsService>();

            currencyMock.Setup(s => s.GetCurrencyUsdConversion(It.Is<Currency>(x=> x.Code == c1.Code))).Returns(
                new CurrencyConversionUnit(c1, new AccountMoney(1)));
            currencyMock.Setup(s => s.GetCurrencyUsdConversion(It.Is<Currency>(x=> x.Code == c2.Code))).Returns(
                new CurrencyConversionUnit(c2, new AccountMoney(0.02m)));
            
            
            var accountList = service.GetAccountsUsingClientId(clientId.Value);
            var amount = new AccountMoney(1000);
            var conversionFrom = currencyMock.Object.GetCurrencyUsdConversion(c1);
            var conversionTo = currencyMock.Object.GetCurrencyUsdConversion(c2);
            var conversion = new CurrencyConversion(conversionFrom,conversionTo);

                
            var response = new CurrencyExchangeRequest(clientId, accountList, c1, c2, amount,conversion.ConversionRate);

            var usd = response.ModifiedAccounts.FirstOrDefault(x => x.AccountId.Value == 1);
            var php = response.ModifiedAccounts.FirstOrDefault(x => x.AccountId.Value == 2);
            


            Assert.AreEqual(php?.Balance.Value,50000);
            Assert.AreEqual(usd?.Balance.Value,1000);

            //Reverse
            data  = new List<Account>();
            data.AddRange(response.ModifiedAccounts);

            conversionFrom = currencyMock.Object.GetCurrencyUsdConversion(c2);
            conversionTo = currencyMock.Object.GetCurrencyUsdConversion(c1);
            
            response = new CurrencyExchangeRequest(clientId, data, c2, c1, new AccountMoney(50000), new CurrencyConversion(conversionFrom,conversionTo).ConversionRate);
            
            usd = response.ModifiedAccounts.FirstOrDefault(x => x.AccountId.Value == 1);
            php = response.ModifiedAccounts.FirstOrDefault(x => x.AccountId.Value == 2);

            Assert.AreEqual(php?.Balance.Value,0);
            Assert.AreEqual(usd?.Balance.Value,2000);
        }

        [Test]
        public void FundTransfer_ShouldProperly_Debit_Credit()
        {
            
            var sender = Fixture.Create<ClientId>();
            var recipient = Fixture.Create<ClientId>();
            
  

            var c = Fixture.Create<Currency>();
           
            var dataSender = new List<Account>
            {
                new Account(c, new AccountId(1), new AccountMoney(2000), sender),
                
            };

            var dataRecipient = new List<Account>
            {
                new Account(c, new AccountId(2), new AccountMoney(0), recipient),
            };


            var ft = new FundTransferRequestUnit(500, c, recipient);
            var response = new FundTransferRequest(dataSender, dataRecipient, ft);

            Assert.AreEqual(response.ModifiedAccounts.First(x => x.AccountId.Value == 1).Balance.Value, 1500);
            Assert.AreEqual(response.ModifiedAccounts.First(x => x.AccountId.Value == 2).Balance.Value, 500);

        }
    }
}
