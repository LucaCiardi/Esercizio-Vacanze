using Microsoft.Data.SqlClient;
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
            var command = new SqlCommand(@"
        SELECT m.*, 
               e.tipo as evento_tipo, 
               e.luogo as evento_luogo, 
               e.anno as evento_anno,
               g.nome as gara_nome, 
               g.categoria as gara_categoria, 
               g.indoor as gara_indoor, 
               g.squadra as gara_squadra
        FROM Medagliere m
        LEFT JOIN Eventi e ON m.IdEvento = e.Id
        LEFT JOIN Gare g ON m.IdGara = g.Id");

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
            var command = new SqlCommand(@"
        SELECT m.*, 
               e.tipo as evento_tipo, 
               e.luogo as evento_luogo, 
               e.anno as evento_anno,
               g.nome as gara_nome, 
               g.categoria as gara_categoria, 
               g.indoor as gara_indoor, 
               g.squadra as gara_squadra
        FROM Medagliere m
        LEFT JOIN Eventi e ON m.IdEvento = e.Id
        LEFT JOIN Gare g ON m.IdGara = g.Id
        WHERE m.Id = @Id");

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

            // Assign a new unique Id if it is not provided
            if (medaglia.Id == 0)
            {
                medaglia.Id = GetNextId("Medagliere");
            }

            var command = new SqlCommand(
                "INSERT INTO Medagliere (Id, IdAtleta, IdGara, IdEvento, medaglia) VALUES (@Id, @IdAtleta, @IdGara, @IdEvento, @Medaglia)");
            command.Parameters.AddWithValue("@Id", medaglia.Id);
            command.Parameters.AddWithValue("@IdAtleta", DBNull.Value); // Adjust if needed
            command.Parameters.AddWithValue("@IdGara", medaglia.Gara.Id);
            command.Parameters.AddWithValue("@IdEvento", medaglia.Evento.Id);
            command.Parameters.AddWithValue("@Medaglia", StringUtils.EscapeSingleQuotes(medaglia.Podio));
            return _db.UpdateDb(command);
        }

        public bool UpdateRecord(Entity entity)
        {
            if (entity is not Medaglia medaglia)
                return false;

            var command = new SqlCommand(
                "UPDATE Medagliere SET IdAtleta = @IdAtleta, IdGara = @IdGara, IdEvento = @IdEvento, medaglia = @Medaglia WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", medaglia.Id);
            command.Parameters.AddWithValue("@IdAtleta", DBNull.Value); // Adjust if needed
            command.Parameters.AddWithValue("@IdGara", medaglia.Gara.Id);
            command.Parameters.AddWithValue("@IdEvento", medaglia.Evento.Id);
            command.Parameters.AddWithValue("@Medaglia", StringUtils.EscapeSingleQuotes(medaglia.Podio));
            return _db.UpdateDb(command);
        }

        public bool DeleteRecord(int recordId)
        {
            var command = new SqlCommand("DELETE FROM Medagliere WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", recordId);
            return _db.UpdateDb(command);
        }

        public int GetNextId(string tableName)
        {
            var query = $"SELECT ISNULL(MAX(Id), 0) + 1 FROM {tableName}";
            using (var command = new SqlCommand(query, _db.Connection))
            {
                _db.Connection.Open();
                var nextId = (int)command.ExecuteScalar();
                _db.Connection.Close();
                return nextId;
            }
        }

        public int? GetIdAtletaFromInputOrDatabase(string[] data)
        {
            if (data.Length > 1) // Ensure there's enough data to retrieve idatleta
            {
                // Check if the value is "NULL" or whitespace
                if (data[1] == "NULL" || string.IsNullOrWhiteSpace(data[1]))
                {
                    return null;
                }

                // Try to parse the idatleta value
                if (int.TryParse(data[1], out int idAtleta))
                {
                    return idAtleta;
                }
            }

            // Return null if the idatleta cannot be retrieved or parsed
            return null;
        }

        public bool CreateRecordWithIdAtleta(Medaglia medaglia, int? idAtleta)
        {
            var command = new SqlCommand(
                "INSERT INTO Medagliere (Id, IdAtleta, IdGara, IdEvento, medaglia) VALUES (@Id, @IdAtleta, @IdGara, @IdEvento, @Medaglia)");
            command.Parameters.AddWithValue("@Id", medaglia.Id);
            command.Parameters.AddWithValue("@IdAtleta", idAtleta.HasValue ? (object)idAtleta.Value : DBNull.Value);
            command.Parameters.AddWithValue("@IdGara", medaglia.Gara?.Id ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IdEvento", medaglia.Evento?.Id ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Medaglia", StringUtils.EscapeSingleQuotes(medaglia.Podio));
            return _db.UpdateDb(command);
        }

        public bool UpdateRecordWithIdAtleta(Medaglia medaglia, int? idAtleta)
        {
            var command = new SqlCommand(
                "UPDATE Medagliere SET IdAtleta = @IdAtleta, IdGara = @IdGara, IdEvento = @IdEvento, medaglia = @Medaglia WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", medaglia.Id);
            command.Parameters.AddWithValue("@IdAtleta", idAtleta.HasValue ? (object)idAtleta.Value : DBNull.Value);
            command.Parameters.AddWithValue("@IdGara", medaglia.Gara.Id);
            command.Parameters.AddWithValue("@IdEvento", medaglia.Evento.Id);
            command.Parameters.AddWithValue("@Medaglia", StringUtils.EscapeSingleQuotes(medaglia.Podio));
            return _db.UpdateDb(command);
        }
    }
}
