using AutoMapper;
using Core.Domain.Commands.Example;
using Core.Domain.Queries.Example;

namespace Api.Features.Example.v1.Models;

using Core.Domain.Queries.Shared;

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
            .ForMember(r => r.ExampleId, o => o.MapFrom(c => c.ExampleId))
            .ForMember(r => r.Name, o => o.MapFrom(c => c.Name));
      
        CreateMap<int, DeleteExample.Command>()
            .ForMember(d => d.ExampleId, o => o.MapFrom(s => s));

        CreateMap<Core.Domain.Models.Example, ExampleModel>()
            .ForMember(d => d.ExampleId, o => o.MapFrom(s => s.Id));

        CreateMap<PagedResults<Core.Domain.Models.Example>, PagedResults<ExampleModel>>();
    }
}