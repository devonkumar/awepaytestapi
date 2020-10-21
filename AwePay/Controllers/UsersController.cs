using AutoMapper;
using AwePay.CQRS;
using AwePay.Domains;
using AwePay.DTO;
using AwePay.Helpers;
using AwePay.Services;
using AwePay.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AwePay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private IUserService _userService;
        private IMapper _mapper;

        private readonly AppSettings _appSettings;

        public UsersController(
        IUserService userService,
        IMapper mapper,
        IOptions<AppSettings> appSettings)
            {
                _userService = userService;
                _mapper = mapper;
                _appSettings = appSettings.Value;
            }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserDto userDto)
        {
            dynamic user;


                user = _userService.AuthUser(userDto.UName, userDto.Paswd);



            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            ClaimsIdentity getClaimsIdentity()
            {
                return new ClaimsIdentity(
                    getClaims()
                    );

                Claim[] getClaims()
                {
                    List<Claim> claims = new List<Claim>();
                    // claims.Add(new Claim(ClaimTypes.Name, user.Id.ToString()));
                    claims.Add(new Claim("UserId", user.UserId.ToString()));
                    claims.Add(new Claim("UName", user.UName.ToString()));
                    claims.Add(new Claim("Id", user.Id.ToString()));
                    claims.Add(new Claim("isAdmin", "0"));
   
                    return claims.ToArray();
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = getClaimsIdentity(),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                MemId = user.Id,
                UserId = user.UserId,
                userName = user.UName,
                firstName = user.FName,
                lastName = user.LName,
                token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]MemCreate userDto)
        {

            try
            {

                    var user = _mapper.Map<User>(userDto);
                    _userService.CreateUser(user, userDto.Paswd);

                return Ok();
            }
            catch (DRException ex)
            {
                // return error message if there was an exception
                return BadRequest(new DRError() { Code = ex.Code, Message = ex.Message });

            }
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] PageQuery @params)
        {
            var users = await _userService.GetAllUsersAsync(@params);

            if (users is null || users.Result is null)
                return Ok(new DRCollection<UserDto>(default));

            var dto = _mapper.Map<IEnumerable<UserDto>>(users.Result);
            var col = new DRCollection<UserDto>(dto, @params.PageNo, @params.PageSize, users.QueryCount);
            return Ok(col);
        }


        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {

            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                var userDto = _mapper.Map<UserDto>(user);
                return Ok(new DRSingle<UserDto>(userDto));
            }
            catch (Exception ex)
            {
                return BadRequest(new DRError() { Code = "0", Message = ex.Message, InnerMsg = ex.InnerException.Message });
            }
        }


        [HttpPut("user/{id}")]
        public IActionResult UpdateUser(int id, [FromBody]UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                user.Id = id;
                _userService.UpdateUser(user, userDto.Paswd, userDto.Chpass);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("user/{id}")]
        public IActionResult DeleteUser(int id)
        {
            _userService.DeleteUser(id);
            return Ok();
        }






    }
}
