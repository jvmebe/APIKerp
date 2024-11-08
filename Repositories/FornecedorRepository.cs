using APIKerp.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace APIKerp.Repositories
{
    public class FornecedorRepository
    {
        private readonly string _connectionString;

        public FornecedorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Fornecedor>> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT IdFornCliente AS Id, IdEmpresa, RazaoSocial, NomeFantasia, InscricaoEstadual, CpfCnpj, Tipo, IdCidade, IdRegiao, Logradouro, Numero, Complemento, Bairro, Cep, ConsumidorRevenda, Observacao, Ativo, FisicaJuridica, DataCadastro, DataModificacao, IdCidadeEmp, LimiteCredito FROM FornCliente";
                return await db.QueryAsync<Fornecedor>(query);
            }
        }

        public async Task<Fornecedor> GetOne(int idEmpresa, int idFornecedor)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT *
                FROM FornCliente
                WHERE IdEmpresa = @IdEmpresa AND IdFornCliente = @IdFornCliente";

                return await db.QueryFirstOrDefaultAsync<Fornecedor>(query, new { IdEmpresa = idEmpresa, IdFornCliente = idFornecedor });

            }
        }

        public async Task<object> Create(Fornecedor fornecedor)
        {

            fornecedor.DataCadastro = DateTime.Now;

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO FornCliente (IdEmpresa, RazaoSocial, NomeFantasia, InscricaoEstadual, CpfCnpj, Tipo, IdCidade, IdRegiao, Logradouro, Numero, Complemento, Bairro, Cep, ConsumidorRevenda, Observacao, Ativo, FisicaJuridica, DataCadastro, DataModificacao, IdCidadeEmp, LimiteCredito) VALUES (@IdEmpresa, @RazaoSocial, @NomeFantasia, @InscricaoEstadual, @CpfCnpj, @Tipo, @IdCidade, @IdRegiao, @Logradouro, @Numero, @Complemento, @Bairro, @Cep, @ConsumidorRevenda, @Observacao, @Ativo, @FisicaJuridica, @DataCadastro, @DataModificacao, @IdEmpresa, @LimiteCredito) " +
                    "SELECT CAST(SCOPE_IDENTITY() as int);";

                return await db.QuerySingleAsync(query, fornecedor);

            }
        }

    }
}
