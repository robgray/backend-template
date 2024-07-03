namespace Api.Infrastructure.HealthChecks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public static class DiagnosticHealthCheckResponseWriter
{
	public static Task WriteJsonObject(HttpContext httpContext, HealthReport healthReport)
	{
		var report = new ValdHealthReport
		{
			MachineName = Environment.MachineName,
			Results = healthReport.Entries.Select(
					e => new HealthEntry
					{
						Key = e.Key,
						IsOk = e.Value.Status == HealthStatus.Healthy,
						Message = e.Value.Description,
					})
				.ToList(),
		};

		return httpContext.Response.WriteAsJsonAsync(report, JsonSerializerDefaults.Value);
	}

	private sealed class ValdHealthReport
	{
		public string MachineName { get; set; } = string.Empty;

		public List<HealthEntry> Results { get; set; } = new();
	}

	private sealed class HealthEntry
	{
		public string Key { get; set; } = string.Empty;

		public bool IsOk { get; set; }

		public string? Message { get; set; }
	}
}