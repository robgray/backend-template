namespace Api.Features.Shared;

using System;
using System.Threading.Tasks;
using AutoMapper;
using Core.Extensions;
using Core.Infrastructure.Mediator;
using Infrastructure.Mediator;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class BaseController(ISender sender, IMapper mapper) : ControllerBase
{
    protected async Task<ActionResult<TMappedResult>> ExecuteQuery<TQuery, TData, TMappedResult>(params object[] models) 
        where TQuery : IQuery<Result<TData>>, new()
        where TData : new()
    {
        return await ExecuteMediatorRequest<TQuery, TData, TMappedResult>(models);
    }
    
    protected async Task<ActionResult> ExecuteAsyncCommand<TCommand>(params object[] models)
        where TCommand: IAsyncCommand, new()
    {
        var command = MapperExtensions.Map<TCommand>(mapper, models);
        var result = (Result)await sender.Send(command);
        return result.ToActionResult(this);
    }
    
    protected async Task<ActionResult<TMappedResult>> ExecuteCommand<TCommand, TData, TMappedResult>(params object[] models) 
        where TCommand : ICommand<TData>, new()
        where TData : new()
    {
        return await ExecuteMediatorRequest<TCommand, TData, TMappedResult>(models);
    }

    private async Task<ActionResult<TMappedResult>> ExecuteMediatorRequest<TRequest, TData, TMappedResult>(params object[] models) 
        where TRequest : IRequest<Result<TData>>, new()
        where TData : new()
    {
        var command = models is not null && models.Length != 0 ? MapperExtensions.Map<TRequest>(mapper, models) : new TRequest();
        var response = await sender.Send(command);
        
        if (response is not Result<TData> result) throw new InvalidOperationException("Return type must be of type Result");
        
        return result.ToActionResult<TMappedResult, TData>(mapper, this);
    }
}