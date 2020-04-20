using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradingEngineDDD.Models.ValueObject
{
    public class Account
    {
        public Currency Currency { get; }

        public AccountId AccountId { get; }

        public AccountMoney Balance { get; }
        public ClientId ClientId { get; }
        public Account(Currency currency, AccountId id, AccountMoney bal,ClientId clientId)
        {
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
            AccountId = id ?? throw new ArgumentNullException(nameof(id));
            Balance = bal ?? throw new ArgumentNullException(nameof(bal));
            ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
        }
    }

    public class AccountId
    {
        public AccountId(int id)
        {
            if (id < 0)
                throw new ArgumentNullException(nameof(id),"AccountId should be >= 0");

            Value = id;
        }

        public int Value { get; }
    }
}