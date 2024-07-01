namespace Core.Infrastructure.Mediator;

using System.Text.Json.Serialization;

public class PagedResult<T> : Result<T>
{
	public PagedResult(PagedInfo pagedInfo, T value) : base(value)
	{
		PagedInfo = pagedInfo;
	}

	[JsonInclude] 
	public PagedInfo PagedInfo { get; init; }
}