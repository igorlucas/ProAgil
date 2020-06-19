using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.Api.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} � obrigat�rio!")]
        public string Local { get; set; }

        [Required(ErrorMessage = "O campo {0} � obrigat�rio!")]
        public string DataEvento { get; set; }

        [Required(ErrorMessage = "O campo {0} � obrigat�rio!")]
        public string Tema { get; set; }

        [Required(ErrorMessage = "O campo {0} � obrigat�rio!")]
        [Range(2, 1200)]
        public int QtdPessoas { get; set; }
        
        public List<LoteDto> Lotes { get; set; }

        [Required(ErrorMessage = "O campo {0} � obrigat�rio!")]
        public string ImagemUrl { get; set; }
        
        [Required(ErrorMessage = "O campo {0} � obrigat�rio!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} � obrigat�rio!")]
        public string Telefone { get; set; }
        
        public List<RedeSocialDto> RedesSociais { get; set; }

        public List<PalestranteDto> Palestrantes { get; set; }
    }
}