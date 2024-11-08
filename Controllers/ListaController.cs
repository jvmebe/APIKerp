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

    }
}
