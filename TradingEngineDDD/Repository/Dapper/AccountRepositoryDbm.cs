using System;
using System.Collections.Generic;
using System.Linq;
using Dapper.Contrib.Extensions;

namespace TradingEngineDDD.Repository.Dapper
{
    [Table("TblAccountRepository")]
    public class AccountRepositoryDbm
    {
        public string CurrencyCode { get; set; }
        public string CurrencyDescription { get; set; }
        public int AccountId { get; set; }
        public decimal Balance { get; set; } 
        public int ClientId { get; set; }
    }
}