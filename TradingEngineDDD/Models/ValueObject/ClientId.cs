using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradingEngineDDD.Models.ValueObject
{
    public class ClientId
    {
        public int Value { get; }

        public ClientId(int value)
        {
            if (value < 1)
                throw new ArgumentException("value should be > 0", nameof(value));

            Value = value;
        }
    }
}