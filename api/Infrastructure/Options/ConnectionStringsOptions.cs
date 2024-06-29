using System.ComponentModel.DataAnnotations;

namespace Api.Infrastructure.Options;

public class ConnectionStringsOptions
{
	public const string Key = "ConnectionStrings";
	
	[Required]
	public string Database { get; set; }
}