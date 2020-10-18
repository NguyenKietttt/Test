using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ncov_BLL;
using Ncovi_Common.Rsp;

namespace Ncovi_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly CountrySvc _svc;

        public CountryController()
        {
            _svc = new CountrySvc();
        }

        [HttpPost("Get-all-country-have-cases")]
        [Produces("application/json")]
        public IActionResult GetAllCountry_Have_Cases()
        {
            var res = new SingleRsp();

            res = _svc.GetAllCountry_Have_Cases();

            return Ok(res);
        }
    }
}