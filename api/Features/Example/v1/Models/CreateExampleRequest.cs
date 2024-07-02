using System.ComponentModel.DataAnnotations;

namespace Api.Features.Example.v1.Models;

using System;

public class CreateExampleRequest
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    public string? Name { get; set; }
}