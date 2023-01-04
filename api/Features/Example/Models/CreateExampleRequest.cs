namespace Api.Features.Example.Models;

using System.ComponentModel.DataAnnotations;

public class CreateExampleRequest
{
    [Required]
    public string Name { get; set; }
}