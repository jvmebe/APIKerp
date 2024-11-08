using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using APIKerp.Models;

namespace APIKerp.Repositories
{
    public class ClienteRepository
    {
        private readonly string _connectionString;

        public ClienteRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM FORNCLIENTE_VIEW";
                return await db.QueryAsync<Cliente>(query);
            }
        }

        /*
        public async Task<Cliente> Create(Cliente cliente)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Clientes";
            }
        }
        */
        /*
        public async Task<FornClienteViewModel> GetByIdAsync(int idEmpresa, int idFornCliente)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"
                SELECT * 
                FROM vw_FornCliente 
                WHERE IdEmpresa = @IdEmpresa AND IdFornCliente = @IdFornCliente";

                return await db.QueryFirstOrDefaultAsync<FornClienteViewModel>(
                    query, new { IdEmpresa = idEmpresa, IdFornCliente = idFornCliente });
            }
        }
        */
    }
}
