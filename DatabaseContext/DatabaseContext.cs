using System.IO;
using System.Text.Json;
using Database.Contracts;
using Domain;
using Npgsql;
namespace Database
{
    public class DatabaseContext : IDatabaseContext
    {
        private readonly string _connectionString;
        private readonly NpgsqlConnection _connection;
        public DatabaseContext(IDatabaseConfig databaseConfig)
        {
            _connectionString = databaseConfig.Configuration["ConnectionStrings:DefaultConnection"] ?? throw new InvalidOperationException("Connection string not found");
            try
            {
                _connection = new NpgsqlConnection(_connectionString);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to establish database connection: " + ex.Message);
            }
        }

        public int Execute(ParameterisedQuery query)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                using var command = new NpgsqlCommand(query.query, connection);

                foreach (var param in query.parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw new Exception("Database execution failed: " + ex.Message);
            }
        }
        public object ExecuteScalar(ParameterisedQuery query)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                using var command = new NpgsqlCommand(query.query, connection);

                foreach (var param in query.parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }

                return command.ExecuteScalar() ?? throw new InvalidOperationException("Query returned null");
            }
            catch (Exception ex)
            {
                throw new Exception("Database scalar execution failed: " + ex.Message);
            }
        }


        public List<T> GetData<T>(ParameterisedQuery query)
        {
            var result = new List<T>();

            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                using var command = new NpgsqlCommand(query.query, connection);

                foreach (var param in query.parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var row = new Dictionary<string, object?>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var value = reader.GetValue(i);
                        row[reader.GetName(i)] = value is DBNull ? null : value;
                    }

                    string json = JsonSerializer.Serialize(row);
                    T? item = JsonSerializer.Deserialize<T>(json);

                    if (item != null)
                    {
                        result.Add(item);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Data retrieval failed: " + ex.Message);
            }
        }
    }
}