using APIKerp.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace APIKerp.Repositories
{
    public class RamoAtividadesRepository
    {
        private readonly string _connectionString;

        public RamoAtividadesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Paises>> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM RamoAtividades";
                return await db.QueryAsync<Paises>(query);
            }
        }

        public async Task<object> Create(RamoAtividades ramo)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO RamoAtividades (IdEmpresa, NomeRamo, DataCadastro, DataModificacao, Ativo) VALUES (@IdEmpresa, @NomeRamo, @DataCadastro, @DataModificacao, @Ativo)

                   SELECT CAST(SCOPE_IDENTITY() as int);";

                return await db.QuerySingleAsync(query, ramo);

            }
        }

    }
}
