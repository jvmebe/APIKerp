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


    }
}
