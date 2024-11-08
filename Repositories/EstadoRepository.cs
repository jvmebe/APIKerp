using APIKerp.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace APIKerp.Repositories
{
    public class EstadoRepository
    {
        private readonly string _connectionString;

        public EstadoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Estados>> GetAllAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT e.IdEstado AS Id, e.IdEmpresa, e.Estado, e.Sigla, e.PercIcms, e.IcmsInt, e.DataCadastro, e.DataModificacao, e.PerRedSt, e.CodigoWeb, p.IdPais as IdPais, p.Pais as NomePais
                 FROM Estado e
                 JOIN Pais p ON e.IdPais = p.IdPais;";
                return await db.QueryAsync<Estados>(query);
            }
        }


        public async Task<object> Create(Estados estado)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Estado (IdEmpresa, Estado, Sigla, PercIcms, IcmsInt, DataCadastro, PerRedSt, CodigoWeb, IdPais) VALUES (@IdEmpresa, @Estado, @Sigla, @PercIcms, @IcmsInt, @DataCadastro, @PerRedSt, @CodigoWeb, @IdPais)

                   SELECT CAST(SCOPE_IDENTITY() as int);";

                return await db.QuerySingleAsync(query, estado);

            }
        }

    }
}
