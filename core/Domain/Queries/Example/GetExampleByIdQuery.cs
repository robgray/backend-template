namespace Core.Domain.Queries.Example;

using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Infrastructure.Database;
using Infrastructure.Mediator;
using Microsoft.EntityFrameworkCore;
using Models;

public static class GetExampleById
{
    public class Query : IQuery<Example>
    {
        public Guid Id { get; set; }
    }

    public class Handler(DataContext context) : IQueryHandler<Query, Example>
    {
        public async Task<Result<Example>> Handle(Query query, CancellationToken cancellationToken)
        {
            var example = await context.Examples.SingleOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

            if (example is null)
                return Result.NotFound();
            
            return Result<Example>.Success(example);
        }
    }
}