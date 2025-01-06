using APIKerp.Models;
using APIKerp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APIKerp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListaController : Controller
    {
        private readonly ListaRepository _repository;

        public ListaController(ListaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Listas>>> GetAll()
        {
            var result = await _repository.GetAll();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] Listas lista)
        {
            var result = await _repository.Create(lista);
            return Ok(result);
        }

        [HttpPut("{idEmpresa:int}/{idLista:int}")]
        public async Task<IActionResult> Update(int idEmpresa, int idLista, [FromBody] Listas lista)
        {
            if (lista == null)
            {
                return BadRequest(new { Message = "Dados inválidos." });
            }

            var (success, errorMessage) = await _repository.Update(idEmpresa, idLista, lista);

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

        [HttpDelete("{idEmpresa:int}/{idLista:int}")]
        public async Task<IActionResult> Delete(int idEmpresa, int idLista)
        {
            var (success, errorMessage) = await _repository.Delete(idEmpresa, idLista);

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
