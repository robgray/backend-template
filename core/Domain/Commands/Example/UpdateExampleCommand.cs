namespace Core.Domain.Commands.Example;

using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Models;

public static class UpdateExample
{
    public class Command : ICommand<Example>
    {
        public int ExampleId { get; set; }
        public string Name { get; set; }
    }

    public class Handler : ICommandHandler<Command, Example>
    { 
        public Task<Result<Example>> Handle(Command command, CancellationToken cancellationToken)
        {
            // TODO: Actually update it
        
            var example = new Example { Id = command.ExampleId, Name = command.Name };
        
            return Task.FromResult(Result<Example>.Success(example));
        }
    }
}
