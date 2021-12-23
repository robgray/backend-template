using AutoMapper;
using core.Domain.Commands.Example;
using core.Domain.Queries.Example;
using core.Domain.Queries.Shared;

namespace api.Features.Example.Models;
public class ExampleMappingProfile : Profile
{
    public ExampleMappingProfile()
    {
        CreateMap<ListExamplesRequest, ListExamplesQuery>();

        CreateMap<int, GetExampleByIdQuery>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s));

        CreateMap<CreateExampleRequest, CreateExampleCommand>();

        CreateMap<int, UpdateExampleCommand>()
            .ForMember(d => d.ExampleId, o => o.MapFrom(s => s))
            .ForAllOtherMembers(d => d.Ignore());
        CreateMap<UpdateExampleRequest, UpdateExampleCommand>()
            .ForMember(d => d.ExampleId, o => o.Ignore());

        CreateMap<int, DeleteExampleCommand>()
            .ForMember(d => d.ExampleId, o => o.MapFrom(s => s));

        CreateMap<core.Domain.Models.Example, ExampleModel>()
            .ForMember(d => d.ExampleId, o => o.MapFrom(s => s.Id));

        CreateMap<PagedResults<core.Domain.Models.Example>, PagedResults<ExampleModel>>();
    }
}