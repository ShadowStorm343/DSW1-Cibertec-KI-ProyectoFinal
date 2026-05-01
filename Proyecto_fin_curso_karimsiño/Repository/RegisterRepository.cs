using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_fin_curso_karimsiño.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace Proyecto_fin_curso_karimsiño.Repository
{
    public class RegisterRepository
    {
        private readonly string _connectionString;

        public RegisterRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("proyecto_dsw1");
        }
        public async Task AgregarRegisterAsync(RegisterModel p)
        {
            var sql = @"INSERT INTO Register (Nombre, Correo, Clave, Rol)
                        VALUES (@Nombre, @Correo, @Clave, @Rol)";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Nombre", p.nombre);
                cmd.Parameters.AddWithValue("@Correo", p.correo);
                cmd.Parameters.AddWithValue("@Clave", p.clave);
                cmd.Parameters.AddWithValue("@Rol", p.rol ?? (object)DBNull.Value);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task<RegisterModel?> ObtenerPorIdAsync(int id)
        {
            var sql = "SELECT Id, Nombre, Correo, Clave, Rol FROM Register WHERE Id = @Id";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                await conn.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new RegisterModel
                        {
                            Id = reader.GetInt32(0),
                            nombre = reader.IsDBNull(1) ? "" : reader.GetString(1),
                            correo = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            clave = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            rol = reader.IsDBNull(4) ? "" : reader.GetString(4)
                        };
                    }
                }
            }

            return null;
        }
        public async Task ActualizarClaveRolAsync(RegisterModel p)
        {
            var sql = @"UPDATE Register
                        SET Clave = @Clave,
                            Rol   = @Rol
                        WHERE Id = @Id";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Clave", p.clave);
                cmd.Parameters.AddWithValue("@Rol", p.rol ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", p.Id);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task EliminarAsync(int id)
        {
            var sql = "DELETE FROM Register WHERE Id = @Id";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task<IEnumerable<RegisterModel>> ObtenerPaginadoAsync(int pagina, int porPagina)
        {
            var lista = new List<RegisterModel>();

            var sql = @"SELECT Id, Nombre, Correo, Clave, Rol
                        FROM Register
                        ORDER BY Id
                        OFFSET @Offset ROWS
                        FETCH NEXT @PorPagina ROWS ONLY";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Offset", (pagina - 1) * porPagina);
                cmd.Parameters.AddWithValue("@PorPagina", porPagina);

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new RegisterModel
                        {
                            Id = reader.GetInt32(0),
                            nombre = reader.IsDBNull(1) ? "" : reader.GetString(1),
                            correo = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            clave = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            rol = reader.IsDBNull(4) ? "" : reader.GetString(4)
                        });
                    }
                }
            }

            return lista;
        }
        public async Task<int> ContarTotalAsync()
        {
            var sql = "SELECT COUNT(*) FROM Register";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync();
                return (int)await cmd.ExecuteScalarAsync();
            }
        }
        public async Task<IEnumerable<RegisterModel>> ObtenerTodosAsync()
        {
            var lista = new List<RegisterModel>();
            var sql = "SELECT Id, Nombre, Correo, Clave, Rol FROM Register";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new RegisterModel
                        {
                            Id = reader.GetInt32(0),
                            nombre = reader.IsDBNull(1) ? "" : reader.GetString(1),
                            correo = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            clave = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            rol = reader.IsDBNull(4) ? "" : reader.GetString(4)
                        });
                    }
                }
            }

            return lista;
        }
    }
}