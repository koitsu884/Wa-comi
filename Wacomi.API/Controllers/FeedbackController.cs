using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wacomi.API.Data;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class FeedbackController : Controller
    {
        private readonly IAdminDataRepository _repo;
        private readonly IEmailSender _emailSender;
        private readonly string _toEmail = "kazunori.hayashi.nz@outlook.com";
        private readonly ILogger<FeedbackController> _logger;

        public FeedbackController(IAdminDataRepository repo, IEmailSender emailSender, ILogger<FeedbackController> logger)
        {
            this._logger = logger;
            this._emailSender = emailSender;
            this._repo = repo;
        }

        [HttpGet("{id}", Name = "GetFeedback")]

        public async Task<ActionResult> Get(int id)
        {
            return Ok(await _repo.GetFeedback(id));
        }

        [HttpPost()]
        public async Task<ActionResult> Post([FromBody]Feedback model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _repo.Add(model);
            await _repo.SaveAll();
            //Should send email here
            try
            {
                await _emailSender.SendEmailAsync("noreply@wa-comi.com", model.SenderName + "<" + model.Email + ">", this._toEmail, model.Title, model.Content);
            }
            catch (System.Exception ex)
            {
                this._logger.LogError(ex.Message);
            }
            return CreatedAtRoute("GetFeedback", new { id = model.Id }, model);

        }
    }
}