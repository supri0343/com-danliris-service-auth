using Com.Danliris.Service.Auth.Lib.BusinessLogic.Interfaces;
using Com.Danliris.Service.Auth.Lib.Services.IdentityService;
using Com.Danliris.Service.Auth.WebApi.Utilities;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Auth.WebApi.Controllers.v1
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/accounts-monitoring")]
    [Authorize]
    public class MonitoringAccountController : Controller
    {

        public static readonly string ApiVersion = "1.0.0";
        public readonly IAccountMonitoringService _accountMonitoringService;

        public MonitoringAccountController(IIdentityService identityService, IAccountMonitoringService _accountMonitoringService) 
        {
            this._accountMonitoringService = _accountMonitoringService;
        }

        [HttpGet("xls")]
        public IActionResult GetXls([FromQuery] int userId, [FromQuery] string menu)
        {

            try
            {
                byte[] xlsInBytes;
              
                string filename;

                var xls = _accountMonitoringService.GetExcel(userId, menu);


                filename = String.Format("Monitoring Akun - {0}.xlsx", DateTime.UtcNow.ToString("dd-MMM-yyyy"));

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;

            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }


    }
}
