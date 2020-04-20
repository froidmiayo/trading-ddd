using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradingEngineDDD.Models.ValueObject
{
    public class CurrencyConversionUnit
    {
        public Currency Currency { get; }
        public AccountMoney ValueInUsd { get; }

        public CurrencyConversionUnit(Currency currency, AccountMoney value)
        {
            Currency = new Currency(currency.Code);
            ValueInUsd = new AccountMoney(value.Value);
        }
    }
}