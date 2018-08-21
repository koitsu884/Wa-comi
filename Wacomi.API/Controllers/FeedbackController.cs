using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public FeedbackController(IAdminDataRepository repo, IEmailSender emailSender)
        {
            this._emailSender = emailSender;
            this._repo = repo;
        }

        [HttpGet("{id}", Name = "GetFeedback")]

        public async Task<ActionResult> GetFeedback(int id)
        {
            return Ok(await _repo.GetFeedback(id));
        }

        [HttpPost()]
        public async Task<ActionResult> SendFeedback([FromBody]Feedback model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _repo.Add(model);
            await _repo.SaveAll();
            //Should send email here
            await _emailSender.SendEmailAsync(model.Email, "Wa-コミ", this._toEmail, model.Title, model.Content);
            return CreatedAtRoute("GetFeedback", new { id = model.Id }, model);

        }
    }
}