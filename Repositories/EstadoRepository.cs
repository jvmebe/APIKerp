using APIKerp.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace APIKerp.Repositories
{
    public class EstadoRepository
    {
        private readonly string _connectionString;

        public EstadoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Estados>> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT e.IdEstado AS Id, e.IdEmpresa, e.Estado, e.Sigla, e.PercIcms, e.IcmsInt, e.DataCadastro, e.DataModificacao, e.PerRedSt, e.CodigoWeb, p.IdPais as IdPais, p.Pais as NomePais
                 FROM Estado e
                 JOIN Pais p ON e.IdPais = p.IdPais;";
                return await db.QueryAsync<Estados>(query);
            }
        }

        public async Task<Estados> GetOne(int idEmpresa, int idEstado)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT *
                FROM Estado
                WHERE IdEmpresa = @IdEmpresa AND IdEstado = @IdEstado";

                return await db.QueryFirstOrDefaultAsync<Estados>(query, new { IdEmpresa = idEmpresa, IdEstado = idEstado });

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
                            UPDATE Estado 
                            SET 
                                IdPais = @IdPais,
                                Estado = @Estado,
                                Sigla = @Sigla,
                                PerIcms = @PerIcms,
                                IcmsInt = @IcmsInt,
                                DataModificacao = @DataModificacao,
                                PerRedSt = @PerRedSt,
                                CodigoWeb = @CodigoWeb
                            WHERE IdEmpresa = @IdEmpresa AND IdEstado = @IdEstado";

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
                    string query = @"DELETE FROM Estado WHERE IdEmpresa = @IdEmpresa AND IdEstado = @IdEstado";

                    var affectedRows = await db.ExecuteAsync(query, new
                    {
                        IdEmpresa = idEmpresa,
                        IdPais = idPais
                    });

                    return (affectedRows > 0, null);
                }
            }
            catch (SqlException ex) when(ex.Number == 547)
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
