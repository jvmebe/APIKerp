using APIKerp.Models;
using APIKerp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APIKerp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CidadeController : Controller
    {
        private readonly CidadeRepository _repository;

        public CidadeController(CidadeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cidades>>> GetAll()
        {
            var result = await _repository.GetAll();
            return Ok(result);
        }

        [HttpGet("{idEmpresa:int}/{idCidade:int}")]
        public async Task<ActionResult<Fornecedor>> GetOne(int idEmpresa, int idCidade)
        {
            var result = await _repository.GetOne(idEmpresa, idCidade);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] Cidades cidade)
        {
            var result = await _repository.Create(cidade);
            return Ok(result);
        }

        [HttpPut("{idEmpresa:int}/{idCidade:int}")]
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



        [HttpDelete("{idEmpresa:int}/{idCidade:int}")]
        public async Task<IActionResult> Delete(int idEmpresa, int idCidade)
        {
            var (success, errorMessage) = await _repository.Delete(idEmpresa, idCidade);

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
