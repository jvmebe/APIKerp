using APIKerp.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

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

        public async Task<(bool Success, string ErrorMessage)> Update(int idEmpresa, int idLista, Listas lista)
        {

            lista.DataModificacao = DateTime.Now;

            try
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    await db.OpenAsync();

                    using (var transaction = db.BeginTransaction())
                    {
                        string query = @"
                            UPDATE Lista 
                            SET 
                                Lista = @Lista,
                                DescMax = @DescMax,
                                MargemLucro = @MargemLucro,
                                PerComissao = @PerComissao,
                                Todas = @Todas,
                                DataModificacao = @DataModificacao
                            WHERE IdEmpresa = @IdEmpresa AND IdLista = @IdLista";

                        var parameters = new
                        {
                            IdEmpresa = idEmpresa,
                            IdLista = idLista,
                            Lista = lista.Lista,
                            DescMax = lista.DescMax,
                            MargemLucro = lista.MargemLucro,
                            PerComissao = lista.PerComissao,
                            Todas = lista.Todas,
                            DataModificacao = lista.DataModificacao,
                            
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

        public async Task<(bool Success, string ErrorMessage)> Delete(int idEmpresa, int idLista)
        {

            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string query = @"DELETE FROM Lista WHERE IdEmpresa = @IdEmpresa AND IdLista = @IdLista";

                    var affectedRows = await db.ExecuteAsync(query, new
                    {
                        IdEmpresa = idEmpresa,
                        IdLista = idLista,
                    });

                    if (affectedRows == 0)
                        return (false, "Item não encontrado");

                    return (true, null);
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return (false, "Não é possível excluir este item: Há dados vinculados");
            }
            catch (Exception ex)

            {
                return (false, "Ocorreu um erro.");
            }
        }
    }
}
