using Microsoft.Data.SqlClient;
using Entities;
using Utility;

namespace DAOs
{
    public class AtletaDAO : IDAO
    {
        private static AtletaDAO? _instance;
        private static readonly object _lock = new();
        private readonly string _connectionString;

        private AtletaDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static AtletaDAO GetInstance(string connectionString)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new AtletaDAO(connectionString);
                }
            }
            return _instance;
        }

        public List<Entity> GetRecords()
        {
            var result = new List<Entity>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Atleti", connection);
                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var atleta = new Atleta
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Cognome = reader.GetString(2),
                        Dob = reader.GetDateTime(3),
                        Nazione = reader.GetString(4)
                    };
                    result.Add(atleta);
                }
            }
            return result;
        }

        public Entity? FindRecord(int recordId)
        {
            Atleta? atleta = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Atleti WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", recordId);
                connection.Open();
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    atleta = new Atleta
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Cognome = reader.GetString(2),
                        Dob = reader.GetDateTime(3),
                        Nazione = reader.GetString(4)
                    };
                }
            }
            return atleta;
        }

        public bool CreateRecord(Entity entity)
        {
            if (entity is not Atleta atleta)
                return false;

            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(
                "INSERT INTO Atleti (Nome, Cognome, Dob, Nazione) VALUES (@Nome, @Cognome, @Dob, @Nazione)", connection);
            command.Parameters.AddWithValue("@Nome", StringUtils.EscapeSingleQuotes(atleta.Nome));
            command.Parameters.AddWithValue("@Cognome", StringUtils.EscapeSingleQuotes(atleta.Cognome));
            command.Parameters.AddWithValue("@Dob", atleta.Dob);
            command.Parameters.AddWithValue("@Nazione", StringUtils.EscapeSingleQuotes(atleta.Nazione));
            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }

        public bool UpdateRecord(Entity entity)
        {
            if (entity is not Atleta atleta)
                return false;

            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(
                "UPDATE Atleti SET Nome = @Nome, Cognome = @Cognome, Dob = @Dob, Nazione = @Nazione WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", atleta.Id);
            command.Parameters.AddWithValue("@Nome", StringUtils.EscapeSingleQuotes(atleta.Nome));
            command.Parameters.AddWithValue("@Cognome", StringUtils.EscapeSingleQuotes(atleta.Cognome));
            command.Parameters.AddWithValue("@Dob", atleta.Dob);
            command.Parameters.AddWithValue("@Nazione", StringUtils.EscapeSingleQuotes(atleta.Nazione));
            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }

        public bool DeleteRecord(int recordId)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("DELETE FROM Atleti WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", recordId);
            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }
    }
}
