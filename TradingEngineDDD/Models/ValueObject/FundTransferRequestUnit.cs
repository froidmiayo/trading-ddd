using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradingEngineDDD.Models.ValueObject
{
    public class FundTransferRequestUnit
    {
        public  ClientId RecipientClientId { get; }
        public  AccountMoney Amount { get; }
        public Currency Currency { get; }

        public FundTransferRequestUnit(decimal amount, Currency currency, ClientId clientId)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "amount should have value");
            if(currency == null)
                throw new ArgumentNullException(nameof(currency));
            if(clientId == null)
                throw  new ArgumentNullException(nameof(clientId));

            Amount = new AccountMoney(amount);
            Currency = new Currency(currency.Code);
            RecipientClientId = new ClientId(clientId.Value);
        }


    }
}