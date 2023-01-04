namespace Api.Features.Example.Models;

using System.ComponentModel.DataAnnotations;

public class UpdateExampleRequest
{
    [Required]
    public string Name { get; set; }
}