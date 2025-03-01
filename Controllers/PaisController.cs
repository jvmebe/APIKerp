﻿using APIKerp.Models;
using APIKerp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APIKerp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaisController : Controller
    {
        private readonly PaisRepository _repository;

        public PaisController(PaisRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paises>>> GetAll()
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
        public async Task<ActionResult<int>> Create([FromBody] Paises pais)
        {
            var result = await _repository.Create(pais);
            return Ok(result);
        }

        [HttpPut("{idEmpresa:int}/{idPais:int}")]
        public async Task<IActionResult> Update(int idEmpresa, int idPais, [FromBody] Paises pais)
        {
            if (pais == null)
            {
                return BadRequest(new { Message = "Dados inválidos." });
            }

            var (success, errorMessage) = await _repository.Update(idEmpresa, idPais, pais);

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

        [HttpDelete("{idEmpresa:int}/{idPais:int}")]
        public async Task<IActionResult> Delete(int idEmpresa, int idPais)
        {
            var (success, errorMessage) = await _repository.Delete(idEmpresa, idPais);

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
