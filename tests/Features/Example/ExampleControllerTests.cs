namespace Tests.Features.Example;

using System;
using System.Threading.Tasks;
using Api.Features.Example.Models;
using Core.Domain.Models;
using Core.Domain.Queries.Shared;
using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Plumbing;
using Xunit;
using Xunit.Abstractions;

public class ExampleControllerTests : ApiFactory<TestStartup>
{
    public ExampleControllerTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public async Task Create()
    {
        var createRequest = new CreateExampleRequest
        {
            Name = "Test"
        };

        var client = CreateUnauthenticatedClient();
        var responseModel = await client.Request("api/examples")
            .PostJsonAsync(createRequest)
            .ReceiveJson<ExampleModel>();

        responseModel.Should()
            .BeEquivalentTo(new
            {
                ExampleId = 1,
                createRequest.Name
            });
    }

    [Fact]
    public async Task Create_FailModelValidation()
    {
        var createRequest = new CreateExampleRequest();

        var client = CreateUnauthenticatedClient();
        Func<Task> action = async () => await client.Request("api/examples")
            .PostJsonAsync(createRequest
            )
            .ReceiveJson<ExampleModel>();

        (await action.Should().ThrowAsync<FlurlHttpException>()).Which.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task Create_FailFluentValidation()
    {
        var createRequest = new CreateExampleRequest
        {
            Name = "Bad name"
        };

        var client = CreateUnauthenticatedClient();
        Func<Task> action = async () => await client.Request("api/examples")
            .PostJsonAsync(createRequest)
            .ReceiveJson<ExampleModel>();

        (await action.Should().ThrowAsync<FlurlHttpException>())
            .Which.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task Delete()
    {
        var client = CreateUnauthenticatedClient();
        var responseModel = await client.Request("api/examples/1")
            .DeleteAsync();

        responseModel.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task Update()
    {
        var example = new Example { Name = "Old Name" };

        var updateCommand = new UpdateExampleRequest
        {
            Name = "Test"
        };

        var client = CreateUnauthenticatedClient();
        var responseModel = await client.Request($"api/examples/{example.Id}")
            .PutJsonAsync(updateCommand)
            .ReceiveJson<ExampleModel>();

        responseModel.Should()
            .BeEquivalentTo(new
            {
                ExampleId = 1,
                updateCommand.Name
            });
    }

    [Fact]
    public async Task List()
    {
        var listRequest = new ListExamplesRequest
        { PageSize = 10, PageNumber = 1, SearchText = "Na" };

        var client = CreateUnauthenticatedClient(); 
        var responseModel = await client.Request("api/examples")
                                .SetQueryParams(listRequest)
                                .GetJsonAsync<PagedResults<ExampleModel>>();

        responseModel.Items.Should().ContainEquivalentOf(
            new
            {
                ExampleId = 1,
            });
        responseModel.CurrentPage.Should().Be(1);
        responseModel.TotalItems.Should().Be(1);
        responseModel.TotalPages.Should().Be(1);
    }

    [Fact]
    public async Task Get()
    {
        var client = CreateUnauthenticatedClient();
        var responseModel = await client.Request($"api/examples/1")
            .GetJsonAsync<ExampleModel>();

        responseModel.Should().BeEquivalentTo(
            new 
            {
                ExampleId = 1
            });
    }
}