using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SessionMonitor.WebSvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
    }
}