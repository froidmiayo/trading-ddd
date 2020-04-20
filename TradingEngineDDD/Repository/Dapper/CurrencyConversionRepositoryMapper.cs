using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using TradingEngineDDD.Models.ValueObject;

namespace TradingEngineDDD.Repository.Dapper
{
    public static class CurrencyConversionRepositoryMapper
    {

        public static CurrencyConversionUnit ToCurrencyConversionUnit(CurrencyConversionRepositoryDbm param)
        {
            return new CurrencyConversionUnit(new Currency(param.Code), new AccountMoney(param.ConversionRate));
        }
        
    }
}