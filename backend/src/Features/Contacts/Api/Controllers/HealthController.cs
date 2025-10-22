using Microsoft.AspNetCore.Mvc;

namespace RecruitmentApp.Features.Contacts.Api.Controllers;

public class HealthController : ControllerBase
{
    [HttpGet("/api/health")]
    public IActionResult CheckHealth()
    {
        return Ok("Healthy");
    }
}