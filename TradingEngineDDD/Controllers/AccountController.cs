using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TradingEngineDDD.Application;
using TradingEngineDDD.Controllers.ModelParam;
using TradingEngineDDD.Models.ValueObject;

namespace TradingEngineDDD.Controllers
{

    [RoutePrefix("account")]
    public class AccountController : ApiController
    {

        private readonly IAccountDetailsService _accountDetailsService;

        public AccountController(IAccountDetailsService accountDetailsService)
        {
            _accountDetailsService = accountDetailsService;
        }


        [Route("{id:int}")]
        [HttpGet]
        public IHttpActionResult GetAccountList([FromUri]int id)
        {

            return Ok(_accountDetailsService.GetAccountsUsingClientId(id));
        }

        [Route("{id:int}/currency-exchange")]
        [HttpPost]
        public IHttpActionResult CurrencyExchangeRequest([FromUri] int id, [FromBody] CurrencyExchangeParam param)
        {
            return Ok(_accountDetailsService.CurrencyExchangeRequest(id, param.CurrencyFrom, param.CurrencyTo,
                param.Amount));
        }
        [Route("{id:int}/fund-transfer")]
        [HttpPost]
        public IHttpActionResult FundTransferRequest([FromUri] int id, [FromBody] FundTransferParam param)
        {
            return Ok(_accountDetailsService.FundTransferRequest(param.RecipientId,param.SenderId,param.Currency, param.Amount));
        }
    }

}
