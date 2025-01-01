using System;
using DAOs;
using Utility;
using Entities;

public class MenuManagement
{
    private readonly IDatabase _database;

    private MenuManagement(IDatabase database)
    {
        _database = database;
    }
    private static MenuManagement? instance = null;
    private static readonly object _lock = new();

    public static MenuManagement GetInstance(IDatabase database)
    {
        if (instance == null)
        {
            lock (_lock)
            {
                instance ??= new MenuManagement(database);
            }
        }
        return instance;
    }

    public void DisplayMenu()
    {
        Console.WriteLine("Menù:");
        Console.WriteLine("1. Ottieni tutti gli atleti");
        Console.WriteLine("2. Trova un atleta per ID");
        Console.WriteLine("3. Crea un nuovo atleta");
        Console.WriteLine("4. Aggiorna un atleta esistente");
        Console.WriteLine("5. Elimina un atleta");
        Console.WriteLine("6. Ottieni tutti gli eventi");
        Console.WriteLine("7. Trova un evento per ID");
        Console.WriteLine("8. Crea un nuovo evento");
        Console.WriteLine("9. Aggiorna un evento esistente");
        Console.WriteLine("10. Elimina un evento");
        Console.WriteLine("11. Ottieni tutte le gare");
        Console.WriteLine("12. Trova una gara per ID");
        Console.WriteLine("13. Crea una nuova gara");
        Console.WriteLine("14. Aggiorna una gara esistente");
        Console.WriteLine("15. Elimina una gara");
        Console.WriteLine("16. Ottieni tutte le medaglie");
        Console.WriteLine("17. Trova una medaglia per ID");
        Console.WriteLine("18. Crea una nuova medaglia");
        Console.WriteLine("19. Aggiorna una medaglia esistente");
        Console.WriteLine("20. Elimina una medaglia");
        Console.WriteLine("21. Visualizza medaglie per un atleta");
        Console.WriteLine("22. Visualizza eventi per un ID evento");
        Console.WriteLine("23. Visualizza il conteggio delle medaglie per un atleta");
        Console.WriteLine("24. Visualizza atleti per nazione");
        Console.WriteLine("25. Visualizza i vincitori di medaglie d'oro più anziani");
        Console.WriteLine("26. Visualizza medaglie sport di squadra");
        Console.WriteLine("27. Visualizza la categoria più vinta");
        Console.WriteLine("28. Aggiorna i dati tramite file .txt");
        Console.WriteLine("0. Esci");
    }

