using System.ComponentModel.DataAnnotations;

namespace Api.Features.Example.v1.Models;

public class CreateExampleRequest
{
    [Required]
    public string? Name { get; set; }
}