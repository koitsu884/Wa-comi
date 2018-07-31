using System;
using System.Collections.Generic;
using System.Security.Claims;
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
    public class ClanSeekController : DataController
    {
        const int CLANSEEK_MAX = 5;
        public ClanSeekController(IDataRepository repo, IMapper mapper) : base(repo, mapper) { }

        [HttpGet("{id}", Name = "GetClanSeek")]
        public async Task<ActionResult> GetClanSeek(int id)
        {
            var clanSeekFromRepo = await _repo.GetClanSeek(id);
            var clanSeekForReturn = _mapper.Map<ClanSeekForReturnDto>(clanSeekFromRepo);
            return Ok(clanSeekForReturn);
        }

        [HttpGet()]
        //        public async Task<ActionResult> GetClanSeeks(int? categoryId, int? cityId, bool? latest)
        public async Task<ActionResult> GetClanSeeks(PaginationParams paginationParams, int? categoryId, int? cityId)
        {
            //var clanSeeks = await _repo.GetClanSeeks(categoryId, cityId, latest);
            var clanSeeks = await _repo.GetClanSeeks(paginationParams, categoryId, cityId);
            var clanSeeksForReturn = this._mapper.Map<IEnumerable<ClanSeekForReturnDto>>(clanSeeks);

            Response.AddPagination(clanSeeks.CurrentPage, clanSeeks.PageSize, clanSeeks.TotalCount, clanSeeks.TotalPages);
            return Ok(clanSeeksForReturn);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetByUser(int userId)
        {
            var clanSeeks = await _repo.GetClanSeeksByUser(userId);
            return Ok(this._mapper.Map<IEnumerable<ClanSeekForReturnDto>>(clanSeeks));
        }

        [HttpGet("user/{userId}/count")]
        public async Task<int> GetCountByUser(int userId){
            return await this._repo.GetClanSeeksCountByUser(userId);
        }

        [HttpGet("categories")]
        public async Task<ActionResult> GetClanSeekCategories()
        {
            return Ok(await _repo.GetClanSeekCategories());
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddClanSeek([FromBody]ClanSeekForCreationDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _repo.GetClanSeeksCountByUser(model.AppUserId) >= CLANSEEK_MAX)
                return BadRequest($"仲間募集投稿は1人{CLANSEEK_MAX}つまでです。不要な投稿を削除してから、改めて投稿してください。");
            
            var newClanSeek = this._mapper.Map<ClanSeek>(model);
            _repo.Add(newClanSeek);
            if (await _repo.SaveAll() > 0)
            {
                return CreatedAtRoute("GetClanSeek", new { id = newClanSeek.Id }, newClanSeek);
            }
            return BadRequest("Failed to add clanseek");
        }

        [HttpPut()]
        [Authorize]
        public async Task<ActionResult> UpdateClanSeek([FromBody]ClanSeekUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var clanSeekFromRepo = await this._repo.GetClanSeek(model.Id);
            if (clanSeekFromRepo == null)
            {
                return NotFound();
            }
            if (!await MatchAppUserWithToken(clanSeekFromRepo.AppUserId))
            {
                return Unauthorized();
            }

            model.LastActive = DateTime.Now;
            _mapper.Map(model, clanSeekFromRepo);
            if (await _repo.SaveAll() > 0)
            {
                return Ok();
            }
            return BadRequest("募集内容の変更に失敗しました。");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteClanSeek(int id)
        {
            var clanSeek = await _repo.GetClanSeek(id);
            if (clanSeek == null)
            {
                return NotFound();
            }
            if (!await MatchAppUserWithToken(clanSeek.AppUserId))
            {
                return Unauthorized();
            }
            _repo.Delete(clanSeek);

            if (await _repo.SaveAll() > 0)
                return Ok();

            return BadRequest("Failed to delete the clan seek");
        }
    }
}