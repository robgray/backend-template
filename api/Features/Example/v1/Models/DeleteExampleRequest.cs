using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Example.v1.Models;

using System;

public class DeleteExampleRequest
{
    [FromRoute]
    [Required]
    public Guid ExampleId { get; set; }
}