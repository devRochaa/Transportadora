namespace Transportadora.API.Shared.Enums;

public enum DeliveryEventType
{
    /// <summary>
    /// Iniciado
    /// </summary>
    Started,

    /// <summary>
    /// Rota transferida para outro veículo/condutor
    /// </summary>
    RouteTransferred,

    /// <summary>
    /// Atrasado
    /// </summary>
    Delayed,

    /// <summary>
    /// Parcialmente entregue
    /// </summary>
    PartialDelivered,

    /// <summary>
    /// Entregue
    /// </summary>
    Delivered,

    /// <summary>
    /// Falhou
    /// </summary>
    Failed,
}