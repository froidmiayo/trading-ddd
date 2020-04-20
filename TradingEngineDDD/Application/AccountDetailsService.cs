using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TradingEngineDDD.Models.DomainEvent;
using TradingEngineDDD.Models.Entity;
using TradingEngineDDD.Models.ValueObject;
using TradingEngineDDD.Repository;

namespace TradingEngineDDD.Application
{
    public interface IAccountDetailsService
    {
        List<Account> GetAccountsUsingClientId(int id);

        List<Account> CurrencyExchangeRequest(int clientIdValue, string currencyFromStr, string currencyToStr,
            decimal amountValue);

        List<Account> FundTransferRequest(int recipientClientId, int senderClientId, string currencyStr,
            decimal amount);
    }
    public class AccountDetailsService: IAccountDetailsService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrencyConversionRepository _currencyConversionRepository;
        private readonly IDomainEventPublisher _domainEvent;
        public AccountDetailsService(IAccountRepository accountRepository, ICurrencyConversionRepository currencyConversionRepository, IDomainEventPublisher domainEvent)
        {
            _accountRepository = accountRepository;
            _currencyConversionRepository = currencyConversionRepository;
            _domainEvent = domainEvent;
        }

        public List<Account> GetAccountsUsingClientId(int id)
        {
            return _accountRepository.FindByClientId(new ClientId(id));

        }

        public List<Account> CurrencyExchangeRequest(int clientIdValue, string currencyFromStr, string currencyToStr,
            decimal amountValue)
        {
            var currencyFrom = new Currency(currencyFromStr);
            var currencyTo = new Currency(currencyToStr);
            var clientId = new ClientId(clientIdValue);
            var accountList = GetAccountsUsingClientId(clientId.Value);
            var amount = new AccountMoney(amountValue);
            var conversion = new CurrencyConversion(_currencyConversionRepository.GetCurrencyUsdConversion(currencyFrom),_currencyConversionRepository.GetCurrencyUsdConversion(currencyTo));
                
            var response = new CurrencyExchangeRequest(clientId, accountList, currencyFrom, currencyTo, amount,conversion.ConversionRate);

            InsertOrUpdateAccounts(response.ModifiedAccounts);

            _domainEvent.Publish(new CurrencyExchangeSuccessfulEvent(clientId, currencyFrom, currencyTo, amount, conversion));

            return response.ModifiedAccounts;
        }

        private void InsertOrUpdateAccounts(IEnumerable<Account> list)
        {
            foreach (var item in list)
            {
                if (item.AccountId.Value.Equals(0))
                {
                    _accountRepository.Insert(item);
                }
                else
                {
                    _accountRepository.Update(item);
                }
            }
        }

        public List<Account> FundTransferRequest(int recipientClientId, int senderClientId, string currencyStr,
            decimal amount)
        {
            var currency = new Currency(currencyStr);

            var sender = GetAccountsUsingClientId(senderClientId);
            var recipient = GetAccountsUsingClientId(recipientClientId);
            var fundTransfer = new FundTransferRequestUnit(amount, currency, new ClientId(recipientClientId));


            var response = new FundTransferRequest(sender, recipient, fundTransfer);

            InsertOrUpdateAccounts(response.ModifiedAccounts);

            _domainEvent.Publish(new FundTransferSuccessfulEvent(senderClientId,recipientClientId, amount, currency));

            return response.ModifiedAccounts;
        }
    }
}