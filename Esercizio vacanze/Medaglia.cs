namespace Entities
{
    public class Medaglia : Utility.Entity
    {
        public string Podio { get; set; }
        public Evento Evento { get; set; }
        public Gara Gara { get; set; }
    }
}
