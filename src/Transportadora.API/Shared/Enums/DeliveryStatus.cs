using System.Text.Json.Serialization;

namespace Transportadora.API.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DeliveryStatus
{
    /// <summary>
    /// Aguardando atribuição de entregador e veículo
    /// </summary>
    Pending,

    /// <summary>
    /// Entregador e veículo atribuídos
    /// </summary>
    ReadyToGo,

    /// <summary>
    /// Entrega em trânsito
    /// </summary>
    InTransit,

    /// <summary>
    /// Entrega concluída com sucesso
    /// </summary>
    Delivered,

    /// <summary>
    /// Entrega falhou
    /// </summary>
    Failed,
}