    public void StartMenu()
    {
        var atletaDAO = AtletaDAO.GetInstance(_database);
        var eventoDAO = EventoDAO.GetInstance(_database);
        var garaDAO = GaraDAO.GetInstance(_database);
        var medagliaDAO = MedagliaDAO.GetInstance(_database);
        var additionalMethods = AdditionalMethods.GetInstance(_database);

        while (true)
        {
            DisplayMenu();
            Console.Write("Seleziona un'opzione: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Input non valido. Per favore, inserisci un numero.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    var atleti = atletaDAO.GetRecords();
                    Console.WriteLine("Elenco degli atleti:");
                    foreach (Atleta atleta in atleti)
                    {
                        Console.WriteLine($"ID: {atleta.Id}, Nome: {atleta.Nome}, Cognome: {atleta.Cognome}, Data di Nascita: {atleta.Dob.ToShortDateString()}, Nazione: {atleta.Nazione}");
                    }
                    break;
                case 2:
                    Console.Write("Inserisci l'ID dell'atleta: ");
                    if (int.TryParse(Console.ReadLine(), out int atletaId))
                    {
                        var atleta = atletaDAO.FindRecord(atletaId) as Atleta;
                        if (atleta != null)
                        {
                            Console.WriteLine($"ID: {atleta.Id}, Nome: {atleta.Nome}, Cognome: {atleta.Cognome}, Data di Nascita: {atleta.Dob.ToShortDateString()}, Nazione: {atleta.Nazione}");
                        }
                        else
                        {
                            Console.WriteLine("Atleta non trovato.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID non valido.");
                    }
                    break;
                case 3:
                    var nuovoAtleta = new Atleta();
                    Console.Write("Inserisci il nome dell'atleta: ");
                    nuovoAtleta.Nome = Console.ReadLine();

                    Console.Write("Inserisci il cognome dell'atleta: ");
                    nuovoAtleta.Cognome = Console.ReadLine();

                    Console.Write("Inserisci la data di nascita dell'atleta (yyyy-mm-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime dob))
                    {
                        nuovoAtleta.Dob = dob;
                    }
                    else
                    {
                        Console.WriteLine("Data di nascita non valida.");
                        break;
                    }

                    Console.Write("Inserisci la nazione dell'atleta: ");
                    nuovoAtleta.Nazione = Console.ReadLine();

                    if (atletaDAO.CreateRecord(nuovoAtleta))
                    {
                        Console.WriteLine("Nuovo atleta creato con successo.");
                    }
                    else
                    {
                        Console.WriteLine("Errore durante la creazione del nuovo atleta.");
                    }
                    break;
                case 4:
                    Console.Write("Inserisci l'ID dell'atleta da aggiornare: ");
                    if (int.TryParse(Console.ReadLine(), out int updateId))
                    {
                        var atletaDaAggiornare = atletaDAO.FindRecord(updateId) as Atleta;
                        if (atletaDaAggiornare != null)
                        {
                            Console.Write("Inserisci il nuovo nome (lascia vuoto per mantenere invariato): ");
                            var nuovoNome = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovoNome))
                                atletaDaAggiornare.Nome = nuovoNome;

                            Console.Write("Inserisci il nuovo cognome (lascia vuoto per mantenere invariato): ");
                            var nuovoCognome = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovoCognome))
                                atletaDaAggiornare.Cognome = nuovoCognome;

                            Console.Write("Inserisci la nuova data di nascita (yyyy-mm-dd) (lascia vuoto per mantenere invariato): ");
                            var nuovaDobInput = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovaDobInput) && DateTime.TryParse(nuovaDobInput, out DateTime nuovaDob))
                                atletaDaAggiornare.Dob = nuovaDob;

                            Console.Write("Inserisci la nuova nazione (lascia vuoto per mantenere invariato): ");
                            var nuovaNazione = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovaNazione))
                                atletaDaAggiornare.Nazione = nuovaNazione;

                            if (atletaDAO.UpdateRecord(atletaDaAggiornare))
                            {
                                Console.WriteLine("Atleta aggiornato con successo.");
                            }
                            else
                            {
                                Console.WriteLine("Errore durante l'aggiornamento dell'atleta.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Atleta non trovato.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID non valido.");
                    }
                    break;
                case 5:
                    Console.Write("Inserisci l'ID dell'atleta da eliminare: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteId))
                    {
                        if (atletaDAO.DeleteRecord(deleteId))
                        {
                            Console.WriteLine("Atleta eliminato con successo.");
                        }
                        else
                        {
                            Console.WriteLine("Errore durante l'eliminazione dell'atleta.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID non valido.");
                    }
                    break;
                case 6:
                    var eventi = eventoDAO.GetRecords();
                    Console.WriteLine("Elenco degli eventi:");
                    foreach (Evento evento in eventi)
                    {
                        Console.WriteLine($"ID: {evento.Id}, Tipo: {evento.Tipo}, Anno: {evento.Anno}, Luogo: {evento.Luogo}");
                    }
                    break;
                case 7:
                    Console.Write("Inserisci l'ID dell'evento: ");
                    if (int.TryParse(Console.ReadLine(), out int eventoId))
                    {
                        var evento = eventoDAO.FindRecord(eventoId) as Evento;
                        if (evento != null)
                        {
                            Console.WriteLine($"ID: {evento.Id}, Tipo: {evento.Tipo}, Anno: {evento.Anno}, Luogo: {evento.Luogo}");
                        }
                        else
                        {
                            Console.WriteLine("Evento non trovato.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID non valido.");
                    }
                    break;
                case 8:
                    var nuovoEvento = new Evento();
                    Console.Write("Inserisci il tipo di evento: ");
                    nuovoEvento.Tipo = Console.ReadLine();

                    Console.Write("Inserisci l'anno dell'evento: ");
                    if (int.TryParse(Console.ReadLine(), out int anno))
                    {
                        nuovoEvento.Anno = anno;
                    }
                    else
                    {
                        Console.WriteLine("Anno non valido.");
                        break;
                    }

                    Console.Write("Inserisci il luogo dell'evento: ");
                    nuovoEvento.Luogo = Console.ReadLine();

                    if (eventoDAO.CreateRecord(nuovoEvento))
                    {
                        Console.WriteLine("Nuovo evento creato con successo.");
                    }
                    else
                    {
                        Console.WriteLine("Errore durante la creazione del nuovo evento.");
                    }
                    break;
                case 9:
                    Console.Write("Inserisci l'ID dell'evento da aggiornare: ");
                    if (int.TryParse(Console.ReadLine(), out int updateEventId))
                    {
                        var eventoDaAggiornare = eventoDAO.FindRecord(updateEventId) as Evento;
                        if (eventoDaAggiornare != null)
                        {
                            Console.Write("Inserisci il nuovo tipo (lascia vuoto per mantenere invariato): ");
                            var nuovoTipo = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovoTipo))
                                eventoDaAggiornare.Tipo = nuovoTipo;

                            Console.Write("Inserisci il nuovo anno (lascia vuoto per mantenere invariato): ");
                            var nuovoAnnoInput = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovoAnnoInput) && int.TryParse(nuovoAnnoInput, out int nuovoAnno))
                                eventoDaAggiornare.Anno = nuovoAnno;

                            Console.Write("Inserisci il nuovo luogo (lascia vuoto per mantenere invariato): ");
                            var nuovoLuogo = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovoLuogo))
                                eventoDaAggiornare.Luogo = nuovoLuogo;

                            if (eventoDAO.UpdateRecord(eventoDaAggiornare))
                            {
                                Console.WriteLine("Evento aggiornato con successo.");
                            }
                            else
                            {
                                Console.WriteLine("Errore durante l'aggiornamento dell'evento.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Evento non trovato.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID non valido.");
                    }
                    break;
                case 10:
                    Console.Write("Inserisci l'ID dell'evento da eliminare: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteEventId))
                    {
                        if (eventoDAO.DeleteRecord(deleteEventId))
                        {
                            Console.WriteLine("Evento eliminato con successo.");
                        }
                        else
                        {
                            Console.WriteLine("Errore durante l'eliminazione dell'evento.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID non valido.");
                    }
                    break;
                case 11:
                    var gare = garaDAO.GetRecords();
                    Console.WriteLine("Elenco delle gare:");
                    foreach (Gara gara in gare)
                    {
                        Console.WriteLine($"ID: {gara.Id}, Nome: {gara.Nome}, Categoria: {gara.Categoria}, Indoor: {gara.Indoor}, Squadra: {gara.Squadra}");
                    }
                    break;
                case 12:
                    Console.Write("Inserisci l'ID della gara: ");
                    if (int.TryParse(Console.ReadLine(), out int garaId))
                    {
                        var gara = garaDAO.FindRecord(garaId) as Gara;
                        if (gara != null)
                        {
                            Console.WriteLine($"ID: {gara.Id}, Nome: {gara.Nome}, Categoria: {gara.Categoria}, Indoor: {gara.Indoor}, Squadra: {gara.Squadra}");
                        }
                        else
                        {
                            Console.WriteLine("Gara non trovata.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID non valido.");
                    }
                    break;
                case 13:
                    var nuovaGara = new Gara();
                    Console.Write("Inserisci il nome della gara: ");
                    nuovaGara.Nome = Console.ReadLine();

                    Console.Write("Inserisci la categoria della gara: ");
                    nuovaGara.Categoria = Console.ReadLine();

                    Console.Write("La gara è indoor? (sì/no): ");
                    var indoorInput = Console.ReadLine();
                    nuovaGara.Indoor = indoorInput.Equals("sì", StringComparison.OrdinalIgnoreCase);

                    Console.Write("La gara è di squadra? (sì/no): ");
                    var squadraInput = Console.ReadLine();
                    nuovaGara.Squadra = squadraInput.Equals("sì", StringComparison.OrdinalIgnoreCase);

                    if (garaDAO.CreateRecord(nuovaGara))
                    {
                        Console.WriteLine("Nuova gara creata con successo.");
                    }
                    else
                    {
                        Console.WriteLine("Errore durante la creazione della nuova gara.");
                    }
                    break;
                case 14:
                    Console.Write("Inserisci l'ID della gara da aggiornare: ");
                    if (int.TryParse(Console.ReadLine(), out int updateGaraId))
                    {
                        var garaDaAggiornare = garaDAO.FindRecord(updateGaraId) as Gara;
                        if (garaDaAggiornare != null)
                        {
                            Console.Write("Inserisci il nuovo nome (lascia vuoto per mantenere invariato): ");
                            var nuovoNomeGara = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovoNomeGara))
                                garaDaAggiornare.Nome = nuovoNomeGara;

                            Console.Write("Inserisci la nuova categoria (lascia vuoto per mantenere invariato): ");
                            var nuovaCategoriaGara = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovaCategoriaGara))
                                garaDaAggiornare.Categoria = nuovaCategoriaGara;

                            Console.Write("La gara è indoor? (sì/no) (lascia vuoto per mantenere invariato): ");
                            var nuovaIndoorInput = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovaIndoorInput))
                                garaDaAggiornare.Indoor = nuovaIndoorInput.Equals("sì", StringComparison.OrdinalIgnoreCase);

                            Console.Write("La gara è di squadra? (sì/no) (lascia vuoto per mantenere invariato): ");
                            var nuovaSquadraInput = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovaSquadraInput))
                                garaDaAggiornare.Squadra = nuovaSquadraInput.Equals("sì", StringComparison.OrdinalIgnoreCase);

                            if (garaDAO.UpdateRecord(garaDaAggiornare))
                            {
                                Console.WriteLine("Gara aggiornata con successo.");
                            }
                            else
                            {
                                Console.WriteLine("Errore durante l'aggiornamento della gara.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Gara non trovata.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID non valido.");
                    }
                    break;
                case 15:
                    Console.Write("Inserisci l'ID della gara da eliminare: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteGaraId))
                    {
                        if (garaDAO.DeleteRecord(deleteGaraId))
                        {
                            Console.WriteLine("Gara eliminata con successo.");
                        }
                        else
                        {
                            Console.WriteLine("Errore durante l'eliminazione della gara.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID non valido.");
                    }
                    break;
                case 16:
                    var medaglie = medagliaDAO.GetRecords();
                    Console.WriteLine("Elenco delle medaglie:");
                    foreach (Medaglia medaglia in medaglie)
                    {
                        Console.WriteLine($"Podio: {medaglia.Podio}, Evento: {medaglia.Evento?.Tipo} ({medaglia.Evento?.Anno}), Luogo: {medaglia.Evento?.Luogo}, Gara: {medaglia.Gara?.Nome}, Categoria: {medaglia.Gara?.Categoria}, Indoor: {medaglia.Gara?.Indoor}, Squadra: {medaglia.Gara?.Squadra}");
                    }
                    break;
                case 17:
                    Console.Write("Inserisci l'ID della medaglia: ");
                    if (int.TryParse(Console.ReadLine(), out int medagliaId))
                    {
                        var medaglia = medagliaDAO.FindRecord(medagliaId) as Medaglia;
                        if (medaglia != null)
                        {
                            Console.WriteLine($"Podio: {medaglia.Podio}, Evento: {medaglia.Evento?.Tipo} ({medaglia.Evento?.Anno}), Luogo: {medaglia.Evento?.Luogo}, Gara: {medaglia.Gara?.Nome}, Categoria: {medaglia.Gara?.Categoria}, Indoor: {medaglia.Gara?.Indoor}, Squadra: {medaglia.Gara?.Squadra}");
                        }
                        else
                        {
                            Console.WriteLine("Medaglia non trovata.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID non valido.");
                    }
                    break;
                case 18:
                    var nuovaMedaglia = new Medaglia();
                    Console.Write("Inserisci il podio della medaglia (Oro, Argento, Bronzo): ");
                    nuovaMedaglia.Podio = Console.ReadLine();

                    Console.Write("Inserisci l'ID dell'evento: ");
                    if (int.TryParse(Console.ReadLine(), out int eventoIdCreate))
                    {
                        nuovaMedaglia.Evento = eventoDAO.FindRecord(eventoIdCreate) as Evento;
                    }
                    else
                    {
                        Console.WriteLine("ID evento non valido.");
                        break;
                    }

                    Console.Write("Inserisci l'ID della gara: ");
                    if (int.TryParse(Console.ReadLine(), out int garaIdCreate))
                    {
                        nuovaMedaglia.Gara = garaDAO.FindRecord(garaIdCreate) as Gara;
                    }
                    else
                    {
                        Console.WriteLine("ID gara non valido.");
                        break;
                    }

                    // Ask for IdAtleta
                    Console.Write("Inserisci l'ID dell'atleta (lascia vuoto per mantenere invariato): ");
                    var atletaIdCreateInput = Console.ReadLine();
                    int? atletaIdCreate = null;
                    if (!string.IsNullOrEmpty(atletaIdCreateInput) && int.TryParse(atletaIdCreateInput, out int idAtletaCreate))
                    {
                        atletaIdCreate = idAtletaCreate;
                    }

                    if (medagliaDAO.CreateRecordWithIdAtleta(nuovaMedaglia, atletaIdCreate))
                    {
                        Console.WriteLine("Nuova medaglia creata con successo.");
                    }
                    else
                    {
                        Console.WriteLine("Errore durante la creazione della nuova medaglia.");
                    }
                    break;

                case 19:
                    Console.Write("Inserisci l'ID della medaglia da aggiornare: ");
                    if (int.TryParse(Console.ReadLine(), out int updateMedagliaId))
                    {
                        var medagliaDaAggiornare = medagliaDAO.FindRecord(updateMedagliaId) as Medaglia;
                        if (medagliaDaAggiornare != null)
                        {
                            Console.Write("Inserisci il nuovo podio (lascia vuoto per mantenere invariato): ");
                            var nuovoPodio = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovoPodio))
                                medagliaDaAggiornare.Podio = nuovoPodio;

                            Console.Write("Inserisci il nuovo ID dell'evento (lascia vuoto per mantenere invariato): ");
                            var eventoIdUpdateInput = Console.ReadLine();
                            if (!string.IsNullOrEmpty(eventoIdUpdateInput) && int.TryParse(eventoIdUpdateInput, out int eventoIdUpdate))
                            {
                                var eventoUpdate = eventoDAO.FindRecord(eventoIdUpdate) as Evento;
                                if (eventoUpdate != null)
                                    medagliaDaAggiornare.Evento = eventoUpdate;
                                else
                                    Console.WriteLine("Evento non trovato.");
                            }

                            Console.Write("Inserisci il nuovo ID della gara (lascia vuoto per mantenere invariato): ");
                            var garaIdUpdateInput = Console.ReadLine();
                            if (!string.IsNullOrEmpty(garaIdUpdateInput) && int.TryParse(garaIdUpdateInput, out int garaIdUpdate))
                            {
                                var garaUpdate = garaDAO.FindRecord(garaIdUpdate) as Gara;
                                if (garaUpdate != null)
                                    medagliaDaAggiornare.Gara = garaUpdate;
                                else
                                    Console.WriteLine("Gara non trovata.");
                            }

                            // Ask for IdAtleta
                            Console.Write("Inserisci l'ID dell'atleta (lascia vuoto per mantenere invariato): ");
                            var atletaIdUpdateInput = Console.ReadLine();
                            int? atletaIdUpdate = null;
                            if (!string.IsNullOrEmpty(atletaIdUpdateInput) && int.TryParse(atletaIdUpdateInput, out int idAtletaUpdate))
                            {
                                atletaIdUpdate = idAtletaUpdate;
                            }

                            if (medagliaDAO.UpdateRecordWithIdAtleta(medagliaDaAggiornare, atletaIdUpdate))
                            {
                                Console.WriteLine("Medaglia aggiornata con successo.");
                            }
                            else
                            {
                                Console.WriteLine("Errore durante l'aggiornamento della medaglia.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Medaglia non trovata.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID non valido.");
                    }
                    break;
                case 20:
                    Console.Write("Inserisci l'ID della medaglia da eliminare: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteMedagliaId))
                    {
                        if (medagliaDAO.DeleteRecord(deleteMedagliaId))
                        {
                            Console.WriteLine("Medaglia eliminata con successo.");
                        }
                        else
                        {
                            Console.WriteLine("Errore durante l'eliminazione della medaglia.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID non valido.");
                    }
                    break;
                case 21:
                    Console.Write("Inserisci l'ID dell'atleta: ");
                    if (int.TryParse(Console.ReadLine(), out int atletaId21))
                    {
                        additionalMethods.DisplayMedals(atletaId21);
                    }
                    else
                    {
                        Console.WriteLine("ID non valido.");
                    }
                    break;
                case 22:
                    Console.Write("Inserisci l'ID dell'evento: ");
                    if (int.TryParse(Console.ReadLine(), out int eventoId22))
                    {
                        additionalMethods.DisplayEvents(eventoId22);
                    }
                    else
                    {
                        Console.WriteLine("ID non valido.");
                    }
                    break;
                case 23:
                    Console.Write("Inserisci l'ID dell'atleta: ");
                    if (int.TryParse(Console.ReadLine(), out int atletaId23))
                    {
                        additionalMethods.DisplayMedalsCount(atletaId23);
                    }
                    else
                    {
                        Console.WriteLine("ID non valido.");
                    }
                    break;
                case 24:
                    Console.Write("Inserisci la nazione: ");
                    var nazione24 = Console.ReadLine();
                    additionalMethods.DisplayAthletesByNation(nazione24);
                    break;
                case 25:
                    additionalMethods.DisplayOldestGoldMedalists();
                    break;
                case 26:
                    additionalMethods.DisplayTeamSportMedals();
                    break;
                case 27:
                    additionalMethods.DisplayMostWonCategory();
                    break;
                case 28:
                    Console.Write("Inserisci il percorso del file .txt: ");
                    var filePath28 = Console.ReadLine();
                    if (File.Exists(filePath28))
                    {
                        Console.WriteLine("Seleziona il tipo di dati nel file:");
                        Console.WriteLine("1. Atleti");
                        Console.WriteLine("2. Eventi");
                        Console.WriteLine("3. Gare");
                        Console.WriteLine("4. Medagliere");
                        if (int.TryParse(Console.ReadLine(), out int fileType))
                        {
                            var lines = File.ReadAllLines(filePath28);
                            var startIndex = 0;

                            if (!int.TryParse(lines[0].Split(';')[0], out _))
                            {
                                startIndex = 1; // Skip the first row if it's not a number
                            }

                            for (int i = startIndex; i < lines.Length; i++)
                            {
                                var data = lines[i].Split(';');
                                switch (fileType)
                                {
                                    case 1 when data.Length >= 5:
                                        var atleta = new Atleta
                                        {
                                            Id = int.Parse(data[0]),
                                            Nome = data[1],
                                            Cognome = data[2],
                                            Dob = DateTime.Parse(data[3]),
                                            Nazione = data[4]
                                        };
                                        if (_database.RecordExists("Atleti", atleta.Id))
                                        {
                                            Console.WriteLine($"L'id {atleta.Id} esiste già. Vuoi sostituire i dati esistenti? (sì/no)");
                                            var response = Console.ReadLine();
                                            if (response != null && response.Equals("sì", StringComparison.OrdinalIgnoreCase))
                                            {
                                                atletaDAO.UpdateRecord(atleta);
                                            }
                                        }
                                        else
                                        {
                                            atletaDAO.CreateRecord(atleta);
                                        }
                                        break;
                                    case 2 when data.Length >= 4:
                                        var evento = new Evento
                                        {
                                            Id = int.Parse(data[0]),
                                            Tipo = data[1],
                                            Anno = int.Parse(data[2]),
                                            Luogo = data[3]
                                        };
                                        if (_database.RecordExists("Eventi", evento.Id))
                                        {
                                            Console.WriteLine($"L'id {evento.Id} esiste già. Vuoi sostituire i dati esistenti? (sì/no)");
                                            var response = Console.ReadLine();
                                            if (response != null && response.Equals("sì", StringComparison.OrdinalIgnoreCase))
                                            {
                                                eventoDAO.UpdateRecord(evento);
                                            }
                                        }
                                        else
                                        {
                                            eventoDAO.CreateRecord(evento);
                                        }
                                        break;
                                    case 3 when data.Length >= 5:
                                        var gara = new Gara
                                        {
                                            Id = int.Parse(data[0]),
                                            Nome = data[1],
                                            Categoria = data[2],
                                            Indoor = data[3] == "1",
                                            Squadra = data[4] == "1"
                                        };
                                        if (_database.RecordExists("Gare", gara.Id))
                                        {
                                            Console.WriteLine($"L'id {gara.Id} esiste già. Vuoi sostituire i dati esistenti? (sì/no)");
                                            var response = Console.ReadLine();
                                            if (response != null && response.Equals("sì", StringComparison.OrdinalIgnoreCase))
                                            {
                                                garaDAO.UpdateRecord(gara);
                                            }
                                        }
                                        else
                                        {
                                            garaDAO.CreateRecord(gara);
                                        }
                                        break;
                                    case 4 when data.Length >= 5:
                                        int? id = (data[0] == "NULL" || string.IsNullOrWhiteSpace(data[0])) ? (int?)null : int.Parse(data[0]);
                                        int? idAtleta = (data[1] == "NULL" || string.IsNullOrWhiteSpace(data[1])) ? (int?)null : int.Parse(data[1]);
                                        int? idGara = (data[2] == "NULL" || string.IsNullOrWhiteSpace(data[2])) ? (int?)null : int.Parse(data[2]);
                                        int? idEvento = (data[3] == "NULL" || string.IsNullOrWhiteSpace(data[3])) ? (int?)null : int.Parse(data[3]);
                                        string podio = data[4];

                                        // Ensure idGara is valid or created
                                        if (idGara.HasValue && garaDAO.FindRecord(idGara.Value) == null)
                                        {
                                            Console.WriteLine($"Gara con ID {idGara} non trovata. Vuoi creare una nuova Gara con ID {idGara}? (sì/no)");
                                            var response = Console.ReadLine();
                                            if (response != null && response.Equals("sì", StringComparison.OrdinalIgnoreCase))
                                            {
                                                var newGara = new Gara
                                                {
                                                    Id = idGara.Value,
                                                    Nome = "New Gara",
                                                    Categoria = "New Categoria",
                                                    Indoor = false,
                                                    Squadra = false
                                                };
                                                garaDAO.CreateRecord(newGara);
                                            }
                                            else
                                            {
                                                Console.WriteLine("Medaglia non inserita");
                                                continue;
                                            }
                                        }

                                        // Ensure idEvento is valid or created
                                        if (idEvento.HasValue && eventoDAO.FindRecord(idEvento.Value) == null)
                                        {
                                            Console.WriteLine($"Evento con ID {idEvento} non trovato. Vuoi creare un nuovo Evento con ID {idEvento}? (sì/no)");
                                            var response = Console.ReadLine();
                                            if (response != null && response.Equals("sì", StringComparison.OrdinalIgnoreCase))
                                            {
                                                var newEvento = new Evento
                                                {
                                                    Id = idEvento.Value,
                                                    Tipo = "New Evento",
                                                    Anno = default,
                                                    Luogo = "New Luogo"
                                                };
                                                eventoDAO.CreateRecord(newEvento);
                                            }
                                            else
                                            {
                                                Console.WriteLine("Medaglia non inserita");
                                                continue;
                                            }
                                        }

                                        // Ensure idAtleta is valid or created
                                        if (idAtleta.HasValue && atletaDAO.FindRecord(idAtleta.Value) == null)
                                        {
                                            Console.WriteLine($"Atleta con ID {idAtleta} non trovato. Vuoi creare un nuovo Atleta con ID {idAtleta}? (sì/no)");
                                            var response = Console.ReadLine();
                                            if (response != null && response.Equals("sì", StringComparison.OrdinalIgnoreCase))
                                            {
                                                var newAtleta = new Atleta
                                                {
                                                    Id = idAtleta.Value,
                                                    Nome = "New Nome",
                                                    Cognome = "New Cognome",
                                                    Dob = DateTime.Now,
                                                    Nazione = "New Nazione"
                                                };
                                                atletaDAO.CreateRecord(newAtleta);
                                            }
                                            else
                                            {
                                                Console.WriteLine("Medaglia non inserita");
                                                continue;
                                            }
                                        }

                                        var medaglia = new Medaglia
                                        {
                                            Id = id ?? medagliaDAO.GetNextId("Medagliere"),
                                            Podio = podio,
                                            Evento = idEvento.HasValue ? new Evento { Id = idEvento.Value } : null,
                                            Gara = idGara.HasValue ? new Gara { Id = idGara.Value } : null
                                        };

                                        if (_database.RecordExists("Medagliere", medaglia.Id))
                                        {
                                            Console.WriteLine($"L'id {medaglia.Id} esiste già. Vuoi sostituire i dati esistenti? (sì/no)");
                                            var response = Console.ReadLine();
                                            if (response != null && response.Equals("sì", StringComparison.OrdinalIgnoreCase))
                                            {
                                                medagliaDAO.UpdateRecordWithIdAtleta(medaglia, idAtleta);
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                medagliaDAO.CreateRecordWithIdAtleta(medaglia, idAtleta);
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine($"Errore durante l'inserimento della medaglia: {ex.Message}");
                                            }
                                        }
                                        break;
                                    default:
                                        Console.WriteLine("Riga non valida o incompleta: " + lines[i]);
                                        break;
                                }
                            }
                            Console.WriteLine("Dati aggiornati con successo.");
                        }
                        else
                        {
                            Console.WriteLine("Input non valido. Per favore, inserisci un numero.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("File non trovato.");
                    }
                    break;
                case 0:
                    Console.WriteLine("Uscita in corso...");
                    return;
                default:
                    Console.WriteLine("Opzione non valida. Per favore, riprova.");
                    break;
            }
        }
    }
}
