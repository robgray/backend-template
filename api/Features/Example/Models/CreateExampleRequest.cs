using System.ComponentModel.DataAnnotations;

namespace api.Features.Example.Models;
public class CreateExampleRequest
{
    [Required]
    public string Name { get; set; }
}