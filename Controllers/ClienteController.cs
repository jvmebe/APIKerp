using APIKerp.Models;
using APIKerp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APIKerp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : Controller
    {
        private readonly ClienteRepository _repository;

        public ClienteController(ClienteRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAll()
        {
            var result = await _repository.GetAllAsync();
            return Ok(result);
        }

        /*
        [HttpPost]
        public async Task<ActionResult<Cliente>> Create()
        {
            
        }
        */

        /*
        [HttpGet("{idEmpresa:int}/{idFornCliente:int}")]
        public async Task<ActionResult<Cliente>> GetById(int idEmpresa, int idFornCliente)
        {
            var result = await _repository.GetByIdAsync(idEmpresa, idFornCliente);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        */
    }
}
