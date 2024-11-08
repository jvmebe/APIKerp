using APIKerp.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace APIKerp.Repositories
{
    public class ListaRepository
    {

        private readonly string _connectionString;

        public ListaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Listas>> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT IdLista as Id, IdEmpresa, Lista, DataModificacao, DescMax, MargemLucro, PerComissao FROM Lista";
                return await db.QueryAsync<Listas>(query);
            }
        }

        public async Task<object> Create(Listas lista)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Lista (IdEmpresa, Lista, DataModificacao, DescMax, MargemLucro, PerComissao) VALUES (@IdEmpresa, @Lista, @DataModificacao, @DescMax, @MargemLucro, @PerComissao)

                   SELECT CAST(SCOPE_IDENTITY() as int);";

                return await db.QuerySingleAsync(query, lista);

            }
        }

    }
}
