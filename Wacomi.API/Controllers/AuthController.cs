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

namespace Wacomi.API.Controllers
{
     [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            this._mapper = mapper;
            this._config = config;
            this._repo = repo;
        }

        [HttpGet("{id}" , Name = "GetUser")]
        [Authorize]
        public async Task<IActionResult> GetUser(string id){
           var user = await _repo.GetAppUser(id);
           if(user == null)
                return NotFound();

            var userForReturn = _mapper.Map<AppUserForReturnDto>(user);

            return new ObjectResult(userForReturn);
        }

         [HttpPost]
        public async Task<IActionResult> Register([FromBody]UserRegistrationDto model)
        {
            if (await _repo.AppUserExists(model.UserName, model.Email))
                ModelState.AddModelError("UserName", "The username or the email already exist");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           if(string.IsNullOrEmpty(model.DisplayName)){
                model.DisplayName = model.UserName;
            }
            var appUser = _mapper.Map<AppUser>(model);
            
            // if(string.IsNullOrEmpty(model.DisplayName)){
            //     member.DisplayName = appUser.UserName;
            // }
            // member.Identity = appUser;

            var user = await _repo.Register(appUser, model.Password);
            await _repo.AddRoles(appUser, appUser.UserType == "Business" ? new string[] {"Business"} : new string[] {"Member"});
            
            return CreatedAtRoute("GetUser", new {id = appUser.Id}, new {});
        }

        
         [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDto userLoginDto)
        {
            var userFromRepo = await _repo.Login(userLoginDto.UserName, userLoginDto.Password);

            if (userFromRepo == null)
            {
                return BadRequest("ユーザーネームまたはパスワードが間違っています");
            }

            var roles = await this._repo.GetRolesForAppUser(userFromRepo);
            //generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);
            var claimsIdentity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id),
                    new Claim(ClaimTypes.Name, userFromRepo.UserName),
                });
            if(roles != null){
                claimsIdentity.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var appUser = _mapper.Map<AppUserForReturnDto>(userFromRepo);

            return Ok(new { tokenString, appUser});
        }
    }
}