namespace Core.Domain.Queries.Example;

using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Models;

public static class GetExampleById
{
    public class Query : IQuery<Result<Example>>
    {
        public int Id { get; set; }
    }

    public class Handler : IQueryHandler<Query, Result<Example>>
    {
        public Task<Result<Example>> Handle(Query query, CancellationToken cancellationToken)
        {
            if (query.Id >= 10)
            {
                return Task.FromResult(Result<Example>.Success(new Example
                {
                    Id = query.Id,
                    Name = "Name",
                }));
            }
            
            return Task.FromResult(Result<Example>.NotFound());
        }
    }
}