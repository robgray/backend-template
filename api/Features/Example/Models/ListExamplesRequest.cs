namespace Api.Features.Example.Models;

using Shared.Models;

public class ListExamplesRequest : PagedRequest
{
    public string SearchText { get; set; }
}