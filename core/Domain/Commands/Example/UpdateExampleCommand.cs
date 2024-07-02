namespace Core.Domain.Commands.Example;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Infrastructure.Database;
using FluentValidation;
using Infrastructure.Mediator;
using Microsoft.EntityFrameworkCore;
using Models;

public static class UpdateExample
{
    public class Command : ICommand<Example>
    {
        public Guid ExampleId { get; set; }
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
            var example = await context.Examples.SingleOrDefaultAsync(x => x.Id == command.ExampleId, cancellationToken);
            if (example is null)
            {
                return Result.NotFound();
            }

            example.Name = command.Name;
            
            return Result<Example>.Success(example);
        }
    }
}
