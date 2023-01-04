namespace Core.Domain.Queries.Example;

using System.Threading;
using System.Threading.Tasks;
using Models;
using Shared;

public class ListExamplesQuery : IQuery<PagedResults<Example>>
{
    public string SearchText { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class ListExamplesQueryHandler : IQueryHandler<ListExamplesQuery, PagedResults<Models.Example>>
{
    public async Task<PagedResults<Models.Example>> Handle(ListExamplesQuery request,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return new PagedResults<Models.Example>(new[] { new Models.Example { Id = 1, Name = "Name" } },
            request.PageNumber, 1, 1);
    }
}
