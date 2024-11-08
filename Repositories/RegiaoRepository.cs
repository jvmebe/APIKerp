using APIKerp.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace APIKerp.Repositories
{
    public class RegiaoRepository
    {
        private readonly string _connectionString;

        public RegiaoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Regioes>> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM Pais";
                return await db.QueryAsync<Regioes>(query);
            }
        }

        public async Task<object> Create(Regioes regiao)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Regiao (IdEmpresa, Regiao, Descricao, DataCadastro, DataModificacao, IdUsuario, Ativo) VALUES (@IdEmpresa, @Regiao, @Descricao, @DataCadastro, @DataModificacao, @IdUsuario, @Ativo)

                   SELECT CAST(SCOPE_IDENTITY() as int);";

                return await db.QuerySingleAsync(query, regiao);

            }
        }
    }
}
