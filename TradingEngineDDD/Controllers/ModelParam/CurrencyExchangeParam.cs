using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradingEngineDDD.Controllers.ModelParam
{
    public class CurrencyExchangeParam
    {
        public  string CurrencyFrom { get; set; }
        public  string CurrencyTo { get; set; }
        public  decimal Amount { get; set; }
    }
}