using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Transportadora.Application.Clients.Requests;
using Transportadora.Application.Clients.Services;
using Transportadora.Infrasctructure.Providers.CepProvider;
using Transportadora.Infrasctructure.Providers.Geoapify;

namespace Transportadora.API.Controllers.Client;

[ApiController]
[Route("client")]
public class ClientController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromServices] GetClientService getClientService,
                                         CancellationToken cancellationToken){
        try
        {
            var query = await getClientService.ExecuteAsync(cancellationToken);

            return Ok(query);
        }
        catch(Exception ex) {
            return BadRequest(ex);
        }
    }


    [HttpGet("{clientId}")]
    public async Task<IActionResult> GetById([FromServices] GetClientByIdService getClientByIdService,
                                             Guid clientId,
                                             CancellationToken cancellationToken){
        try
        {
            var query = await getClientByIdService.ExecuteAsync(clientId, cancellationToken);

            return Ok(query);
        }
        catch(Exception ex) {
            return BadRequest(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromServices] CreateClientService createClientService,
                                          [FromBody] CreateClientRequest request,
                                          CancellationToken cancellationToken){
        try
        {
            var clientId = await createClientService.ExecuteAsync(request, cancellationToken);

            return Created(string.Empty, new { id = clientId });
        }
        catch(Exception ex) {
            return BadRequest(ex);
        }
    }

    [HttpPut("{clientId}")]
    public async Task<IActionResult> Put([FromServices] UpdateClientService updateClientService,
                                         [FromBody] UpdateClientRequest request,
                                         [FromRoute] Guid clientId,
                                         CancellationToken cancellationToken){
        try
        {
            await updateClientService.ExecuteAsync(clientId, request, cancellationToken);

            return NoContent();
        }
        catch(Exception ex) {
            return BadRequest(ex);
        }
    }
    
}
