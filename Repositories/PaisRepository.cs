using APIKerp.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace APIKerp.Repositories
{
    public class PaisRepository
    {
        private readonly string _connectionString;

        public PaisRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Paises>> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM Pais";
                return await db.QueryAsync<Paises>(query);
            }
        }

        public async Task<object> Create(Paises pais)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Pais (IdEmpresa, Pais, Sigla, Ddi, DataCadastro, DataModificacao, Nacional) VALUES (@IdEmpresa, @Pais, @Sigla, @Ddi, @DataCadastro, @DataModificacao, @Nacional)

                   SELECT CAST(SCOPE_IDENTITY() as int);";

                return await db.QuerySingleAsync(query, pais);

            }
        }
    }
}
