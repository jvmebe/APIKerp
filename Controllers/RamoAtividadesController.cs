using APIKerp.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using APIKerp.Repositories;

namespace APIKerp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RamoAtividadesController : Controller
    {
        private readonly RamoAtividadesRepository _repository;

        public RamoAtividadesController(RamoAtividadesRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RamoAtividades>>> GetAll()
        {
            var result = await _repository.GetAll();
            return Ok(result);
        }


        [HttpGet("{idEmpresa:int}/{idPais:int}")]
        public async Task<ActionResult<Paises>> GetOne(int idEmpresa, int idPais)
        {
            var result = await _repository.GetOne(idEmpresa, idPais);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] RamoAtividades ramo)
        {
            var result = await _repository.Create(ramo);
            return Ok(result);
        }


        [HttpPut("{idEmpresa:int}/{idRamo:int}")]
        public async Task<IActionResult> Update(int idEmpresa, int idRamo, [FromBody] RamoAtividades ramo)
        {
            if (ramo == null)
            {
                return BadRequest(new { Message = "Dados inválidos." });
            }

            var (success, errorMessage) = await _repository.Update(idEmpresa, idRamo, ramo);

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


        [HttpDelete("{idEmpresa:int}/{idRamo:int}")]
        public async Task<IActionResult> Delete(int idEmpresa, int idRamo)
        {
            var (success, errorMessage) = await _repository.Delete(idEmpresa, idRamo);

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
