using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProAgil.Domain;
using ProAgil.Repository;
using System;
using System.Threading.Tasks;

namespace ProAgilServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly ILogger<EventosController> _logger;
        private readonly IProAgilRepository _repository;

        public EventosController(ILogger<EventosController> logger, IProAgilRepository repository)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _repository.GetEventosAsync(true);
                return Ok(eventos);
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
                return Ok(evento);
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
                return Ok(eventos); 
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou!");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Evento evento)
        {
            try
            {
                _repository.Add(evento);
                if (await _repository.SaveChangesAsync()) return Created($"/api/eventos/{evento.Id}", evento);    
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou!");
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Evento evento)
        {
            try
            {
                var eventoBD = await _repository.GetEventosAsyncById(id, false);
                if (eventoBD == null) return NotFound();

                _repository.Update(evento);
                if (await _repository.SaveChangesAsync())
                    return Created($"/api/eventos/{evento.Id}", evento);
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
