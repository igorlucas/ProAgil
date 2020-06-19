using System.Linq;
using AutoMapper;
using ProAgil.Api.Dtos;
using ProAgil.Domain;

namespace ProAgil.Api.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Evento, EventoDto>()
            .ForMember(eventoDto => eventoDto.Palestrantes, opt =>
            {
              opt.MapFrom(evento => evento.PalestrantesEventos.Select(pe => pe.Palestrante).ToList());
            }).ReverseMap();

            CreateMap<Palestrante, PalestranteDto>()
            .ForMember(palestranteDto => palestranteDto.Eventos, opt =>
             {
               opt.MapFrom(palestrante => palestrante.PalestrantesEventos.Select(pe => pe.Evento).ToList());
             }).ReverseMap();

            CreateMap<Lote, LoteDto>().ReverseMap();

            CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();
        }
    }
}