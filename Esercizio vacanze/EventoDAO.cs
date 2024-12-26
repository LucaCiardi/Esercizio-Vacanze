using Entities;
using Microsoft.Data.SqlClient;
using Utility;

namespace DAOs
{
    public class EventoDAO : IDAO
    {
        private static EventoDAO? _instance;
        private static readonly object _lock = new();
        private readonly string _connectionString;

        private EventoDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static EventoDAO GetInstance(string connectionString)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new EventoDAO(connectionString);
                }
            }
            return _instance;
        }

        public List<Entity> GetRecords()
        {
            var result = new List<Entity>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Eventi", connection);
                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var evento = new Evento
                    {
                        Id = reader.GetInt32(0),
                        Tipo = reader.GetString(1),
                        Luogo = reader.GetString(2),
                        Anno = reader.GetInt32(3)
                    };
                    result.Add(evento);
                }
            }
            return result;
        }

        public Entity? FindRecord(int recordId)
        {
            Evento? evento = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Eventi WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", recordId);
                connection.Open();
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    evento = new Evento
                    {
                        Id = reader.GetInt32(0),
                        Tipo = reader.GetString(1),
                        Luogo = reader.GetString(2),
                        Anno = reader.GetInt32(3)
                    };
                }
            }
            return evento;
        }

        public bool CreateRecord(Entity entity)
        {
            if (entity is not Evento evento)
                return false;

            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(
                "INSERT INTO Eventi (Tipo, Luogo, Anno) VALUES (@Tipo, @Luogo, @Anno)", connection);
            command.Parameters.AddWithValue("@Tipo", StringUtils.EscapeSingleQuotes(evento.Tipo));
            command.Parameters.AddWithValue("@Luogo", StringUtils.EscapeSingleQuotes(evento.Luogo));
            command.Parameters.AddWithValue("@Anno", evento.Anno);
            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }

        public bool UpdateRecord(Entity entity)
        {
            if (entity is not Evento evento)
                return false;

            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(
                "UPDATE Eventi SET Tipo = @Tipo, Luogo = @Luogo, Anno = @Anno WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", evento.Id);
            command.Parameters.AddWithValue("@Tipo", StringUtils.EscapeSingleQuotes(evento.Tipo));
            command.Parameters.AddWithValue("@Luogo", StringUtils.EscapeSingleQuotes(evento.Luogo));
            command.Parameters.AddWithValue("@Anno", evento.Anno);
            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }

        public bool DeleteRecord(int recordId)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("DELETE FROM Eventi WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", recordId);
            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }
    }
}