using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Api.Infrastructure.Options;

public class OptionsValidator : IOptionsValidator
{
	private const string ValuePropertyName = "Value";

	private static readonly List<(Type optionsType, Type genericType, PropertyInfo valueProperty)> Options = new();

	private readonly IServiceProvider _serviceProvider;

	public OptionsValidator(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public static void AddOptionsType<TOptions>()
	{
		var optionsType = typeof(TOptions);
		var genericType = typeof(IOptions<>).MakeGenericType(optionsType);
		var valueProperty = genericType.GetProperty(ValuePropertyName);
		if (valueProperty is not null)
		{
			Options.Add((optionsType, genericType, valueProperty));
		}
	}

	public IEnumerable<(Type optionsType, ValidateOptionsResult validateResult)> Validate()
	{
		foreach (var (optionsType, genericType, valueProperty) in Options)
		{
			var validateResult = ValidateOptionsResult.Success;

			try
			{
				valueProperty.GetValue(
					_serviceProvider.GetRequiredService(genericType));
			}
			catch (TargetInvocationException tie) when (tie.InnerException is OptionsValidationException ove)
			{
				validateResult = ValidateOptionsResult.Fail(ove.Failures);
			}

			yield return  (optionsType, validateResult);
		}
	}
}