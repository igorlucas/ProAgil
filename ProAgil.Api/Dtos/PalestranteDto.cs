using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ProAgil.Api.Dtos
{
    public class PalestranteDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }   
        public string ImagemURL { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public List<RedeSocialDto> RedesSociais { get; set; }
        
        [JsonIgnore]
        public List<EventoDto> Eventos { get; set; }
    }
}