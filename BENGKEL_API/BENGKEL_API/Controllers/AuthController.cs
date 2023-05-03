using BENGKEL_API.Helper;
using BENGKEL_API.Models;
using BENGKEL_API.Viewmodel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BENGKEL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SidorejoBengkelContext context;

        public AuthController(SidorejoBengkelContext context)
        {
            this.context = context;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(UserDto user)
        {
            var customer = await context.Customers.FirstOrDefaultAsync(x => x.Email == user.Email);
            var employee = await context.Employees.FirstOrDefaultAsync(x => x.Email == user.Email);

            if (customer is null && employee is null) return NotFound("Data Not Found");
            
            if (customer != null)
            {
                if (customer.Password != user.Password ) return BadRequest("Incorrect Password");
            }
            if (employee != null)
            {
                if (employee.Password != user.Password) return BadRequest("Incorrect Password");
            }

            var token = CreateToken(user);
            return Ok(token);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<string>> Register(UserDto user)
        {
            var strgs = new List<string>()
            {
                user.Name,
                user.Email,
                user.Password,
                user.Address,
                user.Phone
            };
            var validEmpty = Validation.ValidationString(strgs);
            if (!validEmpty) return BadRequest("All input must be filled");
            
            var validEmail = Validation.ValidationInEmail(user.Email);
            if (!validEmail) return BadRequest("Invalid Email Format");
            
            var validPwd = Validation.ValidationInPassword(user.Password);
            if (!validPwd) return BadRequest(validPwd);

            var cData = new Customer()
            {
                CustomerId = Generator.GenerateID(Master.Customer.ToString(), 5),
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Address = user.Address,
                Phone = user.Phone
            };
            context.Customers.Add(cData);
            await context.SaveChangesAsync();
            var token = CreateToken(user);
            return Ok(token);
        }

        private string CreateToken(UserDto user)
        {
            var claims = new List<Claim>();
            if (user.Email == "admin")
            {
                var claim = new Claim(ClaimTypes.Role, "Admin");
                claims.Add(claim);
            }
            else
            {
                var claim = new Claim(ClaimTypes.Role, "Customer");
                claims.Add(claim);
            }
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(UserDto.KEY));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
