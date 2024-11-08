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

    }
}
