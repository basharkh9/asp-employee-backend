using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmployeeCRUD.API.Data;
using EmployeeCRUD.API.Dtos;
using EmployeeCRUD.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeCRUD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository authRepository, IConfiguration config)
        {
            _config = config;
            _authRepository = authRepository;
        }

        /// <summary>
        /// Register user endpoint
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     Post
        ///     {
        ///         "username":"bashar",
        ///         "password":"somePass"
        ///     }
        /// </remarks>
        /// <param name="userForRegisterDto"></param>
        /// <response code="201">User Created Successfully</response>
        /// <response code="400">User Already Exists</response>
        /// <response code="401">Model State Error check the request body you send</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            if (await _authRepository.UserExist(userForRegisterDto.Username.ToLower()))
                return BadRequest("User name already exist.");

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username.ToLower()
            };

            var createdUser = await _authRepository.Register(userToCreate, userForRegisterDto.Password);

            // TODO 
            // Return CreatedAtRoute
            return StatusCode(201);
        }

        /// <summary>
        /// Login user endpoint
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     Post
        ///     {
        ///         "username":"bashar",
        ///         "password":"somePass"
        ///     }
        /// </remarks>
        /// <response code="200">login successful and the body of the response has the token</response>
        /// <response code="401">Model State Error check the request body you send</response>
        /// <response code="500">Internal Server Error</response>

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _authRepository.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}