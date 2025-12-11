using Microsoft.AspNetCore.Mvc;
using Transportadora.Application.Supplier.Services;

namespace Transportadora.API.Controllers.Supplier;

[ApiController]
[Route("supplier")]
public class SupplierController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromServices] GetSuppliersService getSuppliersService,
                                         CancellationToken cancellationToken = default)
    {
        var suppliers = await getSuppliersService.ExecuteAsync(cancellationToken);

        return Ok(suppliers);
    }
}
