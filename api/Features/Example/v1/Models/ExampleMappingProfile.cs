using AutoMapper;
using Core.Domain.Commands.Example;
using Core.Domain.Queries.Example;
using Core.Domain.Queries.Shared;

namespace Api.Features.Example.v1.Models;

public class ExampleMappingProfile : Profile
{
    public ExampleMappingProfile()
    {
        CreateMap<ListExamplesRequest, ListExamples.Query>();

        CreateMap<int, GetExampleById.Query>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s));

        CreateMap<CreateExampleRequest, CreateExample.Command>();

        CreateMap<int, UpdateExample.Command>()
            .ForMember(d => d.ExampleId, o => o.MapFrom(s => s));
        
        CreateMap<UpdateExampleRequest, UpdateExample.Command>()
            .ForMember(d => d.ExampleId, o => o.Ignore());

        CreateMap<int, DeleteExample.Command>()
            .ForMember(d => d.ExampleId, o => o.MapFrom(s => s));

        CreateMap<Core.Domain.Models.Example, ExampleModel>()
            .ForMember(d => d.ExampleId, o => o.MapFrom(s => s.Id));

        CreateMap<PagedResults<Core.Domain.Models.Example>, PagedResults<ExampleModel>>();
    }
}