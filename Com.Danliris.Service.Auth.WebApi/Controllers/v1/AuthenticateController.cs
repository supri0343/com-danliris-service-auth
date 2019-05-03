using AutoMapper;
using Com.Danliris.Service.Auth.Lib.BusinessLogic.Interfaces;
using Com.Danliris.Service.Auth.Lib.BusinessLogic.Services;
using Com.Danliris.Service.Auth.Lib.ViewModels;
using Com.Danliris.Service.Auth.WebApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Auth.WebApi.Controllers.v1
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/authenticate")]
    public class AuthenticateController : Controller
    {
        public static readonly string ApiVersion = "1.0.0";
        public readonly IAccountService _accountService;
        public readonly IMapper Mapper;
        public IConfiguration Configuration { get; }

        public AuthenticateController(IAccountService accountService, IMapper mapper, IConfiguration configuration)
        {
            Mapper = mapper;
            _accountService = accountService;
            Configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginViewModel User)
        {
            try
            {
                var account = await _accountService.Authenticate(User.Username, User.Password);

                if (account == null)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                    return NotFound(Result);
                }
                else
                {
                    AccountViewModel viewModel = Mapper.Map<AccountViewModel>(account);

                    string Secret = Configuration.GetValue<string>(Constant.SECRET) ?? Configuration[Constant.SECRET];
                    SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var header = new JwtHeader(credentials);

                    var payload = new JwtPayload
                    {
                        { "username", viewModel.username}
                    };

                    payload["profile"] = new {
                        viewModel.profile.firstname,
                        viewModel.profile.lastname,
                        viewModel.profile.gender,
                        viewModel.profile.dob,
                        viewModel.profile.email
                    };

                    string jsonRes = "{";

                    foreach(var item in viewModel.roles.SelectMany(x => x.permissions).GroupBy(x => x.unit.code).Select(g => g.First()))
                    {
                        jsonRes = jsonRes + "'" + item.unit.code + "'" + " : " + item.permission + ",";
                    }
                    jsonRes = jsonRes.Remove(jsonRes.Length - 1) + "}";

                    var jsonObject = JObject.Parse(jsonRes);

                    payload["permission"] = jsonObject;

                    payload["iat"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                    var secToken = new JwtSecurityToken(header, payload);
                    var handler = new JwtSecurityTokenHandler();

                    var tokenString = handler.WriteToken(secToken);

                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                        .Ok();

                    Result.Add("data", tokenString);

                    return Ok(Result);
                }

               
            }
            catch (Exception ex)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, ex.Message)
                    .Fail();

                return BadRequest(Result);
            }
        }
    }
}
