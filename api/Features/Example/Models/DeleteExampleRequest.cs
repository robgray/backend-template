namespace Api.Features.Example.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

public class DeleteExampleRequest
{
    [FromRoute]
    [Required]
    public int? ExampleId { get; set; }
}