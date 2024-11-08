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

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CondPags condPag)
        {
            var result = await _repository.Create(condPag);
            return Ok(result);
        }

    }
}
