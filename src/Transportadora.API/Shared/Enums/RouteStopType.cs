using System.Text.Json.Serialization;

namespace Transportadora.API.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RouteStopType
{
    /// <summary>
    /// Ponto de origem da rota.
    /// </summary>
    Origin,

    /// <summary>
    /// Ponto de destino da rota.
    /// </summary>
    Delivery,

    /// <summary>
    /// Ponto de parada para descanso ou refeições.
    /// </summary>
    Break,

    /// <summary>
    /// Ponto de reabastecimento de combustível.
    /// </summary>
    Fuel,

    /// <summary>
    /// Ponto de manutenção ou verificação do veículo.
    /// </summary>
    Unexpected,
}