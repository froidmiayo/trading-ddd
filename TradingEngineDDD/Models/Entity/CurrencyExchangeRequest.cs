using System;
using System.Collections.Generic;
using System.Linq;
using TradingEngineDDD.Models.ValueObject;

namespace TradingEngineDDD.Models.Entity
{
    public class CurrencyExchangeRequest
    {
        
        public CurrencyExchangeRequest(ClientId clientId,List<Account> currentAccounts, Currency currencyFrom,Currency currencyTo,AccountMoney amount, AccountMoney conversionRate)
        {
            
            if(currentAccounts == null || currentAccounts.Count == 0)
                throw new ArgumentNullException(nameof(currentAccounts), "Needs to have at least 1 account");
            if(currencyFrom == null)
                throw new ArgumentNullException(nameof(currencyFrom), "currencyFrom required");
            if(currencyTo == null)
                throw new ArgumentNullException(nameof(currencyTo), "currencyTo required");
            if(currentAccounts.All(x => x.Currency.Code != currencyFrom.Code))
                throw new ArgumentOutOfRangeException(nameof(currencyFrom), "currencyFrom need to exist from currentAccounts");
            if(amount == null || amount.Value <=0)
                throw new ArgumentOutOfRangeException(nameof(currencyFrom), "amount should be valid");
            if(conversionRate == null || conversionRate.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(conversionRate), "conversionRate should be valid");
            if(amount != null && currentAccounts.First(x=> x.Currency.Code == currencyFrom.Code).Balance.Value < amount.Value)
                throw new ArgumentOutOfRangeException(nameof(conversionRate), "insufficient balance");
            if(currencyFrom.Code == currencyTo.Code)
                throw new ArgumentOutOfRangeException(nameof(currencyTo), "currencyTo Should not be the same with currencyFrom");

            var fromAccount = currentAccounts.First(x => x.Currency.Code == currencyFrom.Code);
            var toAccount = currentAccounts.FirstOrDefault(x => x.Currency.Code == currencyTo.Code);

            ModifiedAccounts = new List<Account>
            {
                new Account(new Currency(fromAccount.Currency.Code), new AccountId(fromAccount.AccountId.Value),
                    fromAccount.Balance.Deduct(amount.Value), new ClientId(clientId.Value)),
                toAccount == null
                    ? new Account(new Currency(currencyTo.Code), new AccountId(0),
                        new AccountMoney(amount.Value).Multiply(conversionRate.Value), new ClientId(clientId.Value))
                    : new Account(new Currency(currencyTo.Code), new AccountId(toAccount.AccountId.Value),
                        toAccount.Balance.Add(amount.Value * conversionRate.Value), new ClientId(clientId.Value))
            };
        }
        public List<Account> ModifiedAccounts { get; }

    }
}