namespace Core.Domain.Commands.Example;

using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Mediator;

public static class DeleteExample
{
    public class Command : IAsyncCommand
    {
        public int ExampleId { get; set; }
    }
    
    public class Handler : IAsyncCommandHandler<Command>
    {
        public Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            // TODO: Actually delete it...
            
            return Task.FromResult(Result.NoContent());
        }
    }
}

