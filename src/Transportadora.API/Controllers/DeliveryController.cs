using Microsoft.AspNetCore.Mvc;

namespace Transportadora.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DeliveryController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}
