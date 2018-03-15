using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model.Interfaces;
using Model.Model;

namespace Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/values")]
    public class ValuesController : Controller
    {
        private readonly IConfiguration _configuration;

        public ValuesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("Test")]
        public IActionResult Test()
        {
            var userName = User.Identity.Name;

            return Ok($"Super secret content, I hope you've got clearance for this {userName}...");
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public IActionResult RequestToken([FromBody] LoginAccount request)
        {
            var result = Helpers.GetToken(request, _configuration);

            if (!string.IsNullOrEmpty(result))
            {
                return Ok(result);
            }
            
            return BadRequest("Could not verify username and password");
        }

        //// GET api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

    }
}

