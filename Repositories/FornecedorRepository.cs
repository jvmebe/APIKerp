using APIKerp.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

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

        public async Task<(bool Success, string ErrorMessage)> Update(int idEmpresa, int idFornecedor, Fornecedor fornecedor)
        {

            fornecedor.DataModificacao = DateTime.Now;

            try
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    await db.OpenAsync();

                    using (var transaction = db.BeginTransaction())
                    {
                        string query = @"
                            UPDATE FornCliente
                            SET 
                                RazaoSocial = @RazaoSocial,
                                NomeFantasia = @NomeFantasia,
                                InscricaoEstadual = @InscricaoEstadual,
                                CpfCnpj = @CpfCnpj,
                                Tipo = @Tipo,
                                IdCidade = @IdCidade,
                                IdRegiao = @IdRegiao,
                                Logradouro = @Logradouro,
                                Numero = @Numero,
                                Complemento = @Complemento,
                                Bairro = @Bairro,
                                Cep = @Cep,
                                ConsumidorRevenda = @ConsumidorRevenda,
                                Observacao = @Observacao,
                                Ativo = @Ativo,
                                FisicaJuridica = @FisicaJuridica,
                                DataModificacao = @DataModificacao,
                                LimiteCredito = @LimiteCredito
                            WHERE IdEmpresa = @IdEmpresa AND IdFornCliente = @IdFornCliente";

                        var parameters = new
                        {
                            IdEmpresa = idEmpresa,
                            IdFornCliente = idFornecedor,
                            RazaoSocial = fornecedor.RazaoSocial,
                            NomeFantasia = fornecedor.NomeFantasia,
                            InscricaoEstadual = fornecedor.InscricaoEstadual,
                            CpfCnpj = fornecedor.CpfCnpj,
                            Tipo = fornecedor.Tipo,
                            IdCidade = fornecedor.IdCidade,
                            IdRegiao = fornecedor.IdRegiao,
                            Logradouro = fornecedor.Logradouro,
                            Numero = fornecedor.Numero,
                            Complemento = fornecedor.Complemento,
                            Bairro = fornecedor.Bairro,
                            Cep = fornecedor.Cep,
                            ConsumidorRevenda = fornecedor.ConsumidorRevenda,
                            Observacao = fornecedor.Observacao,
                            Ativo = fornecedor.Ativo,
                            FisicaJuridica = fornecedor.FisicaJuridica,
                            DataModificacao = fornecedor.DataModificacao,
                            LimiteCredito = fornecedor.LimiteCredito
                        };

                        int affectedRows = await db.ExecuteAsync(query, parameters, transaction);

                        if (affectedRows == 0)
                        {
                            transaction.Rollback();
                            return (false, "Fornecedor ou Cliente não encontrado");
                        }

                        transaction.Commit();
                        return (true, null);
                    }
                }

            }
            //SQL Server normalmente retorna código 2628 ou 8152 quando os dados não passam validação por tamanho do campo
            catch (SqlException ex) when (ex.Number == 2628 || ex.Number == 8152)
            {
                return (false, "Um ou mais campos excedem o tamnho limite.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return (false, $"Erro");
            }
        }

        public async Task<(bool Success, string ErrorMessage)> Delete(int idEmpresa, int idFornecedor)
        {

            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string query = @"DELETE FROM FornCliente WHERE IdEmpresa = @IdEmpresa AND IdFornCliente = @IdFornCliente";

                    var affectedRows = await db.ExecuteAsync(query, new
                    {
                        IdEmpresa = idEmpresa,
                        IdFornCliente = idFornecedor,
                    });

                    if (affectedRows == 0)
                        return (false, "Fornecedor não encontrado");

                    return (true, null);
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return (false, "Não é possível excluir este fornecedor: Há dados vinculados");
            }
            catch (Exception ex)
              
            {
                return (false, "Ocorreu um erro.");
            }
        }


    }
}
