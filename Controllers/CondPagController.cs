using APIKerp.Models;
using APIKerp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APIKerp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CondPagController : Controller
    {
        private readonly CondPagRepository _repository;

        public CondPagController(CondPagRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CondPags>>> GetAll()
        {
            var result = await _repository.GetAll();
            return Ok(result);
        }

        [HttpGet("{idEmpresa:int}/{idCondPag:int}")]
        public async Task<ActionResult<Fornecedor>> GetOne(int idEmpresa, int idCondPag)
        {
            var result = await _repository.GetOne(idEmpresa, idCondPag);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CondPags condPag)
        {
            var result = await _repository.Create(condPag);
            return Ok(result);
        }


        [HttpPut("{idEmpresa:int}/{idCondPag:int}")]
        public async Task<IActionResult> Update(int idEmpresa, int idCondPag, [FromBody] CondPags condPag)
        {
            if (condPag == null)
            {
                return BadRequest(new { Message = "Dados inválidos." });
            }

            var (success, errorMessage) = await _repository.Update(idEmpresa, idCondPag, condPag);

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


        [HttpDelete("{idEmpresa:int}/{idCondPag:int}")]
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
