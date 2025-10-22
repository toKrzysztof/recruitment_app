using Microsoft.AspNetCore.Mvc;

namespace RecruitmentApp.Shared.Api;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase;