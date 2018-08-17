using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Models;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class AppUserController : DataController
    {
        public AppUserController(IDataRepository repo, IMapper mapper) : base(repo, mapper){}
        
        [HttpGet("{id}" , Name = "GetAppUser")]
        public async Task<IActionResult> Get(int id){
           var appUser = await _repo.GetAppUser(id);
           if(appUser == null)
                return NotFound();

            var appUserForReturn = _mapper.Map<AppUserForReturnDto>(appUser);

            return Ok(appUserForReturn);
        }

        [HttpGet("{id}/detail")]
        public async Task<IActionResult> GetDetails(int id){
           var appUser = await _repo.GetAppUser(id);
           if(appUser == null)
                return NotFound("ユーザー情報が見つかりませんでした。ID:" + id);

            Dictionary<string, object> returnValues = new Dictionary<string,object>();
            returnValues.Add("appUser", _mapper.Map<AppUserForReturnDto>(appUser));
            
            switch(appUser.UserType){
                case "Member":
                    returnValues.Add("memberProfile", _mapper.Map<MemberProfileForReturnDto>(await _repo.GetMemberProfile(appUser.UserProfileId)));
                    break;
                case "Business":
                    returnValues.Add("businessProfile", _mapper.Map<BusinessProfileForReturnDto>(await _repo.GetBusinessProfile(appUser.UserProfileId)));
                    break;
            }

            return Ok(returnValues);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]AppUserUpdateDto updateUserDto){
            var appUserFromRepo = await _repo.GetAppUser(id);
            if(appUserFromRepo == null)
                return NotFound();

            if(!MatchUserWithToken(appUserFromRepo.AccountId))
                return Unauthorized();

            if(updateUserDto.CityId == 0)
                updateUserDto.CityId = null;
            if(updateUserDto.MainPhotoId == 0)
                updateUserDto.MainPhotoId = null;
            _mapper.Map(updateUserDto, appUserFromRepo);

            return Ok(await _repo.SaveAll());
        }

        [Authorize]
        [HttpPut("{id}/{photoId}")]
        public async Task<IActionResult> ChangeMainPhoto(int id, int photoId){
            var appUserFromRepo = await _repo.GetAppUser(id);
            if(appUserFromRepo == null)
                return NotFound("The user was not found");

            if(!await _repo.RecordExist("Photo", photoId))
                return NotFound("Photo " + photoId + " was not found");

            if(!MatchUserWithToken(appUserFromRepo.AccountId))
                return Unauthorized();

            appUserFromRepo.MainPhotoId = photoId;

            return Ok(await _repo.SaveAll());
        }
    }
}