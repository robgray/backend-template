using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Example.v1.Models;

public class DeleteExampleRequest
{
    [FromRoute]
    [Required]
    public int ExampleId { get; set; }
}