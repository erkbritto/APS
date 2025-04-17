using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using APS.Models;

namespace APS.Data
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "A string de conexão 'DefaultConnection' não foi encontrada no arquivo de configuração.");
        }

        public void SaveCalculo(Calculo calculo)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand(
                        "INSERT INTO Calculos (Tipo, Entrada, Resultado, Timestamp) VALUES (@Tipo, @Entrada, @Resultado, @Timestamp)",
                        connection);
                    command.Parameters.AddWithValue("@Tipo", calculo.Tipo);
                    command.Parameters.AddWithValue("@Entrada", calculo.Entrada);
                    command.Parameters.AddWithValue("@Resultado", calculo.Resultado);
                    command.Parameters.AddWithValue("@Timestamp", calculo.Timestamp);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao salvar no MySQL: {ex.Message}");
                throw;
            }
        }
    }
}