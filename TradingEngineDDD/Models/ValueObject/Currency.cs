using System;

namespace TradingEngineDDD.Models.ValueObject
{
    public class Currency
    {
        public Currency(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Currency Code should not be empty");
            Code = code;
        }

        public string Code { get; }
    }
}