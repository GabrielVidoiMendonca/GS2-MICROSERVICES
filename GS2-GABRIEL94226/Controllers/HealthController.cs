using Microsoft.AspNetCore.Mvc;

namespace GS2_GABRIEL94226.Controllers
{
    
        [ApiController]
        [Route("api/[controller]")]
        public class HealthController : ControllerBase
        {
            [HttpGet("health")]
            public IActionResult GetHealthStatus()
            {
                return Ok(new { status = "Service is running" });
            }
        }
    }