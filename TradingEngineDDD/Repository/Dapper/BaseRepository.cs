using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Dapper;
using Dapper.Contrib.Extensions;
namespace TradingEngineDDD.Repository.Dapper
{
    public abstract class BaseRepository: IGenericRepository
    {
        private readonly string _connectionString;

        protected BaseRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["TEST-CONNECTION-STRING"]?.ConnectionString; //get from web config
        }
      
        protected IDbConnection DbConnection()
        {
            return  new SqlConnection(_connectionString);
        }


        //https://stackoverflow.com/questions/43032797/
        protected string TableName<T>()
        {
            // Check if we've already set our custom table mapper to TableNameMapper.
            if (SqlMapperExtensions.TableNameMapper != null)
                return SqlMapperExtensions.TableNameMapper(typeof(T));

            // If not, we can use Dapper default method "SqlMapperExtensions.GetTableName(Type type)" which is unfortunately private, that's why we have to call it via reflection.
            string getTableName = "GetTableName";
            MethodInfo getTableNameMethod = typeof(SqlMapperExtensions).GetMethod(getTableName, BindingFlags.NonPublic | BindingFlags.Static);

            if (getTableNameMethod == null)
                throw new System.ArgumentOutOfRangeException($"Method '{getTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.");

            return getTableNameMethod.Invoke(null, new object[] { typeof(T) }) as string;
        }

        
        public int Insert<T>(T entity) where T : class
        {
            using (var db = DbConnection())
            {
                return (int)db.Insert(entity);
            }
        }

        public bool Update<T>(T entity) where T : class
        {
            using (var db = DbConnection())
            {
                return db.Update(entity);
            }
        }

        public T Get<T>(int id) where T : class
        {
            using (var db = DbConnection())
            {
                return db.Get<T>(id);
            }
        }

        public List<T> GetAllWhere<T>(dynamic where) where T : class
        {
            var tblName = TableName<T>();
            return GetAllWhere<T>(tblName, where);
        }
        private List<T> GetAllWhere<T>(string tbl, dynamic where = null) where T : class
        {
            using (var db = DbConnection())
            {
                var sql = $"SELECT * FROM {tbl}";
                if (where == null)
                {
                    return new List<T>();
                }
                var paramNames = GetParamNames((object)where);
                var w = string.Join(" AND ", paramNames.Select(p => $"{p} = @{p}"));
                sql += " WHERE " + w;
                return db.Query<T>(sql, where as object).ToList() ?? new List<T>();
            }
        }
        private IEnumerable<string> GetParamNames(object o)
        {
            if (o is DynamicParameters parameters)
            {
                return parameters.ParameterNames.ToList();
            }
            var paramNames = new List<string>();
            foreach (var prop in o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.GetGetMethod(false) != null))
            {
                paramNames.Add(prop.Name);
            }
            return paramNames;
        }

    }
}