using System.Web.Http;

namespace TradingEngineDDD.Controllers
{
    [RoutePrefix("test")]
    public class TestController : ApiController
    {
        // GET

        [Route("")]
        [HttpGet]
        public IHttpActionResult Idx()
        {
         
            return Ok("Hello World");
        }
    }
}