using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProAgil.Api.Dtos;
using ProAgil.Domain;
using ProAgil.Repository;
using System;
using System.Collections.Generic;
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
