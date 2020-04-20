using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TradingEngineDDD.Models.ValueObject;

namespace TradingEngineDDD.Models.Entity
{
    public class Client
    {
        public Client(ClientId clientId, List<Account> accounts)
        {
            ClientId = clientId;
            Accounts = accounts;
        }

        public  ClientId ClientId { get; }
        public  List<Account> Accounts { get; }

    }
}