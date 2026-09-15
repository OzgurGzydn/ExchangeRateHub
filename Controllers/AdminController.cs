using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ExchangeRateHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AdminController : ControllerBase
    {
        [HttpGet("panel")]
        public IActionResult GetAdminPanel()
        {
            return StatusCode(StatusCodes.Status403Forbidden, "Access denied. Admin role is required.");
        }
    }
}