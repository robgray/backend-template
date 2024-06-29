namespace Core.Domain.Commands.Example;

using System.Threading;
using System.Threading.Tasks;
using MediatR;

public static class DeleteExample
{
    public class Command : ICommand
    {
        public int ExampleId { get; set; }
    }

    public class Handler : ICommandHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            // TODO: Actually delete it...
        }
    }
}

