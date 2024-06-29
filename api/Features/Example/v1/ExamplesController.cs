using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Api.Features.Example.v1.Models;
using Api.Features.Shared;
using AutoMapper;
using Core.Domain.Commands.Example;
using Core.Domain.Queries.Example;
using Core.Domain.Queries.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Example.v1;

[ApiController]
[Route("v1/[controller]")]
public class ExamplesController(ISender sender, IMapper mapper) : BaseController(sender, mapper)
{
    [HttpGet("{exampleId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ExampleModel>> Get([FromRoute] int exampleId) =>
        Ok(await ExecuteQuery<GetExampleById.Query, ExampleModel>(exampleId));

    [HttpGet]
    [ProducesResponseType(typeof(PagedResults<ExampleModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResults<ExampleModel>>> List([FromQuery] ListExamplesRequest request) =>
        Ok(await ExecuteQuery<ListExamples.Query, PagedResults<ExampleModel>>(request));

    [HttpPost]
    [ProducesResponseType(typeof(ExampleModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<ExampleModel>> Create([FromBody] CreateExampleRequest request) =>
        Ok(await ExecuteCommand<CreateExample.Command, ExampleModel>(request));

    [HttpPut("{exampleId?}")]
    [ProducesResponseType(typeof(ExampleModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<ExampleModel>> Update([Range(0, int.MaxValue)] int exampleId, UpdateExampleRequest request) =>
        Ok(await ExecuteCommand<UpdateExample.Command, ExampleModel>(exampleId, request));

    [HttpDelete("{exampleId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(int exampleId)
    {
        await ExecuteCommand<DeleteExample.Command>(exampleId);
        return NoContent();
    }
}