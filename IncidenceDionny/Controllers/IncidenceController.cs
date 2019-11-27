using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IncidenceDionny.DB;
using IncidenceDionny.DTO;
using IncidenceDionny.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IncidenceDionny.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidenceController : ControllerBase
    {
        private IConfiguration _configuration;
        public IncidenceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet("GetToken")]
        public ActionResult GetToken([FromQuery]User user)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,"dionny.prensa@solvex.com.do"),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim("Name",user.UserName)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Secret"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _configuration["Token:Issuer"],
                    _configuration["Token:Audience"],
                    claims,
                    notBefore: DateTime.UtcNow, // Fecha de generacion
                    expires: DateTime.UtcNow.AddSeconds(40), // Fecha de duracion del token
                    signingCredentials: credentials
                    );
                string tk = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(tk);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        // GET api/Incidence
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<IEnumerable<string>> Get()
        {
            try
            {
                var db = new Contest(_configuration["ConnectionStrings:DefaultConnection"]);
                return Ok(db.SelectAll());
            }
            catch (Exception ex)
            {
                // Lo que necesites hacer.
                return BadRequest();
            }
        }

        // GET api/Incidence/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                var db = new Contest(_configuration["ConnectionStrings:DefaultConnection"]);
                return Ok(db.GetIncidence(id));
            }
            catch (Exception ex)
            {
                // Lo que necesites hacer.
                return BadRequest();
            }
        }

        // POST api/Incidence
        [HttpPost]
        public ActionResult Post([FromBody] Incidence incidence)
        {
            try
            {
                var db = new Contest(_configuration["ConnectionStrings:DefaultConnection"]);
                if (db.Insert(incidence) > 0)
                    return Ok();
                return BadRequest();
            }
            catch (Exception ex)
            {
                // Lo que necesites hacer.
                return BadRequest();
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public void Delete(int id)
        {
        }
    }
}
