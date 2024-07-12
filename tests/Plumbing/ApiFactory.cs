namespace Tests.Plumbing;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Api;
using Core.Infrastructure.Database;
using Flurl.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Xunit.Abstractions;

/// <summary>
///     API integration test class.  Your test class should inherit this class, provide the desired
///     TStartup and set expectations and returns on mocked classes.
/// </summary>
public class ApiFactory(ITestOutputHelper testOutputHelper) : WebApplicationFactory<Startup>
{
	private bool disposed;
	private Logger logger;

	protected override void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				logger?.Dispose();
				logger = null;
			}

			disposed = true;
		}

		base.Dispose(disposing);
	}

	protected FlurlClient CreateUnauthenticatedClient()
	{
		var client = this.Server.CreateClient();

		return new FlurlClient(client);
	}

	protected FlurlClient CreateAuthenticatedClient()
	{
		var client = this.Server.CreateClient();

		var token = GenerateToken();

		client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

		return new FlurlClient(client);

		string GenerateToken()
		{
			var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Constants.TestSecurityKey));

			const string myIssuer = "https://openid.example.io";
			const string myAudience = "api.example";

			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(JwtRegisteredClaimNames.Sub, "TEST USER"),
					new Claim(JwtRegisteredClaimNames.Email, "test.user@example.com"),
					// Add other claims as needed.
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				Issuer = myIssuer,
				Audience = myAudience,
				SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature),
			};

			var internalToken = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(internalToken);
		}
	}

	protected T GetService<T>()
	{
		var scope = this.Server.Services.CreateScope();
		return scope.ServiceProvider.GetRequiredService<T>();
	}

	protected override IHost CreateHost(IHostBuilder builder)
	{
		var newBuilder = Host.CreateDefaultBuilder()
			.ConfigureWebHostDefaults(
				webBuilder =>
				{
					webBuilder.UseEnvironment(
						"Development"); // Create a new environment to use its appSettings if you don't want to run against local/dev
					// Settings as Development is helpful if you have other code dependent on that environment.
					webBuilder.UseContentRoot(".");
					webBuilder.UseTestServer();
					webBuilder.ConfigureServices(services =>
					{
						services.TryAddEnumerable(ServiceDescriptor
							.Singleton<IPostConfigureOptions<JwtBearerOptions>, JwtBearerPostConfigureOptions>());
					});
					webBuilder.UseStartup<Startup>();
					webBuilder.ConfigureTestServices(services =>
					{
						ConfigureDataContext(services);
						ConfigureTestServices(services);
					});
				})
			.ConfigureLogging(ConfigureLogging)
			.UseSerilog((_, services, configuration) =>
				configuration
					.ReadFrom.Services(services)
					.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning));

		return base.CreateHost(newBuilder);
	}

	protected virtual void ConfigureTestServices(IServiceCollection services)
	{
		// Override this in subclasses to provide any necessary services
		// This can be helpful if not all your existing tests have migrated to suit the TStartup provided.
		// You can add mocks for the retiring services.
	}

	private void ConfigureDataContext(IServiceCollection services)
	{
		// Remove real DataContext
		var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DataContext>));
		services.Remove(descriptor);

		var contextId = Guid.NewGuid().ToString();

		services.AddDbContext<DataContext>(
			options =>
			{
				options.UseInMemoryDatabase(contextId);
				options.ConfigureWarnings(wcb => wcb.Ignore(InMemoryEventId.TransactionIgnoredWarning));
			});

		var sp = services.BuildServiceProvider();

		using var scope = sp.CreateScope();
		var scopedServices = scope.ServiceProvider;
		var db = scopedServices.GetRequiredService<DataContext>();

		db.Database.EnsureCreated();
	}

	private void ConfigureLogging(ILoggingBuilder logging)
	{
		logger = TestSerilogLogger.CreateTestLogger(testOutputHelper);

		Log.Logger = logger;

		logging.ClearProviders();
		logging.AddSerilog(logger);
	}
}