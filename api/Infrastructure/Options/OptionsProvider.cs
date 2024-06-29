using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Api.Infrastructure.Options;

public class OptionsProvider : IOptionsProvider
{
	public static readonly IOptionsProvider Empty =
		new OptionsProvider(new ServiceCollection().BuildServiceProvider());

	private readonly IServiceProvider _serviceProvider;

	public OptionsProvider(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public TOptions GetOptions<TOptions>() where TOptions : class =>
		_serviceProvider
			.GetRequiredService<IOptions<TOptions>>()
			.Value;
}