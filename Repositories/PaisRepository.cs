using APIKerp.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace APIKerp.Repositories
{
    public class PaisRepository
    {
        private readonly string _connectionString;


        //Inicializa o repositorio com a connection string indicada em appsettings.json. Esse valor é inicializado no program.cs
        public PaisRepository(string connectionString)
        {
            _connectionString = connectionString;
        }


        //Funcao para buscar todos os itens
        public async Task<IEnumerable<Paises>> GetAll()
        {
            //Inicializa uma IDbConnection usando o _connectionString
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT IdPais as Id, IdEmpresa, Pais, Sigla, Ddi, DataCadastro, DataModificacao, Nacional FROM Pais ";
             
                //Usa a query e mapeia automaticamente em uma lista de objetos do tipo Paises usando o Dapper. Para isso funcionar as tabelas da query tem que ter o mesmo nome que os atributos da clasee
                return await db.QueryAsync<Paises>(query);
            }
        }

        //Funcao para retornar um único item
        public async Task<Paises> GetOne(int idEmpresa, int idPais)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT *
                FROM Pais
                WHERE IdEmpresa = @IdEmpresa AND IdPais = @IdPais";

                return await db.QueryFirstOrDefaultAsync<Paises>(query, new { IdEmpresa = idEmpresa, IdPais = idPais });

            }
        }

        //Funcao para criar um item
        public async Task<object> Create(Paises pais)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Pais (IdEmpresa, Pais, Sigla, Ddi, DataCadastro, DataModificacao, Nacional) VALUES (@IdEmpresa, @Pais, @Sigla, @Ddi, @DataCadastro, @DataModificacao, @Nacional)

                   SELECT CAST(SCOPE_IDENTITY() as int);";

                return await db.QuerySingleAsync(query, pais);

            }
        }


        //Funcao para atualizar o item. Retorna um bool para indicar sucesso e uma mensagem de erro caso falhe
        public async Task<(bool Success, string ErrorMessage)> Update(int idEmpresa, int idPais, Paises pais)
        {
            //Define data de modificacao para a hora atual
            pais.DataModificacao = DateTime.Now;

            try
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    await db.OpenAsync();


                    //Definindo o uso de transaction explicitamente
                    using (var transaction = db.BeginTransaction())
                    {
                        string query = @"
                            UPDATE Pais 
                            SET 
                                Pais = @Pais,
                                Sigla = @Sigla,
                                Ddi = @Ddi,
                                DataModificacao = @DataModificacao,
                                Nacional = @Nacional
                            WHERE IdEmpresa = @IdEmpresa AND IdPais = @IdPais";


                        //Define os valores dos parametros da query, já que nesse caso o dapper não mapeia automaticamente
                        var parameters = new
                        {
                            IdEmpresa = idEmpresa,
                            IdPais = idPais,
                            Pais = pais.Pais,
                            Sigla = pais.Sigla,
                            Ddi = pais.Ddi,
                            DataModificacao = pais.DataModificacao,
                            Nacional = pais.Nacional
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
        public async Task<(bool Success, string ErrorMessage)> Delete(int idEmpresa, int idPais)
        {

            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string query = @"DELETE FROM Pais WHERE IdEmpresa = @IdEmpresa AND IdPais = @IdPais";

                    //Define os parametros da querry com base nos valores recebidos pela função
                    var affectedRows = await db.ExecuteAsync(query, new
                    {
                        IdEmpresa = idEmpresa,
                        IdPais = idPais
                    });

                    if (affectedRows == 0)
                        return (false, "País não encontrado");

                    return (true, null);
                }
            }
            //Se o item não puder ser excluido por causa de uma dependencia em outra tabela, o Sql server retorna erro 547
            catch (SqlException ex) when (ex.Number == 547)
            {
                return (false, "Não é possível excluir este país: Há um estado vinculado.");
            }
            catch (Exception ex)
            {
                return (false, "Ocorreu um erro.");
            }
        }

    }
}
