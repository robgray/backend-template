namespace Api.Features.Example.Models;

using AutoMapper;
using Core.Domain.Commands.Example;
using Core.Domain.Models;
using Core.Domain.Queries.Example;
using Core.Domain.Queries.Shared;

public class ExampleMappingProfile : Profile
{
    public ExampleMappingProfile()
    {
        CreateMap<ListExamplesRequest, ListExamplesQuery>();

        CreateMap<int, GetExampleByIdQuery>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s));

        CreateMap<CreateExampleRequest, CreateExampleCommand>();

        CreateMap<int, UpdateExampleCommand>()
            .ForMember(d => d.ExampleId, o => o.MapFrom(s => s));
        
        CreateMap<UpdateExampleRequest, UpdateExampleCommand>()
            .ForMember(d => d.ExampleId, o => o.Ignore());

        CreateMap<int, DeleteExampleCommand>()
            .ForMember(d => d.ExampleId, o => o.MapFrom(s => s));

        CreateMap<Example, ExampleModel>()
            .ForMember(d => d.ExampleId, o => o.MapFrom(s => s.Id));

        CreateMap<PagedResults<Example>, PagedResults<ExampleModel>>();
    }
}