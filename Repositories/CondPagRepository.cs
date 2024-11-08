using APIKerp.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace APIKerp.Repositories
{
    public class CondPagRepository
    {
        private readonly string _connectionString;

        public CondPagRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<CondPags>> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM CondicaoPagamento";
                return await db.QueryAsync<CondPags>(query);
            }
        }

        public async Task<object> Create(CondPags condPag)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO CondicaoPagamento (IdEmpresa, CondicaoPagamento, TaxaJuros, NumeroParcelas, DataCadastro, DataModificacao, Tipo, Dia, Operacao, Ativo, PorParcela) VALUES (@IdEmpresa, @CondicaoPagamento, @TaxaJuros, @NumeroParcelas, @DataCadastro, @DataModificacao, @Tipo, @Dia, @Operacao, @Ativo, @PorParcela)

                   SELECT CAST(SCOPE_IDENTITY() as int);";

                return await db.QuerySingleAsync(query, condPag);

            }
        }

    }
}
