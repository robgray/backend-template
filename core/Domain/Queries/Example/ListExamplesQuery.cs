namespace Core.Domain.Queries.Example;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Database;
using Infrastructure.Mediator;
using Microsoft.EntityFrameworkCore;
using Models;
using Shared;

public static class ListExamples
{
    public class Query : IQuery<PagedResults<Example>>
    {
        public string? SearchText { get; set; }
        
        public int PageNumber { get; set; }
        
        public int PageSize { get; set; }
    }

    public class Handler(DataContext context) : IQueryHandler<Query, PagedResults<Example>>
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


