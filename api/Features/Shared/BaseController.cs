namespace Api.Features.Shared;

using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class BaseController(ISender sender, IMapper mapper) : ControllerBase
{
    protected async Task<TMappedResult> ExecuteQuery<TCommand, TMappedResult>(params object[] models) where TCommand : new()
    {
        return await ExecuteMediatorRequest<TCommand, TMappedResult>(models);
    }

    protected async Task ExecuteCommand<TCommand>(params object[] models)
    {
        var command = MapperExtensions.Map<TCommand>(mapper, models);
        await sender.Send(command);
    }

    protected async Task<TMappedResult> ExecuteCommand<TCommand, TMappedResult>(params object[] models) where TCommand : new()
    {
        return await ExecuteMediatorRequest<TCommand, TMappedResult>(models);
    }

    private async Task<TMappedResult> ExecuteMediatorRequest<TRequest, TMappedResult>(params object[] models) where TRequest : new()
    {
        var command = models != null && models.Any() ? MapperExtensions.Map<TRequest>(mapper, models) : new TRequest();
        var result = await sender.Send(command);
        var mappedResult = MapperExtensions.Map<TMappedResult>(mapper, result);

        return mappedResult;
    }
}