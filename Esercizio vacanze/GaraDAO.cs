using Microsoft.Data.SqlClient;
using Entities;
using Utility;

namespace DAOs
{
    public class GaraDAO : IDAO
    {
        private static GaraDAO? _instance;
        private static readonly object _lock = new();
        private readonly IDatabase _db;

        private GaraDAO(IDatabase db)
        {
            _db = db;
        }

        public static GaraDAO GetInstance(IDatabase db)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new GaraDAO(db);
                }
            }
            return _instance;
        }

        public List<Entity> GetRecords()
        {
            var result = new List<Entity>();
            var command = new SqlCommand("SELECT * FROM Gare");
            var fullResponse = _db.ReadDb(command);
            if (fullResponse == null) return result;

            foreach (var singleResponse in fullResponse)
            {
                var gara = new Gara();
                gara.FromDictionary(singleResponse);
                result.Add(gara);
            }
            return result;
        }

        public Entity? FindRecord(int recordId)
        {
            var command = new SqlCommand("SELECT * FROM Gare WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", recordId);
            var singleResponse = _db.ReadOneDb(command);
            if (singleResponse == null) return null;

            var gara = new Gara();
            gara.FromDictionary(singleResponse);
            return gara;
        }

        public bool CreateRecord(Entity entity)
        {
            if (entity is not Gara gara)
                return false;

            var command = new SqlCommand(
                "INSERT INTO Gare (Id, Nome, Categoria, Indoor, Squadra) VALUES (@Id, @Nome, @Categoria, @Indoor, @Squadra)");
            command.Parameters.AddWithValue("@Id", gara.Id);
            command.Parameters.AddWithValue("@Nome", StringUtils.EscapeSingleQuotes(gara.Nome));
            command.Parameters.AddWithValue("@Categoria", StringUtils.EscapeSingleQuotes(gara.Categoria));
            command.Parameters.AddWithValue("@Indoor", gara.Indoor);
            command.Parameters.AddWithValue("@Squadra", gara.Squadra);
            return _db.UpdateDb(command);
        }

        public bool UpdateRecord(Entity entity)
        {
            if (entity is not Gara gara)
                return false;

            var command = new SqlCommand(
                "UPDATE Gare SET Nome = @Nome, Categoria = @Categoria, Indoor = @Indoor, Squadra = @Squadra WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", gara.Id);
            command.Parameters.AddWithValue("@Nome", StringUtils.EscapeSingleQuotes(gara.Nome));
            command.Parameters.AddWithValue("@Categoria", StringUtils.EscapeSingleQuotes(gara.Categoria));
            command.Parameters.AddWithValue("@Indoor", gara.Indoor);
            command.Parameters.AddWithValue("@Squadra", gara.Squadra);
            return _db.UpdateDb(command);
        }

        public bool DeleteRecord(int recordId)
        {
            var command = new SqlCommand("DELETE FROM Gare WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", recordId);
            return _db.UpdateDb(command);
        }
    }
}
