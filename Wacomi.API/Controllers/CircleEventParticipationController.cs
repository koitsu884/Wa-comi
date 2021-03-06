using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class CircleEventParticipationController : DataController
    {
        private readonly ICircleRepository _repo;
        private readonly INotificationRepository _notificationRepo;
        public CircleEventParticipationController(ICircleRepository circleRepo, INotificationRepository notificationRepo, IAppUserRepository appUserRepository, IMapper mapper) : base(appUserRepository, mapper)
        {
            this._notificationRepo = notificationRepo;
            this._repo = circleRepo;
        }

        [HttpGet("{eventId}/{userId}", Name = "GetCircleEventParticipation")]
        public async Task<ActionResult> Get(int userId, int eventId)
        {
            return Ok(_mapper.Map<CircleEventParticipationForReturnDto>(await _repo.GetCircleEventParticipation(userId, eventId)));
        }

        [HttpGet("{eventId}", Name = "GetCircleEventParticipations")]
        public async Task<ActionResult> GetCircleEventParticipations(PaginationParams paginationParams, int eventId, [FromQuery]CircleEventParticipationStatus status)
        {
            var memberListFromRepo = await _repo.GetCircleEventParticipationList(paginationParams, eventId, status);
            Response.AddPagination(memberListFromRepo.CurrentPage, memberListFromRepo.PageSize, memberListFromRepo.TotalCount, memberListFromRepo.TotalPages);
            return Ok(_mapper.Map<IEnumerable<CircleEventParticipationForReturnDto>>(memberListFromRepo));
        }

        [HttpGet("{eventId}/latest")]
        public async Task<ActionResult> GetLatestCircleEventParticipations(int eventId)
        {
            var memberListFromRepo = await _repo.GetCircleEventParticipationList(new PaginationParams(), eventId, CircleEventParticipationStatus.Confirmed);
           return Ok(_mapper.Map<IEnumerable<CircleEventParticipationForReturnDto>>(memberListFromRepo));
        }

        [Authorize]
        [HttpPost()]
        public async Task<ActionResult> Post([FromBody]CircleEventParticipation model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!await this.MatchAppUserWithToken(model.AppUserId))
                return Unauthorized();
            var circleEventFromRepo = await _repo.Get<CircleEvent>(model.CircleEventId);
            if (circleEventFromRepo == null)
                return NotFound("イベントが見つかりません: " + model.CircleEventId);
            var existingRequest = await _repo.GetCircleEventParticipation((int)model.AppUserId, model.CircleEventId);
            if (existingRequest != null)
                return BadRequest("既にリクエストしています");

            if (circleEventFromRepo.ApprovalRequired || await _repo.IsEventFull(circleEventFromRepo.Id))
                await _notificationRepo.AddNotification(NotificationEnum.NewCircleEventParticipationRequest, (int)circleEventFromRepo.AppUserId, circleEventFromRepo);
            else
                model.Status = CircleEventParticipationStatus.Confirmed;

            _repo.Add(model);
            await _repo.SaveAll();
            return CreatedAtRoute("GetCircleEventParticipation", new { userId = model.AppUserId, eventId = model.CircleEventId }, _mapper.Map<CircleEventParticipationForReturnDto>(model));
        }

        [Authorize]
        [HttpPut("approve")]
        public async Task<ActionResult> ApproveCircleEventParticipation([FromBody]CircleEventParticipation model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var circleEventFromRepo = await _repo.Get<CircleEvent>(model.CircleEventId);
            if (circleEventFromRepo == null)
                return NotFound("イベントが見つかりません");
            if (!await this.MatchAppUserWithToken(circleEventFromRepo.AppUserId))
                return Unauthorized();
            var circleEventParticipationFromRepo = await _repo.GetCircleEventParticipation((int)model.AppUserId, model.CircleEventId);
            if (circleEventParticipationFromRepo == null)
                return NotFound("参加リクエストが見つかりません");

            circleEventParticipationFromRepo.Status = CircleEventParticipationStatus.Confirmed;
            await _notificationRepo.AddNotification(NotificationEnum.EventParticipationRequestAccepted, (int)circleEventFromRepo.AppUserId, model);
            await _repo.SaveAll();
            return Ok();
        }

        [Authorize]
        [HttpPut("cancel")]
        public async Task<ActionResult> CancelCircleEventParticipation([FromBody]CircleEventParticipation model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!await this.MatchAppUserWithToken(model.AppUserId))
                return Unauthorized();
            var eventParticipationFromRepo = await _repo.GetCircleEventParticipation((int)model.AppUserId, model.CircleEventId);
            if(eventParticipationFromRepo == null)
                return NotFound();

            eventParticipationFromRepo.Status = CircleEventParticipationStatus.Canceled;
            var firstWaitingParticipant = await _repo.GetCircleEventFirstWaitingParticipation(model.CircleEventId);
            if (firstWaitingParticipant != null)
            {
                firstWaitingParticipant.Status = CircleEventParticipationStatus.Confirmed;
                await _notificationRepo.AddNotification(NotificationEnum.EventParticipationRequestAccepted, (int)model.AppUserId, firstWaitingParticipant);
            }

            await _repo.SaveAll();
            return Ok();
        }

        [Authorize]
        [HttpDelete("{appUserId}/{eventId}")]
        public async Task<ActionResult> Delete(int appUserId, int eventId)
        {
            if (!await this.MatchAppUserWithToken(appUserId))
                return Unauthorized();

            var participationFromRepo = await _repo.GetCircleEventParticipation(appUserId, eventId);
            if(participationFromRepo == null)
                return NotFound();
            _repo.Delete(participationFromRepo);
            await _repo.SaveAll();
            return Ok();
        }
    }
}