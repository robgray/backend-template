namespace Core.Infrastructure.Mediator;

public class ValidationError
{
	public ValidationError()
	{
	}

	public ValidationError(string errorMessage)
	{
		ErrorMessage = errorMessage;
	}

	public ValidationError(string identifier, string errorMessage, string errorCode, ValidationSeverity severity)
	{
		Identifier = identifier;
		ErrorMessage = errorMessage;
		ErrorCode = errorCode;
		Severity = severity;
	}

	public required string Identifier { get; set; }
	
	public required string ErrorMessage { get; set; }
	
	public required string ErrorCode { get; set; }
	public ValidationSeverity Severity { get; set; } = ValidationSeverity.Error;
}