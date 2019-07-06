using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApplication.Data;
using DatingApplication.Dtos;
using DatingApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApplication.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public UserManager<User> _UserManager { get; set; }
        public SignInManager<User> _SignInManager { get; set; }
        public AuthController(IConfiguration config,
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager) // IAuthRepository repo => no need to manullay use repository because UserManger will handel all of this by itself. 
        {
            _SignInManager = signInManager;
            _UserManager = userManager;
            _mapper = mapper;
            _config = config;

            // _repo = repo;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            // if (await _repo.UserExists(userForRegisterDto.Username))
            //     return BadRequest("Username Already Exist");
            // var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);


            // Map the requested user object with the UserManger's User object.
            var userToCreate = _mapper.Map<User>(userForRegisterDto);

            // Create the user by sending paramenters of requested user and it's password to the useManager;
            var result = await _UserManager.CreateAsync(userToCreate, userForRegisterDto.Password);

            if (result.Succeeded) 
            {
                
                // If the operation results in success it will map the requsted user to return type user to return
                var userToReturn = _mapper.Map<UserForDetailsDto>(userToCreate);
                
                // This will create the user and return full information of the user useing Users Controller's Action GetUser
                return CreatedAtRoute("GetUser", new { Controller = "Users", Id = userToCreate.Id }, userToReturn);

            }
            
            return BadRequest(result.Errors);
        }
 

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            #region Custom Login system
            // var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            // if (userFromRepo == null)
            //     return Unauthorized();
            #endregion

            var user = await _UserManager.FindByNameAsync(userForLoginDto.Username);

            var result = await _SignInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);

            if (result.Succeeded)
            {
                var appUser = await _UserManager.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.NormalizedUserName == userForLoginDto.Username.ToUpper());
            
                var userToReturn = _mapper.Map<UserForListDto>(appUser);
               
                return Ok( new { token = GeneratesJwtToken(appUser).Result, userToReturn });
            }

            return Unauthorized();
        }


        private async Task<string> GeneratesJwtToken(User user)
        {

            var claims = new List<Claim>
            {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)
            };

            var rolse = await _UserManager.GetRolesAsync(user);

            foreach (var role in rolse)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}