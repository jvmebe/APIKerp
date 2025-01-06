using APIKerp.Models;
using APIKerp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APIKerp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegiaoController : Controller
    {
        private readonly RegiaoRepository _repository;

        public RegiaoController(RegiaoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Regioes>>> GetAll()
        {
            var result = await _repository.GetAll();
            return Ok(result);
        }

        [HttpGet("{idEmpresa:int}/{idRegiao:int}")]
        public async Task<ActionResult<Fornecedor>> GetOne(int idEmpresa, int idRegiao)
        {
            var result = await _repository.GetOne(idEmpresa, idRegiao);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] Regioes regiao)
        {
            var result = await _repository.Create(regiao);
            return Ok(result);
        }

        [HttpPut("{idEmpresa:int}/{idRegiao:int}")]
        public async Task<IActionResult> Update(int idEmpresa, int idRegiao, [FromBody] Regioes regiao)
        {
            if (regiao == null)
            {
                return BadRequest(new { Message = "Dados inválidos." });
            }

            var (success, errorMessage) = await _repository.Update(idEmpresa, idRegiao, regiao);

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

        [HttpDelete("{idEmpresa:int}/{idRegiao:int}")]
        public async Task<IActionResult> Delete(int idEmpresa, int idRegiao)
        {
            var (success, errorMessage) = await _repository.Delete(idEmpresa, idRegiao);

            if (!success)
            {
                if (errorMessage.Contains("encontrado"))
                    return NotFound();

                if (errorMessage.Contains("vinculado"))
                    return Conflict(errorMessage);

                return StatusCode(500, new { Message = errorMessage });

            }

            return NoContent();
        }


    }
}
