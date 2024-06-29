namespace Core.Domain.Commands.Example;

using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

public static class CreateExample
{
    public class Command : ICommand<Models.Example>
    {
        public string Name { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    public class Handler : ICommandHandler<Command, Models.Example>
    {
        public async Task<Models.Example> Handle(Command command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        
            var example = new Models.Example { Id = 1, Name = command.Name };
        
            // TODO: Actually save it somewhere

            return example;
        }
    }
}