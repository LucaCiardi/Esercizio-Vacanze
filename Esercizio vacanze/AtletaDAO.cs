using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Entities;
using Utility;

namespace DAOs
{
    public class AtletaDAO : IDAO
    {
        private static AtletaDAO? _instance;
        private static readonly object _lock = new();
        private readonly IDatabase _db;

        private AtletaDAO(IDatabase db)
        {
            _db = db;
        }

        public static AtletaDAO GetInstance(IDatabase db)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new AtletaDAO(db);
                }
            }
            return _instance;
        }

        public List<Entity> GetRecords()
        {
            var result = new List<Entity>();
            var command = new SqlCommand("SELECT * FROM Atleti");
            var fullResponse = _db.ReadDb(command);
            if (fullResponse == null) return result;

            foreach (var singleResponse in fullResponse)
            {
                var atleta = new Atleta();
                atleta.FromDictionary(singleResponse);
                result.Add(atleta);
            }
            return result;
        }

        public Entity? FindRecord(int recordId)
        {
            var command = new SqlCommand("SELECT * FROM Atleti WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", recordId);
            var singleResponse = _db.ReadOneDb(command);
            if (singleResponse == null) return null;

            var atleta = new Atleta();
            atleta.FromDictionary(singleResponse);
            return atleta;
        }

        public bool CreateRecord(Entity entity)
        {
            var atleta = entity as Atleta;
            if (atleta == null)
                throw new ArgumentException("Entity must be of type Atleta");

            var query = "INSERT INTO Atleti (Id, Nome, Cognome, Dob, Nazione) VALUES (@Id, @Nome, @Cognome, @Dob, @Nazione)";
            int? id = atleta.Id;

            if (!id.HasValue)
            {
                id = _db.GetNextId("Atleti");
            }

            if (_db.RecordExists("Atleti", id.Value))
            {
                Console.WriteLine($"L'id {id} esiste già. Vuoi sovrascrivere i dati esistenti? (sì/no)");
                var response = Console.ReadLine();
                if (response != null && response.Equals("sì", StringComparison.OrdinalIgnoreCase))
                {
                    atleta.Id = id.Value;
                    return UpdateRecord(atleta);
                }
                else
                {
                    id = _db.GetNextId("Atleti");
                }
            }

            using (var command = new SqlCommand(query, _db.Connection))
            {
                command.Parameters.AddWithValue("@Id", id.Value);
                command.Parameters.AddWithValue("@Nome", atleta.Nome);
                command.Parameters.AddWithValue("@Cognome", atleta.Cognome);
                command.Parameters.AddWithValue("@Dob", atleta.Dob);
                command.Parameters.AddWithValue("@Nazione", atleta.Nazione);
                _db.Connection.Open();
                var result = command.ExecuteNonQuery() > 0;
                _db.Connection.Close();
                return result;
            }
        }

        public bool UpdateRecord(Entity entity)
        {
            if (entity is not Atleta atleta)
                return false;

            var command = new SqlCommand(
                "UPDATE Atleti SET Nome = @Nome, Cognome = @Cognome, Dob = @Dob, Nazione = @Nazione WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", atleta.Id);
            command.Parameters.AddWithValue("@Nome", StringUtils.EscapeSingleQuotes(atleta.Nome));
            command.Parameters.AddWithValue("@Cognome", StringUtils.EscapeSingleQuotes(atleta.Cognome));
            command.Parameters.AddWithValue("@Dob", atleta.Dob);
            command.Parameters.AddWithValue("@Nazione", StringUtils.EscapeSingleQuotes(atleta.Nazione));
            return _db.UpdateDb(command);
        }

        public bool DeleteRecord(int recordId)
        {
            var command = new SqlCommand("DELETE FROM Atleti WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", recordId);
            return _db.UpdateDb(command);
        }
    }
}
