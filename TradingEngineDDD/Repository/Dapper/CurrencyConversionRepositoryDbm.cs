
using Dapper.Contrib.Extensions;

namespace TradingEngineDDD.Repository.Dapper
{
    [Table("TblCurrencyConversion")]
    public class CurrencyConversionRepositoryDbm
    {
        public  string Code { get; set; }
        public  decimal ConversionRate { get; set; }
    }
}