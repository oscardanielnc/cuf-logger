
using cuf_admision_domain.Services;
using Microsoft.AspNetCore.Mvc;
using cuf_admision_domain.Entities.Responses;
using cuf_admision_domain.Models;

namespace cuf_admision_app_service.Controllers.v1.Academic
{
    [ApiController]
    [Route("/api/v1/logger")]
    public class AcademicController : ControllerBase
    {

        private readonly IUtilsService _utilsService;

        public AcademicController(IUtilsService utilsService)
        {
            _utilsService = utilsService;
        }

        [HttpPost]
        [Route("simple")]
        public ActionResult LogSimple([FromBody] LogModel body)
        {
            try
            {
                var response  = _utilsService.LogSimple(body);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CustomErrorResponse
                {
                    code = "500",
                    message = ex.Message,
                    data = ex.StackTrace?.ToString() ?? "--"
                });
            }
        }
    }
}

