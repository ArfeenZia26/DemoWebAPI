using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v1/[controller]")]
public class BaseApiControllerV1 : ControllerBase
{

}

[ApiVersion("2.0")]
[ApiController]
[Route("api/v2/[controller]")]
public class BaseApiControllerV2 : ControllerBase
{

}