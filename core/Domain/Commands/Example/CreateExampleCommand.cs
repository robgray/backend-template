using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

namespace core.Domain.Commands.Example;
public class CreateExampleCommand : ICommand<Models.Example>
{
    public string Name { get; set; }
}

public class CreateCommandValidator : AbstractValidator<CreateExampleCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(x => x.Name).Must(name => name != "Bad name");
    }
}

public class CreateExampleCommandHandler : ICommandHandler<CreateExampleCommand, Models.Example>
{
    public async Task<Models.Example> Handle(CreateExampleCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        
        var example = new Models.Example { Id = 1, Name = command.Name };
        
        // TODO: Actually save it somewhere

        return example;
    }
}