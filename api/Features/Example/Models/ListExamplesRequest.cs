using api.Features.Shared.Models;

namespace api.Features.Example.Models
{
    public class ListExamplesRequest : PagedRequest
    {
        public string SearchText { get; set; }
    }
}