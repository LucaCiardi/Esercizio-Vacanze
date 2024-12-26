namespace Entities
{
    public class Atleta : Utility.Entity
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public DateTime Dob { get; set; }
        public string Nazione { get; set; }
        public List<Medaglia> Medaglie { get; set; } = [];

    }
}
