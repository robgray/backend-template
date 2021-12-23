using System;
using System.Net.Http;
using System.Threading.Tasks;
using api.Features.Example.Models;
using core.Domain.Queries.Shared;
using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using tests.Plumbing;
using Xunit;
using Xunit.Abstractions;

namespace tests.Features.Example;

public class ExampleControllerTests : ApiTest
{
    public ExampleControllerTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

    [Fact]
    public async Task Create()
    {
        var createRequest = new CreateExampleRequest
        {
            Name = "Test"
        };

        var client = CreateClient();
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

        var client = CreateClient();
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

        var client = CreateClient();
        Func<Task> action = async () => await client.Request("api/examples")
            .PostJsonAsync(createRequest
            )
            .ReceiveJson<ExampleModel>();

        (await action.Should().ThrowAsync<FlurlHttpException>())
            .Which.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task Delete()
    {
        var client = CreateClient();
        var responseModel = await client.Request("api/examples/1").SendJsonAsync(HttpMethod.Delete, new DeleteExampleRequest());

        responseModel.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task Update()
    {
        var example = new core.Domain.Models.Example { Name = "Old Name" };

        var updateCommand = new UpdateExampleRequest
        {
            Name = "Test"
        };

        var client = CreateClient();
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

        var client = CreateClient();
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
        var client = CreateClient();
        var responseModel = await client.Request($"api/examples/1")
            .GetJsonAsync<ExampleModel>();

        responseModel.Should().BeEquivalentTo(
            new
            {
                ExampleId = 1
            });
    }
}