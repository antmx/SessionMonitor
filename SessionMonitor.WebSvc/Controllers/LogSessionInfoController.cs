using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SessionMonitor.Common;

namespace SessionMonitor.WebSvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogSessionInfoController : ControllerBase
    {
        // GET api/LogSessionInfo
        [HttpGet]
        public ActionResult<IEnumerable<SessionInfo>> Get()
        {
            return new SessionInfo[] {
                new SessionInfo{ AuthenticationPackage = "Foo" }
            };
        }

        //// GET api/LogSessionInfo/5
        //[HttpGet("{id}")]
        //public ActionResult<SessionInfo> Get(int id)
        //{
        //    return new SessionInfo();
        //}

        // POST api/LogSessionInfo
        [HttpPost]
        public bool Post([FromBody] SessionInfo[] sessionInfo)
        {
            // todo - insert new SessionInfos into database
            return true;
        }

        // PUT api/LogSessionInfo/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] SessionInfo sessionInfo)
        {
            // todo - update existing SessionInfo item in database
        }

        //// DELETE api/LogSessionInfo/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}