namespace Api.Features.Example.v1.Models;

using System;
using AutoMapper;
using Core.Domain.Commands.Example;
using Core.Domain.Models;
using Core.Domain.Queries.Example;
using Core.Domain.Queries.Shared;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ListExamplesRequest, ListExamples.Query>();

        CreateMap<Guid, GetExampleById.Query>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s));

        CreateMap<CreateExampleRequest, CreateExample.Command>();

        CreateMap<Guid, UpdateExample.Command>()
            .ForMember(d => d.ExampleId, o => o.MapFrom(s => s));

        CreateMap<UpdateExampleRequest, UpdateExample.Command>()
            .ForMember(r => r.ExampleId, o => o.MapFrom(c => c.ExampleId))
            .ForMember(r => r.Name, o => o.MapFrom(c => c.Name));

        CreateMap<Guid, DeleteExample.Command>()
            .ForMember(d => d.ExampleId, o => o.MapFrom(s => s));

        CreateMap<Example, ExampleModel>()
            .ForMember(d => d.ExampleId, o => o.MapFrom(s => s.Id));

        CreateMap<PagedResults<Example>, PagedResults<ExampleModel>>();
    }
}