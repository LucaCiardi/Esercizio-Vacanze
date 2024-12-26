using Microsoft.Data.SqlClient;
using Entities;
using Utility;

namespace DAOs
{
    public class MedagliaDAO : IDAO
    {
        private static MedagliaDAO? _instance;
        private static readonly object _lock = new();
        private readonly string _connectionString;

        private MedagliaDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static MedagliaDAO GetInstance(string connectionString)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new MedagliaDAO(connectionString);
                }
            }
            return _instance;
        }

        public List<Entity> GetRecords()
        {
            var result = new List<Entity>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Medagliere", connection);
                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var medaglia = new Medaglia
                    {
                        Id = reader.GetInt32(0),
                        Podio = reader.GetString(1),
                        Evento = new Evento
                        {
                            Id = reader.GetInt32(2),
                            Tipo = reader.GetString(3),
                            Luogo = reader.GetString(4),
                            Anno = reader.GetInt32(5)
                        },
                        Gara = new Gara
                        {
                            Id = reader.GetInt32(6),
                            Nome = reader.GetString(7),
                            Categoria = reader.GetString(8),
                            Indoor = reader.GetBoolean(9),
                            Squadra = reader.GetBoolean(10)
                        }
                    };
                    result.Add(medaglia);
                }
            }
            return result;
        }

        public Entity? FindRecord(int recordId)
        {
            Medaglia? medaglia = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Medagliere WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", recordId);
                connection.Open();
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    medaglia = new Medaglia
                    {
                        Id = reader.GetInt32(0),
                        Podio = reader.GetString(1),
                        Evento = new Evento
                        {
                            Id = reader.GetInt32(2),
                            Tipo = reader.GetString(3),
                            Luogo = reader.GetString(4),
                            Anno = reader.GetInt32(5)
                        },
                        Gara = new Gara
                        {
                            Id = reader.GetInt32(6),
                            Nome = reader.GetString(7),
                            Categoria = reader.GetString(8),
                            Indoor = reader.GetBoolean(9),
                            Squadra = reader.GetBoolean(10)
                        }
                    };
                }
            }
            return medaglia;
        }

        public bool CreateRecord(Entity entity)
        {
            if (entity is not Medaglia medaglia)
                return false;

            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(
                "INSERT INTO Medagliere (Podio, IdEvento, IdGara) VALUES (@Podio, @IdEvento, @IdGara)", connection);
            command.Parameters.AddWithValue("@Podio", StringUtils.EscapeSingleQuotes(medaglia.Podio));
            command.Parameters.AddWithValue("@IdEvento", medaglia.Evento.Id);
            command.Parameters.AddWithValue("@IdGara", medaglia.Gara.Id);
            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }

        public bool UpdateRecord(Entity entity)
        {
            if (entity is not Medaglia medaglia)
                return false;

            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(
                "UPDATE Medagliere SET Podio = @Podio, IdEvento = @IdEvento, IdGara = @IdGara WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", medaglia.Id);
            command.Parameters.AddWithValue("@Podio", StringUtils.EscapeSingleQuotes(medaglia.Podio));
            command.Parameters.AddWithValue("@IdEvento", medaglia.Evento.Id);
            command.Parameters.AddWithValue("@IdGara", medaglia.Gara.Id);
            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }

        public bool DeleteRecord(int recordId)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("DELETE FROM Medagliere WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", recordId);
            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }
    }
}
