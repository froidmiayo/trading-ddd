using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;
using Dapper.Contrib.Extensions;
using TradingEngineDDD.Models.ValueObject;

namespace TradingEngineDDD.Repository.Dapper
{
    public class AccountRepository: BaseRepository,IAccountRepository
    {
      
        public List<Account> FindByClientId(ClientId clientId)
        {
            var result = new List<Account>();

            
            var data = GetAllWhere<AccountRepositoryDbm>(new {id = clientId.Value});
            if (data != null)
            {
                result.AddRange(data.Select(AccountRepositoryMapper.ToAccount));
            }
            return result;
        }

        public Account Get(int id)
        {
            return AccountRepositoryMapper.ToAccount(Get<AccountRepositoryDbm>(id));
        }

        public int Insert(Account param)
        {
            return Insert(AccountRepositoryMapper.ToAccountDbm(param));
        }

        public bool Update(Account param)
        {
            return Update(AccountRepositoryMapper.ToAccountDbm(param));
        }
    }
}