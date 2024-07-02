namespace Api.Features.Example.v1;

using MapsterMapper;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Api.Features.Example.v1.Models;
using Api.Features.Shared;
using Core.Domain.Commands.Example;
using Core.Domain.Models;
using Core.Domain.Queries.Example;
using Core.Domain.Queries.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("v1/[controller]")]
public class ExamplesController(ISender sender, IMapper mapper) : BaseController(sender, mapper)
{
    [HttpGet("{exampleId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ExampleModel>> Get([FromRoute] int exampleId) =>
        await ExecuteQuery<GetExampleById.Query, Example, ExampleModel>(exampleId);

    [HttpGet]
    [ProducesResponseType(typeof(PagedResults<ExampleModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResults<ExampleModel>>> List([FromQuery] ListExamplesRequest request) =>
        await ExecuteQuery<ListExamples.Query, PagedResults<Example>, PagedResults<ExampleModel>>(request);

    [HttpPost]
    [ProducesResponseType(typeof(ExampleModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<ExampleModel>> Create([FromBody] CreateExampleRequest request) =>
        await ExecuteCommand<CreateExample.Command, Example, ExampleModel>(request);

    [HttpPut("{exampleId?}")]
    [ProducesResponseType(typeof(ExampleModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<ExampleModel>> Update([Range(0, int.MaxValue)] int exampleId, UpdateExampleRequest request) =>
        await ExecuteCommand<UpdateExample.Command, Example, ExampleModel>(exampleId, request);

    [HttpDelete("{exampleId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Delete(int exampleId) =>
        await ExecuteAsyncCommand<DeleteExample.Command>(exampleId);
}