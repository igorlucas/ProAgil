using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProAgil.Api.Dtos;
using ProAgil.Domain;
using ProAgil.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProAgilServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly ILogger<EventosController> _logger;
        private readonly IProAgilRepository _repository;

        private readonly IMapper _mapper;

        public EventosController(ILogger<EventosController> logger, IProAgilRepository repository, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _repository.GetEventosAsync(true);
                var result = _mapper.Map<EventoDto[]>(eventos);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou!");
            }
        }

        [HttpGet("{id}")]   
        public async Task<IActionResult> Get(int id)
        {
            try
            {   
                var evento = await _repository.GetEventosAsyncById(id, true);
                var result = _mapper.Map<EventoDto>(evento);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou!");
            }
        }

        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try
            {
                var eventos = await _repository.GetEventosAsyncByTema(tema, true);
                var result = _mapper.Map<IEnumerable<EventoDto>>(eventos);
                return Ok(result); 
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou!");
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSalve = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if(file.Length > 0){
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSalve, fileName.Replace("\"", " ").Trim());

                    using(var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);        
                    }
                }
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou!");
            }
            return BadRequest("Erro ao tentar realizar o upload!");
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto eventoDto)  
        {
            try
            {
                var evento = _mapper.Map<Evento>(eventoDto);
                _repository.Add(evento);
                if (await _repository.SaveChangesAsync()) return Created($"/api/eventos/{evento.Id}", _mapper.Map<EventoDto>(evento));    
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou!");
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto eventoDto)
        {
            try
            {

                var evento = await _repository.GetEventosAsyncById(id, false);
                if (evento == null) return NotFound();

                var idLotes = new List<int>();
                var idRedesSociais = new List<int>();
                
                eventoDto.Lotes.ForEach(lote=> idLotes.Add(lote.Id));
                eventoDto.RedesSociais.ForEach(redeSocial=> idRedesSociais.Add(redeSocial.Id));
                
                var lotes = evento.Lotes.Where(lote=> !idLotes.Contains(lote.Id)).ToArray();
                var redesSociais = evento.RedesSociais.Where(redeSocial=> !idRedesSociais.Contains(redeSocial.Id)).ToArray();                
                
                if(lotes.Length > 0) _repository.DeleteRange(lotes);
                if(redesSociais.Length > 0) _repository.DeleteRange(redesSociais);
                

                _mapper.Map(eventoDto, evento);
                _repository.Update(evento);
                if (await _repository.SaveChangesAsync())
                    return Created($"/api/eventos/{evento.Id}", _mapper.Map<EventoDto>(evento));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou!");
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]    
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var evento = await _repository.GetEventosAsyncById(id, false);
                if (evento == null) return NotFound();

                _repository.Delete(evento);
                if (await _repository.SaveChangesAsync()) return Ok();
            }
            catch (Exception ex)
            {
                var err = ex;
                return StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou!");
            }
            return BadRequest();
        }
    }
}
