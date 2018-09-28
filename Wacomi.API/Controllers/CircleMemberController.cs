using System;
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
    public class CircleMemberController : DataController
    {
        private readonly ICircleRepository _repo;

        public CircleMemberController(ICircleRepository repo, IAppUserRepository appUserRepository, IMapper mapper) : base(appUserRepository, mapper)
        {
            this._repo = repo;
        }

        [HttpGet("{circleId}/{userId}", Name="GetCircleMember")]
        public async Task<ActionResult> Get(int userId, int circleId){
            return Ok(_mapper.Map<CircleMemberForReturnDto>(await _repo.GetCircleMember(userId, circleId)));
        }

        [HttpGet("{circleId}", Name="GetCircleMembers")]
        public async Task<ActionResult> GetMembersForCircle(PaginationParams paginationParams, int circleId){
            var memberListFromRepo = await _repo.GetCircleMemberList(paginationParams, circleId);
            Response.AddPagination(memberListFromRepo.CurrentPage, memberListFromRepo.PageSize, memberListFromRepo.TotalCount, memberListFromRepo.TotalPages);
            return Ok(_mapper.Map<IEnumerable<CircleMemberForReturnDto>>(memberListFromRepo));
        }

        [HttpGet("{circleId}/latest", Name="GetLatestCircleMembers")]
        public async Task<ActionResult> GetLatestMembersForCircle(int circleId){
            return Ok(_mapper.Map<IEnumerable<CircleMemberForReturnDto>>(await _repo.GetLatestCircleMemberList(circleId)));
        }

        [Authorize]
        [HttpGet("{circleId}/{userId}/request", Name="GetCircleRequest")]
        public async Task<ActionResult> GetRequest(int userId, int circleId){
            return Ok(_mapper.Map<CircleRequestForReturnDto>(await _repo.GetCircleRequest(userId, circleId)));
        }

        [Authorize]
        [HttpGet("{circleId}/request", Name="GetAllCircleRequest")]
        public async Task<ActionResult> GetAllCircleRequest(int circleId){
            return Ok(_mapper.Map<IEnumerable<CircleRequestForReturnDto>>(await _repo.GetRequestsForCircle(circleId)));
        }

        [Authorize]
        [HttpPost("request")]
        public async Task<ActionResult> SendCircleRequest([FromBody]CircleRequest model){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!await this.MatchAppUserWithToken(model.AppUserId))
                return Unauthorized();
            if(await _repo.IsMember((int)model.AppUserId, model.CircleId))
                return BadRequest("既にメンバーになっています");
            var circleFromRepo = await _repo.Get<Circle>(model.CircleId);
            if(circleFromRepo == null)
                return NotFound("サークルレコードが見つかりません: " + model.CircleId);
            if(await _repo.RequestSent((int)model.AppUserId, model.CircleId))
                return BadRequest("既にリクエストしています");

            if(circleFromRepo.ApprovalRequired)
            {
                _repo.Add(model);
                await _repo.SaveAll();
                return CreatedAtRoute("GetCircleRequest", new {userId = model.AppUserId, circleId = model.CircleId}, null);
            }
            _repo.Add(new CircleMember(){
                AppUserId = (int)model.AppUserId,
                CircleId = model.CircleId,
                Role = CircleRoleEnum.MEMBER,
                DateJoined = DateTime.Now,
                DateLastActive = DateTime.Now
            });
            await _repo.SaveAll();
            return CreatedAtRoute("GetCircleMember", new {userId = model.AppUserId, circleId = model.CircleId}, null);
        }

        [Authorize]
        [HttpPost("approve")]
        public async Task<ActionResult> ApproveCircleRequest([FromBody]CircleRequest model){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var loggedInUser = await GetLoggedInUserAsync();
            if(loggedInUser == null)
                return Unauthorized();
            if(!await _repo.IsOwner(loggedInUser.Id, model.CircleId))
                return Unauthorized();
            var circleRequestFromRepo = await _repo.GetCircleRequest((int)model.AppUserId, model.CircleId);
            if(circleRequestFromRepo == null)
                return NotFound("リクエストが見つかりません");

            _repo.Delete(circleRequestFromRepo);
            //If already member, just remove request and create nothing
            if(await _repo.IsMember(model.AppUserId, model.CircleId))
            {
                await _repo.SaveAll();
                return Ok();
            }

            var newCircleMember = new CircleMember(){
                AppUserId = (int)model.AppUserId,
                CircleId = model.CircleId,
                Role = CircleRoleEnum.MEMBER,
                DateJoined = DateTime.Now,
                DateLastActive = DateTime.Now
            };
            _repo.Add(newCircleMember);
            await _repo.SaveAll();
            return CreatedAtRoute("GetCircleMember", new {userId = model.AppUserId, circleId = model.CircleId}, _mapper.Map<CircleMemberForReturnDto>(await _repo.GetCircleMember(model.AppUserId, model.CircleId)));
        }

         [Authorize]
        [HttpPut("decline")]
        public async Task<ActionResult> DeclineCircleRequest([FromBody]CircleRequest model){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var loggedInUser = await GetLoggedInUserAsync();
            if(loggedInUser == null)
                return Unauthorized();
            if(!await _repo.IsOwner(loggedInUser.Id, model.CircleId))
                return Unauthorized();
            var circleRequestFromRepo = await _repo.GetCircleRequest((int)model.AppUserId, model.CircleId);
            if(circleRequestFromRepo == null)
                return NotFound("リクエストが見つかりません");

            circleRequestFromRepo.Declined = true;
            await _repo.SaveAll();
            return Ok();
        }

        [Authorize]
        [HttpDelete("{circleId}/{userId}")]
        public async Task<ActionResult> Delete(int circleId, int userId){
            var loggedInUser = await GetLoggedInUserAsync();
            if(!await _repo.IsOwner(loggedInUser.Id, circleId) && userId != loggedInUser.Id)
                return Unauthorized();

            var circleMemberFromRepo = await _repo.GetCircleMember(userId, circleId);
            if(circleMemberFromRepo == null)
                return NotFound();

            _repo.Delete(circleMemberFromRepo);

            await _repo.SaveAll();
            return Ok();
        }
    }
}