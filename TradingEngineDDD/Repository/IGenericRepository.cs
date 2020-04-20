using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradingEngineDDD.Repository
{
    public interface IGenericRepository
    {
        int Insert<T>(T entity) where T : class;
        bool Update<T>(T entity) where T : class;
        T Get<T>(int id) where T : class;
        List<T> GetAllWhere<T>(dynamic where) where T : class;
    }

}