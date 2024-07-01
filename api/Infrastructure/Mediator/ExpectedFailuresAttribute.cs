namespace Api.Infrastructure.Mediator;

using System;
using System.Collections.Generic;
using Core.Infrastructure.Mediator;

[AttributeUsage(AttributeTargets.Method)]
public class ExpectedFailuresAttribute : Attribute
{
	public ExpectedFailuresAttribute(params ResultStatus[] resultStatuses)
	{
		ResultStatuses = resultStatuses;
	}

	public IEnumerable<ResultStatus> ResultStatuses { get; }
}