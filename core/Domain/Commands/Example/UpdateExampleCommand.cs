using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core.Domain.Exceptions;

namespace core.Domain.Commands.Example;

public class UpdateExampleCommand : ICommand<Models.Example>
{
    public int ExampleId { get; set; }
    public string Name { get; set; }
}

public class UpdateExampleCommandHandler : ICommandHandler<UpdateExampleCommand, Models.Example>
{
    public async Task<Models.Example> Handle(UpdateExampleCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        
        // TODO: Actually update it
        
        var example = new Models.Example { Id = 1, Name = command.Name };
        
        return example;
    }
}