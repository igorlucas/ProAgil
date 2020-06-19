using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.Api.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        public string Local { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        public string DataEvento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        public string Tema { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [Range(2, 1200)]
        public int QtdPessoas { get; set; }
        
        public List<LoteDto> Lotes { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        public string ImagemUrl { get; set; }
        
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        public string Telefone { get; set; }
        
        public List<RedeSocialDto> RedesSociais { get; set; }

        public List<PalestranteDto> Palestrantes { get; set; }
    }
}