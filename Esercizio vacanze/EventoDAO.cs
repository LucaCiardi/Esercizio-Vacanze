using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Entities;
using Utility;

namespace DAOs
{
    public class EventoDAO : IDAO
    {
        private static EventoDAO? _instance;
        private static readonly object _lock = new();
        private readonly IDatabase _db;

        private EventoDAO(IDatabase db)
        {
            _db = db;
        }

        public static EventoDAO GetInstance(IDatabase db)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new EventoDAO(db);
                }
            }
            return _instance;
        }

        public List<Entity> GetRecords()
        {
            var result = new List<Entity>();
            var command = new SqlCommand("SELECT * FROM Eventi");
            var fullResponse = _db.ReadDb(command);
            if (fullResponse == null) return result;

            foreach (var singleResponse in fullResponse)
            {
                var evento = new Evento();
                evento.FromDictionary(singleResponse);
                result.Add(evento);
            }
            return result;
        }

        public Entity? FindRecord(int recordId)
        {
            var command = new SqlCommand("SELECT * FROM Eventi WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", recordId);
            var singleResponse = _db.ReadOneDb(command);
            if (singleResponse == null) return null;

            var evento = new Evento();
            evento.FromDictionary(singleResponse);
            return evento;
        }

        public bool CreateRecord(Entity entity)
        {
            if (entity is not Evento evento)
                return false;

            var command = new SqlCommand(
                "INSERT INTO Eventi (Tipo, Luogo, Anno) VALUES (@Tipo, @Luogo, @Anno)");
            command.Parameters.AddWithValue("@Tipo", StringUtils.EscapeSingleQuotes(evento.Tipo));
            command.Parameters.AddWithValue("@Luogo", StringUtils.EscapeSingleQuotes(evento.Luogo));
            command.Parameters.AddWithValue("@Anno", evento.Anno);
            return _db.UpdateDb(command);
        }

        public bool UpdateRecord(Entity entity)
        {
            if (entity is not Evento evento)
                return false;

            var command = new SqlCommand(
                "UPDATE Eventi SET Tipo = @Tipo, Luogo = @Luogo, Anno = @Anno WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", evento.Id);
            command.Parameters.AddWithValue("@Tipo", StringUtils.EscapeSingleQuotes(evento.Tipo));
            command.Parameters.AddWithValue("@Luogo", StringUtils.EscapeSingleQuotes(evento.Luogo));
            command.Parameters.AddWithValue("@Anno", evento.Anno);
            return _db.UpdateDb(command);
        }

        public bool DeleteRecord(int recordId)
        {
            var command = new SqlCommand("DELETE FROM Eventi WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", recordId);
            return _db.UpdateDb(command);
        }
    }
}
