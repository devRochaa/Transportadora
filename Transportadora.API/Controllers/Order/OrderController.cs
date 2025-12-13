using Microsoft.AspNetCore.Mvc;
using Transportadora.Application.Adress.Services;
using Transportadora.Application.Order.Requests;
using Transportadora.Infrasctructure.Providers.CepProvider;
using Transportadora.Infrasctructure.Providers.DistanceCalculate;
using Transportadora.Infrasctructure.Providers.Geoapify;

namespace Transportadora.API.Controllers.Order;

[ApiController]
[Route("order")]
public class OrderController : ControllerBase
{
    [HttpPost("frete")]
    public async Task<IActionResult> FretePost([FromServices] GetSupplierAddressByIdAsync getSupplierByIdAsync,
                                         [FromBody] CreateOrderRequest req,
                                         CancellationToken cancellationToken)
    {
        try
        {
            var supplierAddress = await getSupplierByIdAsync
                .ExecuteAsync(req.OriginId, cancellationToken)
                ?? throw new InvalidOperationException("Supplier not found.");

            GetCepModel originAddress = new GetCepModel
            {
                Cep = supplierAddress.ZipCode ?? string.Empty,
                Logradouro = supplierAddress.Street ?? string.Empty,
                Complemento = supplierAddress.Complement ?? string.Empty,
                Bairro = supplierAddress.Neighborhood ?? string.Empty,
                Cidade = supplierAddress.City ?? string.Empty,
                Uf = supplierAddress.State ?? string.Empty
            };

            GetCepModel? address = await CepClient.GetCepAsync(req.DestinyZipCode)
                ?? throw new InvalidOperationException("Address not found for the provided CEP.");

            GeoCordinatesModel? originCords = await GeoClient.GetCoordinatesAsync(originAddress, apiKey: "teste");
            GeoCordinatesModel? destinyCords = await GeoClient.GetCoordinatesAsync(address, apiKey: "teste");

            if (originCords is null || destinyCords is null)
                throw new InvalidOperationException("Coordinates not found.");

            //GeoDistanceModel? query = await GeoClient.CalculateDistanceAsync(originCords, destinyCords, apiKey: "teste");
            GeoDistanceModel distance = new GeoDistanceModel(){
                Distance = DistanceCalculate.Haversine(originCords.Latitude, originCords.Longitude, destinyCords.Latitude, destinyCords.Longitude),
                DistanceUnits = "KM"
            };

            return Ok(distance);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
