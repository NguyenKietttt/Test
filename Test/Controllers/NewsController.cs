using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ncov_BLL;
using Ncov_DAL;
using Ncovi_Common.Rsp;

namespace Ncov_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly NewsSvc _svc;

        public NewsController()
        {
            _svc = new NewsSvc();
        }

        [HttpGet("Add")]
        public IActionResult AddNews()
        {
            var res = new SingleRsp();

            res = _svc.AddNews();

            return Ok(res);
        }

        [HttpPost("Get-all")]
        public IActionResult GetCasePages([FromBody]PageReq req)
        {
            var res = new SingleRsp();
            var temp = _svc.GetNewsPage(req.Keyword = "", req.Page, req.Size);

            res.Data = temp;

            return Ok(res);
        }
    }
}