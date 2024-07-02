namespace Core.Domain.Commands.Example;

using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Infrastructure.Mediator;
using Models;

public static class CreateExample
{
    public class Command : ICommand<Example>
    {
        public string? Name { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    public class Handler : ICommandHandler<Command, Example>
    {
        public Task<Result<Example>> Handle(Command command, CancellationToken cancellationToken)
        {
            var example = new Example { Id = 1, Name = command.Name };
        
            // TODO: Actually save it somewhere

            return Task.FromResult(Result<Example>.Success(example));
        }
    }
}