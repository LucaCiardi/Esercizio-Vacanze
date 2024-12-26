using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Entities;
using Utility;

namespace DAOs
{
    public class MedagliaDAO : IDAO
    {
        private static MedagliaDAO? _instance;
        private static readonly object _lock = new();
        private readonly IDatabase _db;

        private MedagliaDAO(IDatabase db)
        {
            _db = db;
        }

        public static MedagliaDAO GetInstance(IDatabase db)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new MedagliaDAO(db);
                }
            }
            return _instance;
        }

        public List<Entity> GetRecords()
        {
            var result = new List<Entity>();
            var command = new SqlCommand("SELECT * FROM Medagliere");
            var fullResponse = _db.ReadDb(command);
            if (fullResponse == null) return result;

            foreach (var singleResponse in fullResponse)
            {
                var medaglia = new Medaglia();
                medaglia.FromDictionary(singleResponse);
                result.Add(medaglia);
            }
            return result;
        }

        public Entity? FindRecord(int recordId)
        {
            var command = new SqlCommand("SELECT * FROM Medagliere WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", recordId);
            var singleResponse = _db.ReadOneDb(command);
            if (singleResponse == null) return null;

            var medaglia = new Medaglia();
            medaglia.FromDictionary(singleResponse);
            return medaglia;
        }

        public bool CreateRecord(Entity entity)
        {
            if (entity is not Medaglia medaglia)
                return false;

            var command = new SqlCommand(
                "INSERT INTO Medagliere (Podio, IdEvento, IdGara) VALUES (@Podio, @IdEvento, @IdGara)");
            command.Parameters.AddWithValue("@Podio", StringUtils.EscapeSingleQuotes(medaglia.Podio));
            command.Parameters.AddWithValue("@IdEvento", medaglia.Evento.Id);
            command.Parameters.AddWithValue("@IdGara", medaglia.Gara.Id);
            return _db.UpdateDb(command);
        }

        public bool UpdateRecord(Entity entity)
        {
            if (entity is not Medaglia medaglia)
                return false;

            var command = new SqlCommand(
                "UPDATE Medagliere SET Podio = @Podio, IdEvento = @IdEvento, IdGara = @IdGara WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", medaglia.Id);
            command.Parameters.AddWithValue("@Podio", StringUtils.EscapeSingleQuotes(medaglia.Podio));
            command.Parameters.AddWithValue("@IdEvento", medaglia.Evento.Id);
            command.Parameters.AddWithValue("@IdGara", medaglia.Gara.Id);
            return _db.UpdateDb(command);
        }

        public bool DeleteRecord(int recordId)
        {
            var command = new SqlCommand("DELETE FROM Medagliere WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", recordId);
            return _db.UpdateDb(command);
        }
    }
}
