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
using Microsoft.AspNetCore.Identity;
using Wacomi.API.Helper;
using System.Net;
using System.IO;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepo;
        private readonly IDataRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<Account> _userManager;
        public AuthController(IAuthRepository authRepo,
                             IDataRepository repo,
                             IConfiguration config,
                             UserManager<Account> userManager,
                             IEmailSender emailSender,
                             IMapper mapper)
        {
            this._mapper = mapper;
            this._config = config;
            this._authRepo = authRepo;
            this._repo = repo;
            this._userManager = userManager;
            this._emailSender = emailSender;
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
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var request = Url.ActionContext.HttpContext.Request;
            var callbackUrl = request.Scheme + "://" + request.Host.Value + "/account/confirm?id=" + user.Id + "&code=" + WebUtility.UrlEncode(code);
            // var callbackUrl = Url.Action("ConfirmEmail", "Account",
            //     new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

            if (await _authRepo.AddAppUser(user, model.UserType) == null)
            {
                var result = await _authRepo.DelteAccount(appUser);
                return BadRequest("アカウントの作成に失敗しました");
            }

            await _authRepo.AddRoles(appUser, model.UserType == "Business" ? new string[] { "Business" } : new string[] { "Member" });
            await _emailSender.SendEmailAsync(model.Email, "アカウントの確認", BuildConfirmEmailContent(appUser.UserName, callbackUrl));

            return CreatedAtRoute("GetUser", new { id = appUser.Id }, new { });
        }

        [HttpPut("confirm")]
        public async Task<IActionResult> ConfirmEmail([FromBody]EmailConfirmationDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var account = await _authRepo.GetAccount(model.UserId);
            if (account == null)
            {
                return NotFound();
            }

            var result = await _userManager.ConfirmEmailAsync(account, model.Code);
            if (result.Succeeded)
            {
                var loginResult = await loginAndCreateToken(account);
                return Ok(loginResult);
            }

            return BadRequest("Eメール認証に失敗しました");
        }

        [HttpPost("password/forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordDto model){
            var account = await _authRepo.GetAccountByEmail(model.Email);
            if(account == null)
                return NotFound($"そのEメールアドレス（{model.Email}）は登録されていません");
            if(account.UserName != model.UserId)
                return BadRequest("ユーザーIDとメールアドレスが一致しません");
            if(!account.EmailConfirmed)
                return BadRequest("ユーザー認証がされていません。");

            string code = await _userManager.GeneratePasswordResetTokenAsync(account);
            var request = Url.ActionContext.HttpContext.Request;
            var callbackUrl = request.Scheme + "://" + request.Host.Value + "/account/password/reset?id=" + account.Id + "&code=" + WebUtility.UrlEncode(code);
            await _emailSender.SendEmailAsync(model.Email, "パスワードのリセット", BuildResetPasswordContent(account.UserName, callbackUrl));
            return Ok();
        }

        [HttpPost("password")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordDto model){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var account = await _authRepo.GetAccount(model.UserId);
            if (account == null)
            {
                return NotFound("ユーザーが見つかりません");
            }

            if(model.CurrentPassword == model.NewPassword){
                return BadRequest("入力した新旧パスワードが同じです");
            }

            var result = await _userManager.ChangePasswordAsync(account, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok();
            }
            else{
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
        }


        [HttpPost("password/reset")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordDeto model){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var account = await _authRepo.GetAccount(model.UserId);
            if (account == null)
            {
                return NotFound("ユーザーが見つかりません");
            }

            var result = await _userManager.ResetPasswordAsync(account, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest("パスワードのリセットに失敗しました");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDto userLoginDto)
        {
            var userFromRepo = await _authRepo.Login(userLoginDto.UserName, userLoginDto.Password);

            if (userFromRepo == null)
            {
                return BadRequest("ユーザーネームまたはパスワードが間違っています");
            }

            if (!await _authRepo.EmailConfirmed(userFromRepo))
            {
                return BadRequest("Eメール認証がまだされていません");
            }

            var loginResult = await loginAndCreateToken(userFromRepo);
            //            return Ok(new { tokenString, account, appUser});
            return Ok(loginResult);
        }
        
        private async Task<Dictionary<string, object>> loginAndCreateToken(Account userFromRepo)
        {
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
                Issuer = _config.GetSection("JwtIssuerOptions:Issuer").Value,
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
                // returnValues.Add("blogs", _mapper.Map<IEnumerable<BlogForReturnDto>>(await _repo.GetBlogsForUser(appUser.Id)));
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
            
            if (roles.Where(r => r == "Administrator").FirstOrDefault() != null)
            {
                returnValues.Add("isAdmin", true);
            }
            return returnValues;
        }

        private string BuildConfirmEmailContent(string userName, string callbackUrl)
        {
            string template = @"<p>お申し込み頂きましたアカウント情報は以下となります。</p>
<br>
<ul>
    <li>ログインID：{{userName}}</li>
    <li>パスワード：個人情報のため表示を伏せています</li>
</ul>

</p>ご本人様確認のため、下記URLへ「24時間以内」にアクセスし
アカウントの本登録を完了させて下さい。</p>
<a href='{{callBackUrl}}'>認証リンク</a>

<p>
※当メール送信後、24時間を超過しますと、セキュリティ保持のため有効期限切れとなります。
　その場合は再度、最初からお手続きをお願い致します。</p>

<p>※当メールは送信専用メールアドレスから配信されています。
　このままご返信いただいてもお答えできませんのでご了承ください。</p>

<p>※当メールに心当たりの無い場合は、誠に恐れ入りますが
　破棄して頂けますよう、よろしくお願い致します。</p>";

            template = template.Replace("{{callBackUrl}}", callbackUrl);
            template = template.Replace("{{userName}}", userName);
            return template;
        }

        private string BuildResetPasswordContent(string userName, string callbackUrl)
        {
            string template = @"
            <p>ユーザーID：{{userName}}</p>
            <p>以下のリンクをクリックして、パスワードをリセットしてください。</p>
<br>
<a href='{{callBackUrl}}'>パスワードリセット</a>

<p>
※当メール送信後、24時間を超過しますと、セキュリティ保持のため有効期限切れとなります。
　その場合は再度、最初からお手続きをお願い致します。</p>

<p>※当メールは送信専用メールアドレスから配信されています。
　このままご返信いただいてもお答えできませんのでご了承ください。</p>

<p>※当メールに心当たりの無い場合は、誠に恐れ入りますが
　破棄して頂けますよう、よろしくお願い致します。</p>";

            template = template.Replace("{{callBackUrl}}", callbackUrl);
            template = template.Replace("{{userName}}", userName);
            return template;
        }
    }
}