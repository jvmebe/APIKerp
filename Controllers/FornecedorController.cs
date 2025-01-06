using APIKerp.Models;
using APIKerp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APIKerp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FornecedorController : Controller
    {
        private readonly FornecedorRepository _repository;

        public FornecedorController(FornecedorRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fornecedor>>> GetAll()
        {
            var result = await _repository.GetAll();
            return Ok(result);
        }

        [HttpGet("{idEmpresa:int}/{idFornecedor:int}")]
        public async Task<ActionResult<Fornecedor>> GetOne(int idEmpresa, int idFornecedor)
        {
            var result = await _repository.GetOne(idEmpresa, idFornecedor);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Fornecedor>> Create([FromBody] Fornecedor fornecedor)
        {
            var result = await _repository.Create(fornecedor);
            return Ok(result);
        }

        [HttpPut("{idEmpresa:int}/{idLista:int}")]
        public async Task<IActionResult> Update(int idEmpresa, int idLista, [FromBody] Fornecedor fornecedor)
        {
            if (fornecedor == null)
            {
                return BadRequest(new { Message = "Dados inválidos." });
            }

            var (success, errorMessage) = await _repository.Update(idEmpresa, idLista, fornecedor);

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
        public async Task<IActionResult> Delete(int idEmpresa, int idFornecedor)
        {
            var (success, errorMessage) = await _repository.Delete(idEmpresa, idFornecedor);

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
