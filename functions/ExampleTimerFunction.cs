namespace Functions;

using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Serilog;

public class ExampleTimerFunction
{
    private readonly ILogger _log;

    public ExampleTimerFunction()
    {
        _log = Log.ForContext<ExampleTimerFunction>();
    }

    [Function(nameof(ExampleTimerFunction))]
    public async Task RunAsync([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
    {
        await Task.CompletedTask;
        
        _log.Information("Finished sample timer function");
    }
}