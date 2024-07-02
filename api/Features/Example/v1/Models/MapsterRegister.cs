namespace Api.Features.Example.v1.Models;

using System;
using Core.Domain.Commands.Example;
using Core.Domain.Models;
using Core.Domain.Queries.Example;
using Mapster;

public class MapsterRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Guid, GetExampleById.Query>()
            .Map(dest => dest.Id, i => i);

        config.NewConfig<Guid, DeleteExample.Command>()
            .Map(dest => dest.ExampleId, i => i);

        config.NewConfig<ListExamplesRequest, ListExamples.Query>();

        config.NewConfig<UpdateExampleRequest, UpdateExample.Command>();

        config.NewConfig<Guid, UpdateExample.Command>()
            .Map(dest => dest.ExampleId, i => i);

        config.NewConfig<Example, ExampleModel>()
            .Map(dest => dest.ExampleId, src => src.Id);
    }
}
