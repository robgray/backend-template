using System.ComponentModel.DataAnnotations;

namespace Api.Features.Example.v1.Models;

public class UpdateExampleRequest
{
    [Required]
    public required int ExampleId { get; set; }
    
    [Required]
    public required string Name { get; set; }
}