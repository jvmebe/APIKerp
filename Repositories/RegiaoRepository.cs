using APIKerp.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

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
                string query = @"SELECT IdEmpresa ,IdRegiao as Id, Regiao, Descricao, DataCadastro, DataModificacao, IdUsuario ,Ativo
                          FROM Regiao";
                return await db.QueryAsync<Regioes>(query);
            }
        }

        public async Task<Regioes> GetOne(int idEmpresa, int idRegiao)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT *
                FROM Regiao
                WHERE IdEmpresa = @IdEmpresa AND IdRegiao = @IdRegiao";

                return await db.QueryFirstOrDefaultAsync<Regioes>(query, new { IdEmpresa = idEmpresa, IdRegiao = idRegiao });

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

        public async Task<(bool Success, string ErrorMessage)> Update(int idEmpresa, int idRegiao, Regioes regiao)
        {
            //Define data de modificacao para a hora atual
            regiao.DataModificacao = DateTime.Now;

            try
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    await db.OpenAsync();


                    //Definindo o uso de transaction explicitamente
                    using (var transaction = db.BeginTransaction())
                    {
                        string query = @"
                            UPDATE Regiao
                            SET 
                                Regiao = @Regiao,
                                Descricao = @Descricao,
                                DataModificacao = @DataModificacao,
                                IdUsuario = @IdUsuario,
                                Ativo = @Ativo
                            WHERE IdEmpresa = @IdEmpresa AND IdRegiao = @IdRegiao";


                        //Define os valores dos parametros da query, já que nesse caso o dapper não mapeia automaticamente
                        var parameters = new
                        {
                            IdEmpresa = idEmpresa,
                            idRegiao = idRegiao,
                            Regiao = regiao.Regiao,
                            Descricao = regiao.Descricao,
                            IdUsuario = regiao.IdUsuario,
                            DataModificacao = regiao.DataModificacao,
                            Ativo = regiao.Ativo,
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
                return (false, "Um ou mais campos excedem o tamanho limite.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return (false, $"Erro");
            }
        }

        //Funcao para excluir um item
        public async Task<(bool Success, string ErrorMessage)> Delete(int idEmpresa, int idRegiao)
        {

            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string query = @"DELETE FROM Regiao WHERE IdEmpresa = @IdEmpresa AND IdRegiao = @IdRegiao";

                    //Define os parametros da querry com base nos valores recebidos pela função
                    var affectedRows = await db.ExecuteAsync(query, new
                    {
                        IdEmpresa = idEmpresa,
                        IdRegiao = idRegiao
                    });

                    if (affectedRows == 0)
                        return (false, "País não encontrado");

                    return (true, null);
                }
            }
            //Se o item não puder ser excluido por causa de uma dependencia em outra tabela, o Sql server retorna erro 547
            catch (SqlException ex) when (ex.Number == 547)
            {
                return (false, "Não é possível excluir esta região: Há dados vinculados.");
            }
            catch (Exception ex)
            {
                return (false, "Ocorreu um erro.");
            }
        }


    }
}
