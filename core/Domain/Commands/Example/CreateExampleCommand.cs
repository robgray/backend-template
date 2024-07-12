namespace Core.Domain.Commands.Example;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Infrastructure.Database;
using FluentValidation;
using Infrastructure.Mediator;
using Models;
using Serilog;

public static class CreateExample
{
    public class Command : ICommand<Example>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    public class Handler(DataContext context) : ICommandHandler<Command, Example>
    {
        public async Task<Result<Example>> Handle(Command command, CancellationToken cancellationToken)
        {
            if (context.Examples.Any(x => x.Id == command.Id))
            {
                return Result.Conflict();
            }
            
            var example = new Example { Id = command.Id, Name = command.Name };
            context.Examples.Add(example);
            
            await context.SaveChangesAsync(cancellationToken);

            return Result<Example>.Success(example);
        }
    }
}