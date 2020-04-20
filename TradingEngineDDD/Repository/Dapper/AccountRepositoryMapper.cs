using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using TradingEngineDDD.Models;
using TradingEngineDDD.Models.ValueObject;

namespace TradingEngineDDD.Repository.Dapper
{
    public static class AccountRepositoryMapper
    {
        public static Account ToAccount(AccountRepositoryDbm param)
        {
            return new Account(
                currency : new Currency(param.CurrencyCode),
                id: new AccountId(param.AccountId),
                bal: new AccountMoney(param.Balance),
                clientId:new ClientId(param.ClientId));
        }

        public static AccountRepositoryDbm ToAccountDbm(Account param)
        {
            return new AccountRepositoryDbm
            {
                AccountId = param.AccountId.Value,
                Balance = param.Balance.Value,
                CurrencyCode = param.Currency.Code,
                ClientId = param.ClientId.Value
            };
        }
    }
}