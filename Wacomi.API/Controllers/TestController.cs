using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Helper;

namespace Wacomi.API.Controllers
{
    public class MessageTest{
        public string Title{get; set;}
        public string Body{get; set;}
    }

    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private IEmailSender _emailSender;

        public TestController(IEmailSender emailSender){
            this._emailSender = emailSender;
        }

        [HttpPost()]
        public async Task<ActionResult> Post([FromBody]MessageTest model){
            var request = Url.ActionContext.HttpContext.Request;
            var url = request.Scheme + "://" + request.Host.Value + "/confirm";
//            await _emailSender.SendEmailAsync("kazunori.hayashi.nz@outlook.com", model.Title, model.Body);
            return Ok(url);
        }
        
    }
}