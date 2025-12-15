namespace Transportadora.API.Tasks.BackgroundServices;

public abstract class PeriodicBackgroundService(TimeSpan interval) : BackgroundService
{
    private readonly SemaphoreSlim _Mutex = new(1, 1);

    protected abstract Task IterateAsync(CancellationToken cancelationToken = default);

    /// <summary>
    /// Nome do serviço (classe derivada) para usar em logs, métricas, etc.
    /// </summary>
    protected string ServiceName => GetType().Name;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken = default)
    {
        // Usa PeriodicTimer para evitar drift no delay
        using var timer = new PeriodicTimer(interval);
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            // Garante que só uma iteração roda por vez
            if (!await _Mutex.WaitAsync(0, stoppingToken))
                continue;

            try
            {
                await IterateAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                // TODO: REGISTRAR LOG DE ERRO
            }
            finally
            {
                _Mutex.Release();
            }
        }
    }
}
