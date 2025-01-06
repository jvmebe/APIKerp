using APIKerp.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace APIKerp.Repositories
{
    public class RamoAtividadesRepository
    {
        private readonly string _connectionString;

        public RamoAtividadesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<RamoAtividades>> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM RamoAtividades";
                return await db.QueryAsync<RamoAtividades>(query);
            }
        }

        public async Task<RamoAtividades> GetOne(int idEmpresa, int idRamo)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT *
                FROM RamoAtividades
                WHERE IdEmpresa = @IdEmpresa AND IdRamo = @IdRamo";

                return await db.QueryFirstOrDefaultAsync<RamoAtividades>(query, new { IdEmpresa = idEmpresa, IdRamo = idRamo });

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


        public async Task<(bool Success, string ErrorMessage)> Update(int idEmpresa, int idRamo, RamoAtividades ramo)
        {
            //Define data de modificacao para a hora atual
            ramo.DataModificacao = DateTime.Now;

            try
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    await db.OpenAsync();


                    //Definindo o uso de transaction explicitamente
                    using (var transaction = db.BeginTransaction())
                    {
                        string query = @"
                            UPDATE RamoAtividades
                            SET 
                                NomeRamo = @NomeRamo,
                                DataModificacao = @DataModificacao,
                                Ativo = @Ativo
                            WHERE IdEmpresa = @IdEmpresa AND IdRamo = @IdRamo";


                        //Define os valores dos parametros da query, já que nesse caso o dapper não mapeia automaticamente
                        var parameters = new
                        {
                            IdEmpresa = idEmpresa,
                            idRamo = idRamo,
                            Ramo = ramo.NomeRamo,
                            DataModificacao = ramo.DataModificacao,
                            Ativo = ramo.Ativo
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

        public async Task<(bool Success, string ErrorMessage)> Delete(int idEmpresa, int idRamo)
        {

            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string query = @"DELETE FROM RamoAtividades WHERE IdEmpresa = @IdEmpresa AND IdRamo = @IdRamo";

                    var affectedRows = await db.ExecuteAsync(query, new
                    {
                        IdEmpresa = idEmpresa,
                        IdRamo = idRamo
                    });

                    if (affectedRows == 0)
                        return (false, "Ramo não encontrado");

                    return (true, null);
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return (false, "Não é possível excluir este país: Há um estado vinculado.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return (false, $"Ocorreu um erro. {ex}");
            }
        }


    }
}
