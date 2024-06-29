namespace Api.Infrastructure.Options;

public interface IOptionsProvider
{
	TOptions GetOptions<TOptions>() where TOptions : class;
}