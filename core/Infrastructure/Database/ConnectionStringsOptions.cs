namespace Core.Infrastructure.Database;

using System.ComponentModel.DataAnnotations;

public class ConnectionStringsOptions
{
	public const string Key = "ConnectionStrings";
	
	[Required]
	public required string Database { get; set; }
}