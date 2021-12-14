using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using api.Features.Example.Models;
using api.Features.Shared;
using api.Plumbing.Auth;
using AutoMapper;
using core.Domain.Commands.Example;
using core.Domain.Queries.Example;
using core.Domain.Queries.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Features.Example
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamplesController : BaseController
    {
        public ExamplesController(IMediator mediator, IMapper mapper) : base(mediator, mapper) { }

        [HttpGet("{exampleId}")]
        [ProducesResponseType(typeof(ExampleModel), StatusCodes.Status200OK)]
        public Task<IActionResult> Get([FromRoute] int exampleId) =>
            ExecuteQuery<GetExampleByIdQuery, ExampleModel>(exampleId);

        [HttpGet]
        [ProducesResponseType(typeof(PagedResults<ExampleModel>), StatusCodes.Status200OK)]
        public Task<IActionResult> List([FromQuery] ListExamplesRequest request) =>
            ExecuteQuery<ListExamplesQuery, PagedResults<ExampleModel>>(request);

        [HttpPost]
        [ProducesResponseType(typeof(ExampleModel), StatusCodes.Status200OK)]
        public Task<IActionResult> Create([FromBody] CreateExampleRequest request) =>
            ExecuteCommand<CreateExampleCommand, ExampleModel>(request);

        [HttpPut("{exampleId?}")]
        [ProducesResponseType(typeof(ExampleModel), StatusCodes.Status200OK)]
        public Task<IActionResult> Update([Range(0, int.MaxValue)] int exampleId, UpdateExampleRequest request) =>
            ExecuteCommand<UpdateExampleCommand, ExampleModel>(exampleId, request);

        [HttpDelete("{exampleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<IActionResult> Delete(int exampleId) => ExecuteCommand<DeleteExampleCommand>(exampleId);
    }
}