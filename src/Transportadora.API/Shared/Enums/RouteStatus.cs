namespace Transportadora.API.Shared.Enums;

public enum RouteStatus
{
    /// <summary>
    /// Rota planejada, mas ainda não iniciada.
    /// </summary>
    Planned,

    /// <summary>
    /// Rota em andamento.
    /// </summary>
    InProgress,

    /// <summary>
    /// Rota concluída com sucesso.
    /// </summary>
    Finished,

    /// <summary>
    /// Rota cancelada.
    /// </summary>
    Cancelled,
}