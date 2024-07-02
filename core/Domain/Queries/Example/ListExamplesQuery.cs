namespace Core.Domain.Queries.Example;

using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Mediator;
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

    public class Handler : IQueryHandler<Query, Result<PagedResults<Example>>>
    {
        public Task<Result<PagedResults<Example>>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            
            var pagedResults = new PagedResults<Example>(new[] { new Example { Id = 1, Name = "Name" } },
                request.PageNumber, 1, 1);

            var result = Result<PagedResults<Example>>.Success(pagedResults);
            return Task.FromResult(result);
        }
    }
}


