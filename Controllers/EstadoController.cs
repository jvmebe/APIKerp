using APIKerp.Models;
using APIKerp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIKerp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EstadoController : Controller
    {
        private readonly EstadoRepository _repository;

        public EstadoController(EstadoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estados>>> GetAll()
        {
            var result = await _repository.GetAll();
            return Ok(result);
        }

        [HttpGet("{idEmpresa:int}/{idEstado:int}")]
        public async Task<ActionResult<Estados>> GetOne(int idEmpresa, int idEstado)
        {
            var result = await _repository.GetOne(idEmpresa, idEstado);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] Estados estado)
        {
            var result = await _repository.Create(estado);
            return Ok(result);
        }

        [HttpPut("{idEmpresa:int}/{idEstado:int}")]
        public async Task<IActionResult> Update(int idEmpresa, int idLista, [FromBody] Estados estado)
        {
            if (estado == null)
            {
                return BadRequest(new { Message = "Dados inválidos." });
            }

            var (success, errorMessage) = await _repository.Update(idEmpresa, idLista, estado);

            if (!success)
            {
                if (errorMessage.Contains("campos"))
                    return BadRequest(new { Message = errorMessage });

                if (errorMessage == "encontrado.")
                    return NotFound(new { Message = errorMessage });

                return StatusCode(500, new { Message = errorMessage });
            }

            return Ok(new { Message = "Item atualizado." });
        }


        [HttpDelete("{idEmpresa:int}/{idEstado:int}")]
        public async Task<IActionResult> Delete(int idEmpresa, int idEstado)
        {
            var (success, errorMessage) = await _repository.Delete(idEmpresa, idEstado);

            if (!success)
            {
                if (errorMessage.Contains("encontrado"))
                    return NotFound();

                if (errorMessage.Contains("vinculada"))
                    return Conflict(errorMessage);

                return StatusCode(500, new { Message = errorMessage });

            }

            return NoContent();
        }
    }
}
