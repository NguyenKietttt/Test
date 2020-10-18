using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ncov_BLL;
using Ncov_DAL;
using Ncovi_Common.Rsp;

namespace Ncovi_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CasesController : ControllerBase
    {
        private readonly CasesSvc _svc;

        public CasesController()
        {
            _svc = new CasesSvc();
        }

        [HttpGet("Add")]
        public IActionResult AddCases()
        {
            var res = new SingleRsp();

            res = _svc.AddCases();

            return Ok(res);
        }

        [HttpPost("Get-Case-Pages")]
        public IActionResult GetCasePages([FromBody]PageReq req)
        {
            var res = new SingleRsp();
            var temp = _svc.GetCasePages(req.Keyword = "", req.Page, req.Size);

            res.Data = temp; 

            return Ok(res);
        }

        [HttpPost("Get-Global")]
        public IActionResult GetGlobal()
        {
            var res = new SingleRsp();
            var temp = _svc.GetGlobalCases();

            res.Data = temp;

            return Ok(res);
        }

        [HttpPost("Get-Cases-By-CountryID")]
        public IActionResult GetCase_By_CountryID([FromBody]string CountryID)
        {
            var res = new SingleRsp();
            var temp = _svc.GetCase_By_CountryID(CountryID);

            res.Data = temp;

            return Ok(res);
        }

        [HttpPost("Get-Newest-Case-By-CountryID")]
        public IActionResult GetNewestCases_ByCountryID([FromBody]string CountryID)
        {
            var res = new SingleRsp();
            var temp = _svc.GetNewestCases_ByCountryID(CountryID);

            res.Data = temp;

            return Ok(res);
        }
    }
}