namespace Api.Infrastructure;

using System.Text.Json;
using System.Text.Json.Serialization;

public static class JsonSerializerDefaults
{
	public static JsonSerializerOptions Value
	{
		get
		{
			var options = new JsonSerializerOptions();
			ApplyDefaults(options);

			return options;
		}
	}

	public static void ApplyDefaults(JsonSerializerOptions options)
	{
		options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
		options.Converters.Add(new JsonStringEnumConverter());
		options.PropertyNameCaseInsensitive = true;
	}
}