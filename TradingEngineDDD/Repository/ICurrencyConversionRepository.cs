using TradingEngineDDD.Models.ValueObject;

namespace TradingEngineDDD.Repository
{
    public interface ICurrencyConversionRepository
    {
        CurrencyConversionUnit GetCurrencyUsdConversion(Currency currency);
    }
}