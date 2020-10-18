using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ncov_BLL;
using Ncovi_Common.Rsp;

namespace Ncov_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly PatientSvc _svc;

        public PatientController()
        {
            _svc = new PatientSvc();
        }

        [HttpGet("Add")]
        public IActionResult AddCases()
        {
            _svc.CheckPatients();

            return Ok();
        }

        [HttpPost("Get-all")]
        public IActionResult GetAllCountry()
        {
            var res = new SingleRsp();

            res = _svc.GetListPatients();

            return Ok(res);
        }
    }
}