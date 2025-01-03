using Entities;

public class Medaglia : Utility.Entity
{
    public string Podio { get; set; }
    public Evento Evento { get; set; }
    public Gara Gara { get; set; }

    public override void FromDictionary(Dictionary<string, string> line)
    {
        // Call base implementation for Id property
        base.FromDictionary(line);

        // Handle Podio property
        if (line.ContainsKey("medaglia"))
        {
            Podio = line["medaglia"];
        }

        // Create and populate Evento
        if (line.ContainsKey("evento_tipo") || line.ContainsKey("evento_luogo") || line.ContainsKey("evento_anno"))
        {
            Evento = new Evento
            {
                Tipo = line.GetValueOrDefault("evento_tipo"),
                Luogo = line.GetValueOrDefault("evento_luogo"),
                Anno = int.TryParse(line.GetValueOrDefault("evento_anno"), out int anno) ? anno : 0
            };
        }

        // Create and populate Gara
        if (line.ContainsKey("gara_nome") || line.ContainsKey("gara_categoria") ||
            line.ContainsKey("gara_indoor") || line.ContainsKey("gara_squadra"))
        {
            Gara = new Gara
            {
                Nome = line.GetValueOrDefault("gara_nome"),
                Categoria = line.GetValueOrDefault("gara_categoria"),
                Indoor = bool.TryParse(line.GetValueOrDefault("gara_indoor"), out bool indoor) && indoor,
                Squadra = bool.TryParse(line.GetValueOrDefault("gara_squadra"), out bool squadra) && squadra
            };
        }
    }
}
