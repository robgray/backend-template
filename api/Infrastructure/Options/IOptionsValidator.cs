using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Api.Infrastructure.Options;

public interface IOptionsValidator
{
	IEnumerable<(Type optionsType, ValidateOptionsResult validateResult)> Validate();
}