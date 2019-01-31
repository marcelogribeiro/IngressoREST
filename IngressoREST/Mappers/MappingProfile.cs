using AutoMapper;
using IngressoREST.Models;

namespace IngressoREST.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NovoVestibularModel, VestibularModel>();
            CreateMap<NovoCandidatoModel, CandidatoModel>();
        }
    }
}
