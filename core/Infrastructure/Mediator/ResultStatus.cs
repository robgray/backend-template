namespace Core.Infrastructure.Mediator;

public enum ResultStatus
{
	Ok,
	Created,
	Error,
	Forbidden,
	Unauthorized,
	Invalid,
	NotFound,
	NoContent,
	Conflict,
	CriticalError,
	Unavailable
}