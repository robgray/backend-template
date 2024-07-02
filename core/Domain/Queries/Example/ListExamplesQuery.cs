namespace Core.Domain.Queries.Example;

using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Core.Infrastructure.Database;
using Infrastructure.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Models;
using Shared;

public static class ListExamples
{
    public class Query : IQuery<Result<PagedResults<Example>>>
    {
        public string? SearchText { get; set; }
        
        public int PageNumber { get; set; }
        
        public int PageSize { get; set; }
    }

    public class Handler(DataContext context) : IQueryHandler<Query, Result<PagedResults<Example>>>
    {
        public async Task<Result<PagedResults<Example>>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.SearchText))
                return Result.NotFound();

            var query = context.Examples
                .Where(x => x.Name != null && x.Name.Contains(request.SearchText));

            var totalItems = await query.CountAsync(cancellationToken);
            
            var examples = await query
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);
            
            var pagedResults = new PagedResults<Example>(examples, request.PageNumber, 1, totalItems);

            return Result<PagedResults<Example>>.Success(pagedResults);
        }
    }
}


