using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Wacomi.API.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepo;
        private readonly IDataRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public AuthController(IAuthRepository authRepo, IDataRepository repo, IConfiguration config, IMapper mapper)
        {
            this._mapper = mapper;
            this._config = config;
            this._authRepo = authRepo;
            this._repo = repo;
        }

        [HttpGet("{id}", Name = "GetUser")]
        [Authorize]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _authRepo.GetAccount(id);
            if (user == null)
                return NotFound();

            var userForReturn = _mapper.Map<AccountForReturnDto>(user);

            return new ObjectResult(userForReturn);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]UserRegistrationDto model)
        {
            if (await _authRepo.AccountExists(model.UserName, model.Email))
                ModelState.AddModelError("UserName", "The username or the email already exist");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var appUser = _mapper.Map<Account>(model);

            // if(string.IsNullOrEmpty(model.DisplayName)){
            //     member.DisplayName = appUser.UserName;
            // }
            // member.Identity = appUser;

            var user = await _authRepo.Register(appUser, model.UserType, model.Password);
            if (await _authRepo.AddAppUser(user, model.UserType) == null)
            {
                var result = await _authRepo.DelteAccount(appUser);
                return BadRequest("アカウントの作成に失敗しました");
            }

            await _authRepo.AddRoles(appUser, model.UserType == "Business" ? new string[] { "Business" } : new string[] { "Member" });

            return CreatedAtRoute("GetUser", new { id = appUser.Id }, new { });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDto userLoginDto)
        {
            var userFromRepo = await _authRepo.Login(userLoginDto.UserName, userLoginDto.Password);

            if (userFromRepo == null)
            {
                return BadRequest("ユーザーネームまたはパスワードが間違っています");
            }

            var appUser = await _repo.GetAppUserByAccountId(userFromRepo.Id);

            var roles = await this._authRepo.GetRolesForAccount(userFromRepo);
            //generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);
            var claimsIdentity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id),
                    new Claim(ClaimTypes.Name, userFromRepo.UserName),
                });
            if (roles != null)
            {
                claimsIdentity.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.Now.AddDays(1),
                Issuer = _config.GetSection("Logging:JwtIssuerOptions:Issuer").Value,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };

            Dictionary<string, object> returnValues = new Dictionary<string, object>();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            // var tokenString = tokenHandler.WriteToken(token);
            // var account = _mapper.Map<AccountForReturnDto>(userFromRepo);
            // var appUser = _mapper.Map<AppUserForReturnDto>(userFromRepo.AppUser);
            returnValues.Add("tokenString", tokenHandler.WriteToken(token));
            returnValues.Add("account", _mapper.Map<AccountForReturnDto>(userFromRepo));
            returnValues.Add("appUser", _mapper.Map<AppUserForReturnDto>(appUser));
            if (appUser != null)
            {
                returnValues.Add("photos", _mapper.Map<IEnumerable<PhotoForReturnDto>>(await _repo.GetPhotosForAppUser(appUser.Id)));
                returnValues.Add("blogs", _mapper.Map<IEnumerable<BlogForReturnDto>>(await _repo.GetBlogsForUser(appUser.Id)));
                switch (appUser.UserType)
                {
                    case "Member":
                        var member = await _repo.GetMemberProfile(appUser.UserProfileId);
                        returnValues.Add("memberProfile", _mapper.Map<MemberProfileForReturnDto>(member));

                        break;
                    case "Business":
                        var business = await _repo.GetBusinessProfile(appUser.UserProfileId);
                        returnValues.Add("businessProfile", _mapper.Map<BusinessProfileForReturnDto>(business));
                        break;
                    default:
                        break;
                }
            }

            //            return Ok(new { tokenString, account, appUser});
            return Ok(returnValues);
        }
    }
}