using System.Threading;
using System.Threading.Tasks;
using core.Domain.Exceptions;

namespace core.Domain.Queries.Example;

public class GetExampleByIdQuery : IQuery<Models.Example>
{
    public int Id { get; set; }
}

public class GetExampleByIdQueryHandler : IQueryHandler<GetExampleByIdQuery, Models.Example>
{
    public async Task<Models.Example> Handle(GetExampleByIdQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        // TODO: Actually get the item 
        var example = new Models.Example { Id = 1, Name = "Name" };

        if (example == null) throw new EntityNotFoundException($"Example with id {request.Id} not found");

        return example;
    }
}