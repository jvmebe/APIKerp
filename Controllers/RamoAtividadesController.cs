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
        public async Task<ActionResult<IEnumerable<Paises>>> GetAll()
        {
            var result = await _repository.GetAll();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] RamoAtividades ramo)
        {
            var result = await _repository.Create(ramo);
            return Ok(result);
        }

    }
}
