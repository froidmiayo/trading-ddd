using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TradingEngineDDD.Models.ValueObject;

namespace TradingEngineDDD.Repository.Dapper
{
    public class CurrencyConversionRepository : BaseRepository, ICurrencyConversionRepository
    {
        public CurrencyConversionUnit GetCurrencyUsdConversion(Currency currency)
        {
            return CurrencyConversionRepositoryMapper.ToCurrencyConversionUnit(GetAllWhere<CurrencyConversionRepositoryDbm>(new { currency.Code }).FirstOrDefault());
        }
    }
}