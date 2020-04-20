using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TradingEngineDDD.Models.ValueObject;

namespace TradingEngineDDD.Models.Entity
{
    public class CurrencyConversion
    {
        public CurrencyConversion(CurrencyConversionUnit currencyFrom, CurrencyConversionUnit currencyTo)
        {
            if(currencyFrom == null)
                throw new ArgumentNullException(nameof(currencyFrom));
            if(currencyTo == null)
                throw new ArgumentNullException(nameof(currencyTo));

            ConversionRate = new AccountMoney(currencyFrom.ValueInUsd.Value).Divide(currencyTo.ValueInUsd.Value);
        }
        public AccountMoney ConversionRate { get; }

    }
}