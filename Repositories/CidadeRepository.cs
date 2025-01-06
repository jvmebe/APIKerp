using APIKerp.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

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

        public async Task<(bool Success, string ErrorMessage)> Update(int idEmpresa, int idEstado, Estados estado)
        {
            estado.DataModificacao = DateTime.Now;
            try
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    await db.OpenAsync();

                    using (var transaction = db.BeginTransaction())
                    {
                        string query = @"
                            UPDATE Cidade 
                            SET 
                                IdEstado = @IdEstado,
                                Cidade = @Cidade,
                                Ddd = @Ddd,
                                Ativo = @Ativo,
                                DataModificacao = @DataModificacao
                            WHERE IdEmpresa = @IdEmpresa AND IdCidade = @IdCidade";

                        var parameters = new
                        {
                            IdEmpresa = idEmpresa,
                            IdEstado = idEstado,
                            IdPais = estado.IdPais,
                            Estado = estado.Estado,
                            Sigla = estado.Sigla,
                            PerIcms = estado.PercIcms,
                            IcmsInt = estado.IcmsInt,
                            DataModificacao = estado.DataModificacao,
                            PerRedSt = estado.PerRedSt,
                            CodigoWeb = estado.CodigoWeb
                        };

                        int affectedRows = await db.ExecuteAsync(query, parameters, transaction);

                        if (affectedRows == 0)
                        {
                            transaction.Rollback();
                            return (false, "País não encontrado");
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


        public async Task<(bool Success, string ErrorMessage)> Delete(int idEmpresa, int idPais)
        {

            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string query = @"DELETE FROM Cidade WHERE IdEmpresa = @IdEmpresa AND IdCidade = @IdCidade";

                    var affectedRows = await db.ExecuteAsync(query, new
                    {
                        IdEmpresa = idEmpresa,
                        IdPais = idPais
                    });

                    return (affectedRows > 0, null);
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return (false, "Não é possível excluir esta cidade: Existem cadastros vinculados.");
            }
            catch (Exception ex)
            {
                return (false, "Ocorreu um erro.");
            }


        }


    }
}
