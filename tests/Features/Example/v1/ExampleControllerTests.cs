using Api;
using Api.Features.Example.v1.Models;

namespace Tests.Features.Example;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.Domain.Queries.Shared;
using Core.Infrastructure.Database;
using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Plumbing;
using Xunit;
using Xunit.Abstractions;

public class ExampleControllerTests(ITestOutputHelper testOutputHelper) 
    : ApiFactory<Startup>(testOutputHelper)
{
    [Fact]
    public async Task Create()
    {
        var exampleId = Guid.NewGuid();
        
        var createRequest = new CreateExampleRequest
        {
            Id = exampleId,
            Name = "Test Example",
        };

        var client = CreateUnauthenticatedClient();
        var responseModel = await client.Request("v1/examples")
            .AllowAnyHttpStatus()
            .PostJsonAsync(createRequest)
            .ReceiveJson<ExampleModel>();
        
        responseModel.Should()
            .BeEquivalentTo(new
            {
                ExampleId = exampleId,
                createRequest.Name,
            });

        var context = GetService<DataContext>();

        var dbExample = await context.Examples.SingleOrDefaultAsync(x => x.Id == exampleId);
        dbExample.Should().NotBeNull();
        dbExample.Id.Should().Be(exampleId);
        dbExample.Name.Should().Be("Test Example");
    }

    [Fact]
    public async Task Create_FailModelValidation()
    {
        var createRequest = new CreateExampleRequest();

        var client = CreateUnauthenticatedClient();
        Func<Task> action = async () => await client.Request("v1/examples")
            .PostJsonAsync(createRequest
            )
            .ReceiveJson<ExampleModel>();

        (await action.Should().ThrowAsync<FlurlHttpException>()).Which.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task Create_FailFluentValidation()
    {
        var createRequest = new CreateExampleRequest(); // Name is required

        var client = CreateUnauthenticatedClient();
        var response = await client.Request("v1/examples")
            .AllowAnyHttpStatus()
            .PostJsonAsync(createRequest);

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task Delete()
    {
        var context = GetService<DataContext>();
        var exampleId = Guid.NewGuid();
        
        var example = new Example { Id = exampleId, Name = "Old Name" };
        context.Examples.Add(example);
        await context.SaveChangesAsync();
        
        
        var client = CreateUnauthenticatedClient();
        var responseModel = await client.Request($"v1/examples/{exampleId}")
            .DeleteAsync();

        responseModel.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task Update()
    {
        var context = GetService<DataContext>();
        var exampleId = Guid.NewGuid();
        
        var example = new Example { Id = exampleId, Name = "Old Name" };
        context.Examples.Add(example);
        await context.SaveChangesAsync();
        
        var updateCommand = new UpdateExampleRequest
        {
            ExampleId = exampleId, 
            Name = "New Name",
        };

        var client = CreateUnauthenticatedClient();
        var responseModel = await client.Request($"v1/examples/{exampleId}")
            .AllowAnyHttpStatus()
            .PutJsonAsync(updateCommand)
            .ReceiveJson<ExampleModel>();

        responseModel.Should()
            .BeEquivalentTo(new
            {
                ExampleId = exampleId,
                Name = "New Name",
            });
    }

    [Fact]
    public async Task List()
    {
        var examples = new List<Example>()
        {
            new() { Id = Guid.NewGuid(), Name = "Test Example 1" },
            new() { Id = Guid.NewGuid(), Name = "A Test" },
            new() { Id = Guid.NewGuid(), Name = "Tester" },
            new() { Id = Guid.NewGuid(), Name = "Do not return" },
        };

        var context = GetService<DataContext>();
        await context.Examples.AddRangeAsync(examples);
        await context.SaveChangesAsync();
        
        var listRequest = new ListExamplesRequest
        { PageSize = 10, PageNumber = 1, SearchText = "Test" };
        
        var client = CreateUnauthenticatedClient(); 
        var responseModel = await client.Request("v1/examples")
                                .SetQueryParams(listRequest)
                                .GetJsonAsync<PagedResults<ExampleModel>>();
        
        responseModel.CurrentPage.Should().Be(1);
        responseModel.TotalItems.Should().Be(3);
        responseModel.TotalPages.Should().Be(1);
        responseModel.Items.Should().OnlyContain(x => x.Name.Contains("Test"));
    }

    [Fact]
    public async Task Get()
    {
        var context = GetService<DataContext>();
        
        var exampleId = Guid.NewGuid();
        var example = new Example { Id = exampleId, Name = "A Test Example" };

        context.Examples.Add(example);
        await context.SaveChangesAsync();
        
        var client = CreateUnauthenticatedClient();
        var responseModel = await client.Request($"v1/examples/{exampleId}")
            .AllowAnyHttpStatus()
            .GetJsonAsync<ExampleModel>();

        responseModel.Should().BeEquivalentTo(
            new 
            {
                ExampleId = exampleId,
                Name = "A Test Example"
            });
    }
    
    [Fact]
    public async Task Get_NonExistent_Returns404()
    {
        var context = GetService<DataContext>();
        
        var exampleId = Guid.NewGuid();
        var example = new Example { Id = exampleId, Name = "A Test Example" };
        
        context.Examples.Add(example);
        await context.SaveChangesAsync();

        var anotherExampleId = Guid.NewGuid();
        var client = CreateUnauthenticatedClient();
        var responseModel = await client.Request($"v1/examples/{anotherExampleId}")
            .AllowAnyHttpStatus()
            .GetAsync();

        responseModel.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        (await context.Examples.AnyAsync(x => x.Id == anotherExampleId)).Should().BeFalse();
    }
}