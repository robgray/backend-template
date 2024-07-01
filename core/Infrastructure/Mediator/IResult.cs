namespace Core.Infrastructure.Mediator;

using System;
using System.Collections.Generic;

public interface IResult
{
	ResultStatus Status { get; }
	IEnumerable<string> Errors { get; }
	IEnumerable<ValidationError> ValidationErrors { get; }
	Type ValueType { get; }
	object GetValue();
}