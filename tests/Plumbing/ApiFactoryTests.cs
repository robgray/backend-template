namespace Tests.Plumbing;

using System;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.Infrastructure.Database;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

public class ApiFactoryTests(ITestOutputHelper testOutputHelper) : ApiFactory(testOutputHelper)
{
	[Fact]
	public async Task DataContext_UsesInMemoryProviderAcrossScopes()
	{
		var exampleId = Guid.NewGuid();

		using (var scope = Server.Services.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<DataContext>();
			context.Database.ProviderName.Should().Be("Microsoft.EntityFrameworkCore.InMemory");

			context.Examples.Add(new Example { Id = exampleId, Name = "Factory persistence" });
			await context.SaveChangesAsync();
		}

		using (var scope = Server.Services.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<DataContext>();
			context.Database.ProviderName.Should().Be("Microsoft.EntityFrameworkCore.InMemory");

			var savedExample = await context.Examples.SingleAsync(example => example.Id == exampleId);
			savedExample.Name.Should().Be("Factory persistence");
		}
	}
}