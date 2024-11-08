using APIKerp.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace APIKerp.Repositories
{
    public class CidadeRepository
    {
        private readonly string _connectionString;

        public CidadeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Cidades>> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                //colocar campos para IdPais e Pais ao invés de agregar a classe em GET
                string query = @"SELECT c.IdCidade AS Id, c.Cidade, c.Ddd, c.Ativo, e.IdEstado, e.Estado
                 FROM Cidade c
                 JOIN Estado e ON c.IdEstado = e.IdEstado;";
                return await db.QueryAsync<Cidades>(query);
            }
        }

        public async Task<Cidades> GetOne(int idEmpresa, int idCidade)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT c.IdCidade AS Id, c.Cidade, c.Ddd, c.Ativo, c.DataCadastro, c.DataModificacao, e.IdEstado, e.Estado
                 FROM Cidade c
                 JOIN Estado e ON c.IdEstado = e.IdEstado
                WHERE c.IdEmpresa = @IdEmpresa AND c.IdCidade = @IdCidade";

                return await db.QueryFirstOrDefaultAsync<Cidades>(query, new { IdEmpresa = idEmpresa, IdCidade = idCidade });

            }
        }

        public async Task<object> Create(Cidades cidade)
        {

            cidade.DataCadastro = DateTime.Now;

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Cidade (IdEmpresa, Cidade, Ddd, DataCadastro, DataModificacao, DataCadastro, Ativo, IdEstado) VALUES (@IdEmpresa, @Cidade, @Ddd, @DataCadastro, @DataModificacao, @DataCadastro, @Ativo, @IdEstado)

                   SELECT CAST(SCOPE_IDENTITY() as int);";

                return await db.QuerySingleAsync(query, cidade);

            }
        }

    }
}
