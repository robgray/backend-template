namespace Core.Domain.Commands.Example;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Infrastructure.Database;
using Infrastructure.Mediator;
using Microsoft.EntityFrameworkCore;

public static class DeleteExample
{
    public class Command : IAsyncCommand
    {
        public Guid ExampleId { get; set; }
    }
    
    public class Handler(DataContext context) : IAsyncCommandHandler<Command>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var example = await context.Examples.SingleOrDefaultAsync(x => x.Id == command.ExampleId, cancellationToken);
            if (example is not null)
            {
                context.Examples.Remove(example);
                await context.SaveChangesAsync(cancellationToken);
            }
                
            return Result.NoContent();
        }
    }
}

