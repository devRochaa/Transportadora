using System.Text.Json.Serialization;

namespace Transportadora.API.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DeliveryStatus
{
    /// <summary>
    /// Aguardando atribuição de entregador
    /// </summary>
    Pending,

    /// <summary>
    /// Entregador atribuído à entrega
    /// </summary>
    Assigned,

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