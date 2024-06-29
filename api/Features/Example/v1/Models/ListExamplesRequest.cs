using Api.Features.Shared.Models;

namespace Api.Features.Example.v1.Models;

public class ListExamplesRequest : PagedRequest
{
    public string SearchText { get; set; }
}