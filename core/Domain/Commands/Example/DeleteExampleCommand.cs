namespace Core.Domain.Commands.Example;

using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class DeleteExampleCommand : ICommand
{
    public int ExampleId { get; set; }
}

public class DeleteExampleCommandHandler : ICommandHandler<DeleteExampleCommand>
{
    public async Task<Unit> Handle(DeleteExampleCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        // TODO: Actually delete it...
        
        return Unit.Value;
    }
}