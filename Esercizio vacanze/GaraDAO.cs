using Microsoft.Data.SqlClient;
using Entities;
using Utility;

namespace DAOs
{
    public class GaraDAO : IDAO
    {
        private static GaraDAO? _instance;
        private static readonly object _lock = new();
        private readonly string _connectionString;

        private GaraDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static GaraDAO GetInstance(string connectionString)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new GaraDAO(connectionString);
                }
            }
            return _instance;
        }

        public List<Entity> GetRecords()
        {
            var result = new List<Entity>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Gare", connection);
                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var gara = new Gara
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Categoria = reader.GetString(2),
                        Indoor = reader.GetBoolean(3),
                        Squadra = reader.GetBoolean(4)
                    };
                    result.Add(gara);
                }
            }
            return result;
        }

        public Entity? FindRecord(int recordId)
        {
            Gara? gara = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Gare WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", recordId);
                connection.Open();
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    gara = new Gara
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Categoria = reader.GetString(2),
                        Indoor = reader.GetBoolean(3),
                        Squadra = reader.GetBoolean(4)
                    };
                }
            }
            return gara;
        }

        public bool CreateRecord(Entity entity)
        {
            if (entity is not Gara gara)
                return false;

            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(
                "INSERT INTO Gare (Nome, Categoria, Indoor, Squadra) VALUES (@Nome, @Categoria, @Indoor, @Squadra)", connection);
            command.Parameters.AddWithValue("@Nome", StringUtils.EscapeSingleQuotes(gara.Nome));
            command.Parameters.AddWithValue("@Categoria", StringUtils.EscapeSingleQuotes(gara.Categoria));
            command.Parameters.AddWithValue("@Indoor", gara.Indoor);
            command.Parameters.AddWithValue("@Squadra", gara.Squadra);
            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }

        public bool UpdateRecord(Entity entity)
        {
            if (entity is not Gara gara)
                return false;

            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(
                "UPDATE Gare SET Nome = @Nome, Categoria = @Categoria, Indoor = @Indoor, Squadra = @Squadra WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", gara.Id);
            command.Parameters.AddWithValue("@Nome", StringUtils.EscapeSingleQuotes(gara.Nome));
            command.Parameters.AddWithValue("@Categoria", StringUtils.EscapeSingleQuotes(gara.Categoria));
            command.Parameters.AddWithValue("@Indoor", gara.Indoor);
            command.Parameters.AddWithValue("@Squadra", gara.Squadra);
            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }

        public bool DeleteRecord(int recordId)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("DELETE FROM Gare WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", recordId);
            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }
    }
}
