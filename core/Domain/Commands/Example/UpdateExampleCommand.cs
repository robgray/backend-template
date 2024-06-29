namespace Core.Domain.Commands.Example;

using System.Threading;
using System.Threading.Tasks;

public static class UpdateExample
{
    public class Command : ICommand<Models.Example>
    {
        public int ExampleId { get; set; }
        public string Name { get; set; }
    }

    public class Handler : ICommandHandler<Command, Models.Example>
    {
        public async Task<Models.Example> Handle(Command command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        
            // TODO: Actually update it
        
            var example = new Models.Example { Id = 1, Name = command.Name };
        
            return example;
        }
    }
}
