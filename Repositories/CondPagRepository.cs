using APIKerp.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

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


        public async Task<CondPags> GetOne(int idEmpresa, int idCondPag)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT *
                FROM CondicaoPagamento
                WHERE IdEmpresa = @IdEmpresa AND IdCondicaoPagamento = @IdCondicaoPagamento";

                return await db.QueryFirstOrDefaultAsync<CondPags>(query, new { IdEmpresa = idEmpresa, IdCondicaoPagamento = idCondPag });

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

        public async Task<(bool Success, string ErrorMessage)> Update(int idEmpresa, int idCondPag, CondPags condPag)
        {

            condPag.DataModificacao = DateTime.Now;

            try
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    await db.OpenAsync();

                    using (var transaction = db.BeginTransaction()) {
                        string query = @"
                        UPDATE CondicaoPagamento 
                        SET 
                            CondicaoPagamento = @CondicaoPagamento,
                            TaxaJuros = @TaxaJuros,
                            NumeroParcelas = @NumeroParcelas,
                            DataModificacao = @DataModificacao,
                            Tipo = @Tipo,
                            Dia = @Dia,
                            Operacao = @Operacao,
                            Ativo = @Ativo,
                            PorParcela = @PorParcela
                        WHERE IdEmpresa = @IdEmpresa AND IdCondicaoPagamento = @IdCondicaoPagamento";

                        var parameters = new
                        {
                            IdEmpresa = idEmpresa,
                            IdCondicaoPagamento = idCondPag,
                            CondicaoPagamento = condPag.CondicaoPagamento,
                            TaxaJuros = condPag.TaxaJuros,
                            NumeroParcelas = condPag.NumeroParcelas,
                            DataModificacao = condPag.DataModificacao,
                            Tipo = condPag.Tipo,
                            Dia = condPag.Dia,
                            Operacao = condPag.Operacao,
                            Ativo = condPag.Ativo,
                            PorParcela = condPag.PorParcela
                        };

                        int affectedRows = await db.ExecuteAsync(query, parameters, transaction);

                        if (affectedRows == 0)
                        {
                            transaction.Rollback();
                            return (false, "Item não encontrado");
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


        public async Task<(bool Success, string ErrorMessage)> Delete(int idEmpresa, int idCondPag)
        {

            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string query = @"DELETE FROM Estado WHERE IdEmpresa = @IdEmpresa AND IdEstado = @IdEstado";

                    var affectedRows = await db.ExecuteAsync(query, new
                    {
                        IdEmpresa = idEmpresa,
                        IdPais = idCondPag
                    });

                    return (affectedRows > 0, null);
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return (false, "Não é possível excluir este estado: Há uma cidade vinculada.");
            }
            catch (Exception ex)
            {
                return (false, "Ocorreu um erro.");
            }


        }


    }
}
