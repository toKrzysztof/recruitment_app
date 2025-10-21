using Microsoft.AspNetCore.Mvc;

namespace RecruitmentApp.Features.Contacts.Api.Base;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase;