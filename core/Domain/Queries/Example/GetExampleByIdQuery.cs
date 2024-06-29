namespace Core.Domain.Queries.Example;

using System.Threading;
using System.Threading.Tasks;
using Exceptions;

public static class GetExampleById
{
    public class Query : IQuery<Models.Example>
    {
        public int Id { get; set; }
    }

    public class Handler : IQueryHandler<Query, Models.Example>
    {
        public async Task<Models.Example> Handle(Query query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            // TODO: Actually get the item 
            var example = new Models.Example { Id = 1, Name = "Name" };

            if (example is null) throw new EntityNotFoundException($"Example with id {query.Id} not found");

            return example;
        }
    }
}