using System;

namespace TradingEngineDDD.Models.ValueObject
{
    public class AccountMoney
    {
        public decimal Value { get; }

        public AccountMoney(decimal value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Account Money could not be negative");

            Value = value;
        }

        public AccountMoney Add(decimal toAdd)
        {
            return  new AccountMoney(Value + toAdd);
        }

        public AccountMoney Deduct(decimal toDeduct)
        {
            return new AccountMoney(Value - toDeduct);
        }

        public AccountMoney Multiply(decimal a)
        {
            return  new AccountMoney(Value * a);
        }

        public AccountMoney Divide(decimal a)
        {
            return new AccountMoney(Math.Round(Value / a,6));
        }
    }
}