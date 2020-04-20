using System.Collections.Generic;
using System.Web.UI;
using TradingEngineDDD.Models.ValueObject;

namespace TradingEngineDDD.Repository
{
    public interface IAccountRepository{
    List<Account> FindByClientId(ClientId clientId);
    int Insert(Account param);
    bool Update(Account param);
    Account Get(int id);
    }
}