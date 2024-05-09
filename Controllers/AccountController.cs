using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UniversityAPP.Dto;
using UniversityAPP.Utilities;
using Infrastructure.Utilities;

namespace UniversityAPP.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 RoleManager<IdentityRole> roleManager,
                                 IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("AddRole")]
        public async Task AddRole(string RoleName)
        {
            IdentityRole role = new IdentityRole(RoleName);
            var res = await _roleManager.CreateAsync(role);
            if (!res.Succeeded)
            {
                string Errors = string.Join(Environment.NewLine, res.Errors.Select(x => x.Code + " : " + x.Description));
                throw new InvalidException(Errors);
            }
        }

        [HttpPost("Register")]
        public async Task Registeration(RegisterUserDTO userDTO)
        {
            if (!ModelState.IsValid)
                throw new InvalidException(ModelState.GetErrors());

            var AppUser = new ApplicationUser
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email
            };
            IdentityResult userRes = await _userManager.CreateAsync(AppUser, userDTO.ConfirmPassword ?? "");
            IdentityResult RoleRes = await _userManager.AddToRoleAsync(AppUser, "User");

            if (!userRes.Succeeded || !RoleRes.Succeeded)
            {
                string Errors = string.Join(Environment.NewLine, userRes.Errors.Select(x => x.Code + " : " + x.Description));
                Errors += string.Join(Environment.NewLine, RoleRes.Errors.Select(x => x.Code + " : " + x.Description));
                throw new InvalidException(Errors);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(userDTO.UserName);
                if (user is null)
                    return Unauthorized();
                else
                {
                    var Found = await _userManager.CheckPasswordAsync(user, userDTO.Password);
                    if (Found)
                    {
                        List<Claim> Claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, userDTO.UserName),
                            new Claim(ClaimTypes.NameIdentifier, user.Id),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var Roles = await _userManager.GetRolesAsync(user);

                        foreach (var Role in Roles)
                            Claims.Add(new Claim(ClaimTypes.Role, Role));

                        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretyKey"] ?? ""));
                        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken token = new JwtSecurityToken(
                            issuer: _configuration["JWT:ValidIssuer"],
                            audience: _configuration["JWT:ValidAudiance"],
                            claims: Claims,
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials: signingCredentials);

                        return Ok(new
                        {
                            Token = new JwtSecurityTokenHandler().WriteToken(token),
                            Expiration = token.ValidTo
                        });
                    }
                }

            }
            return Unauthorized();
        }
    }
}