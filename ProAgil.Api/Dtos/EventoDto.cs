using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.Api.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} � obrigat�rio!")]
        [StringLength(100, MinimumLength=3, ErrorMessage="O campo {0} deve estar entre 3 a 100 caracteres")]
        public string Local { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        [Required(ErrorMessage = "O campo {0} � obrigat�rio!")]
        public DateTime DataEvento { get; set; }

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