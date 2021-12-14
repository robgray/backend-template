using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace api.Features.Example.Models
{
    public class DeleteExampleRequest
    {
        [FromRoute]
        [Required]
        public int? ExampleId { get; set; }
    }
}