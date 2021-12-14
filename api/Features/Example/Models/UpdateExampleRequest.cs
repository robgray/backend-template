using System.ComponentModel.DataAnnotations;

namespace api.Features.Example.Models
{
    public class UpdateExampleRequest
    {
        [Required]
        public string Name { get; set; }
    }
}