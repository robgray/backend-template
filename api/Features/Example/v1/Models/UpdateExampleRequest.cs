using System.ComponentModel.DataAnnotations;

namespace Api.Features.Example.v1.Models;

using System;

public class UpdateExampleRequest
{
    [Required]
    public required Guid ExampleId { get; set; }
    
    [Required]
    public required string Name { get; set; }
}