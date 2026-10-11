using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using web_api_cursos.Models;


namespace web_api_cursos.Repository.SQLServerRepository
{
    public class CursoRepository : ICursoRepository<Models.Curso>
    {
        private readonly string connectionString;

        public CursoRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<Curso> CreateAsync(Curso curso)
        {
            using (SqlConnection connetion = new SqlConnection(connectionString))
            {
                await connetion.OpenAsync();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connetion;
                    command.CommandText = "insert into curso(Nome, CargaHoraria, Valor, DataInicio,Online ,Ativo)values(@Nome, @CargaHoraria, @valor, @DataInicio, @Online, @Ativo)select convert (int, SCOPE_IDENTITY());";

                    if (curso.Nome != null)
                        command.Parameters.Add(new SqlParameter("@Nome", System.Data.SqlDbType.VarChar)).Value = curso.Nome;
                    else
                        command.Parameters.Add(new SqlParameter("@Nome", System.Data.SqlDbType.VarChar)).Value = DBNull.Value;

                    command.Parameters.Add(new SqlParameter("@CargaHoraria", System.Data.SqlDbType.Int)).Value = curso.CargaHoraria;
                    command.Parameters.Add(new SqlParameter("@Valor", System.Data.SqlDbType.Decimal)).Value = curso.Valor;
                    command.Parameters.Add(new SqlParameter("@DataInicio", System.Data.SqlDbType.DateTime)).Value = curso.DataInicio.ToString("yyyy/MM/dd");
                    command.Parameters.Add(new SqlParameter("@Online", System.Data.SqlDbType.Bit)).Value = curso.Online;
                    command.Parameters.Add(new SqlParameter("@Ativo", System.Data.SqlDbType.Bit)).Value = curso.Ativo;

                    curso.Id = Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
            return curso;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            Curso curso = await ReadAsync(id);

            if (curso == null)
                return false;

            if (curso.Ativo)
                throw new CursoAtivoException();

            int affectedRows = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "delete from curso where Id = @Id";
                    command.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.Int)).Value = id;

                    affectedRows = await command.ExecuteNonQueryAsync();
                }
            }
            return affectedRows == 1;
        }

        public async Task<List<Curso>> ReadAsync()
        {
            List<Models.Curso> cursos = new List<Models.Curso>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "select Id, Nome, CargaHoraria, Valor, DataInicio, Online , Ativo from curso;";
                    SqlDataReader dr = await command.ExecuteReaderAsync();
                    while (await dr.ReadAsync())
                    {
                        Models.Curso curso = new Models.Curso();

                        curso.Id = Convert.ToInt32(dr["Id"]);
                        if (dr["Nome"] != DBNull.Value)
                            curso.Nome = (string)dr["Nome"];
                        curso.CargaHoraria = Convert.ToInt32(dr["CargaHoraria"]);
                        curso.Valor = Convert.ToDecimal(dr["Valor"]);
                        curso.DataInicio = Convert.ToDateTime(dr["DataInicio"]);
                        curso.Online = Convert.ToBoolean(dr["Online"]);
                        curso.Ativo = Convert.ToBoolean(dr["Ativo"]);
                        cursos.Add(curso);
                    }
                }
            }
            return cursos;
        }

        public async Task<Curso> ReadAsync(int id)
        {
            Models.Curso curso = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "select Id, Nome, CargaHoraria, Valor, DataInicio, Online , Ativo from curso where Id = @Id;";
                    command.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.Int)).Value = id;

                    SqlDataReader dr = await command.ExecuteReaderAsync();
                    if (await dr.ReadAsync())
                    {
                        curso = new Models.Curso();

                        curso.Id = Convert.ToInt32(dr["Id"]);
                        if (dr["Nome"] != DBNull.Value)
                            curso.Nome = (string)dr["Nome"];
                        curso.CargaHoraria = Convert.ToInt32(dr["CargaHoraria"]);
                        curso.Valor = Convert.ToDecimal(dr["Valor"]);
                        curso.DataInicio = Convert.ToDateTime(dr["DataInicio"]);
                        curso.Online = Convert.ToBoolean(dr["Online"]);
                        curso.Ativo = Convert.ToBoolean(dr["Ativo"]);
                    }
                }
            }
            return curso;
        }

        public async Task<bool> UpdateAsync(Curso curso)
        {
            int affectedRows = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "update curso set Nome = @Nome, CargaHoraria = @CargaHoraria, Valor = @Valor, DataInicio = @DataInicio, Online = @Online, Ativo = @Ativo where Id = @Id ";

                    if (curso.Nome != null)
                        command.Parameters.Add(new SqlParameter("@Nome", System.Data.SqlDbType.VarChar)).Value = curso.Nome;
                    else
                        command.Parameters.Add(new SqlParameter("@Nome", System.Data.SqlDbType.VarChar)).Value = DBNull.Value;

                    command.Parameters.Add(new SqlParameter("@CargaHoraria", System.Data.SqlDbType.Int)).Value = curso.CargaHoraria;
                    command.Parameters.Add(new SqlParameter("@Valor", System.Data.SqlDbType.Decimal)).Value = curso.Valor;
                    command.Parameters.Add(new SqlParameter("@DataInicio", System.Data.SqlDbType.DateTime)).Value = curso.DataInicio.ToString("yyyy/MM/dd");
                    command.Parameters.Add(new SqlParameter("@Online", System.Data.SqlDbType.Bit)).Value = curso.Online;
                    command.Parameters.Add(new SqlParameter("@Ativo", System.Data.SqlDbType.Bit)).Value = curso.Ativo;

                    command.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.Int)).Value = curso.Id;

                    affectedRows = await command.ExecuteNonQueryAsync();
                }
            }            
            return affectedRows == 1;
        }
    }
}