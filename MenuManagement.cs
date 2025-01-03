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
                case 1: // List all athletes
                    var atleti = atletaDAO.GetRecords();
                    Console.WriteLine("\nElenco degli atleti:\n");
                    Console.WriteLine("".PadRight(100, '-'));
                    Console.WriteLine($"{"ID",-5} {"NOME",-15} {"COGNOME",-15} {"DATA DI NASCITA",-15} {"NAZIONE",-15}");
                    Console.WriteLine("".PadRight(100, '-'));
                    foreach (Atleta atleta in atleti)
                    {
                        Console.WriteLine(
                            $"{atleta.Id,-5} " +
                            $"{atleta.Nome,-15} " +
                            $"{atleta.Cognome,-15} " +
                            $"{atleta.Dob.ToShortDateString(),-15} " +
                            $"{atleta.Nazione,-15}");
                    }
                    Console.WriteLine("".PadRight(100, '-'));
                    Console.WriteLine();
                    break;

                case 2: // Find athlete by ID
                    Console.Write("\nInserisci l'ID dell'atleta: ");
                    if (int.TryParse(Console.ReadLine(), out int atletaId))
                    {
                        var atleta = atletaDAO.FindRecord(atletaId) as Atleta;
                        if (atleta != null)
                        {
                            Console.WriteLine("\nDettagli Atleta:");
                            Console.WriteLine("".PadRight(100, '-'));
                            Console.WriteLine($"{"ID",-5} {"NOME",-15} {"COGNOME",-15} {"DATA DI NASCITA",-15} {"NAZIONE",-15}");
                            Console.WriteLine("".PadRight(100, '-'));
                            Console.WriteLine(
                                $"{atleta.Id,-5} " +
                                $"{atleta.Nome,-15} " +
                                $"{atleta.Cognome,-15} " +
                                $"{atleta.Dob.ToShortDateString(),-15} " +
                                $"{atleta.Nazione,-15}");
                            Console.WriteLine("".PadRight(100, '-'));
                        }
                        else
                        {
                            Console.WriteLine("\nAtleta non trovato.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nID non valido.");
                    }
                    Console.WriteLine();
                    break;

                case 3: // Create new athlete
                    var nuovoAtleta = new Atleta();
                    Console.WriteLine("\nInserimento nuovo atleta:\n");
                    Console.WriteLine("".PadRight(50, '-'));

                    Console.Write("ID (lasciare vuoto per assegnazione automatica): ");
                    var idInput = Console.ReadLine();
                    if (!string.IsNullOrEmpty(idInput) && int.TryParse(idInput, out int newId))
                    {
                        nuovoAtleta.Id = newId;
                    }

                    Console.Write("Nome: ");
                    nuovoAtleta.Nome = Console.ReadLine();

                    Console.Write("Cognome: ");
                    nuovoAtleta.Cognome = Console.ReadLine();

                    Console.Write("Data di nascita (yyyy-mm-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime dob))
                    {
                        nuovoAtleta.Dob = dob;
                    }
                    else
                    {
                        Console.WriteLine("\nData di nascita non valida.");
                        break;
                    }

                    Console.Write("Nazione: ");
                    nuovoAtleta.Nazione = Console.ReadLine();

                    Console.WriteLine("".PadRight(50, '-'));

                    if (atletaDAO.CreateRecord(nuovoAtleta))
                    {
                        Console.WriteLine("\nNuovo atleta creato con successo.");
                    }
                    else
                    {
                        Console.WriteLine("\nErrore durante la creazione del nuovo atleta.");
                    }
                    Console.WriteLine();
                    break;

                case 4: // Update athlete
                    Console.Write("\nInserisci l'ID dell'atleta da aggiornare: ");
                    if (int.TryParse(Console.ReadLine(), out int updateId))
                    {
                        var atletaDaAggiornare = atletaDAO.FindRecord(updateId) as Atleta;
                        if (atletaDaAggiornare != null)
                        {
                            Console.WriteLine("\nAggiornamento dati atleta:\n");
                            Console.WriteLine("".PadRight(50, '-'));

                            Console.Write("Nuovo nome (invio per mantenere invariato): ");
                            var nuovoNome = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovoNome))
                                atletaDaAggiornare.Nome = nuovoNome;

                            Console.Write("Nuovo cognome (invio per mantenere invariato): ");
                            var nuovoCognome = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovoCognome))
                                atletaDaAggiornare.Cognome = nuovoCognome;

                            Console.Write("Nuova data di nascita (yyyy-mm-dd) (invio per mantenere invariato): ");
                            var nuovaDobInput = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovaDobInput) && DateTime.TryParse(nuovaDobInput, out DateTime nuovaDob))
                                atletaDaAggiornare.Dob = nuovaDob;

                            Console.Write("Nuova nazione (invio per mantenere invariato): ");
                            var nuovaNazione = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovaNazione))
                                atletaDaAggiornare.Nazione = nuovaNazione;

                            Console.WriteLine("".PadRight(50, '-'));

                            if (atletaDAO.UpdateRecord(atletaDaAggiornare))
                            {
                                Console.WriteLine("\nAtleta aggiornato con successo.");
                            }
                            else
                            {
                                Console.WriteLine("\nErrore durante l'aggiornamento dell'atleta.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nAtleta non trovato.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nID non valido.");
                    }
                    Console.WriteLine();
                    break;

                case 5: // Delete athlete
                    Console.Write("\nInserisci l'ID dell'atleta da eliminare: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteId))
                    {
                        Console.WriteLine("".PadRight(50, '-'));
                        if (atletaDAO.DeleteRecord(deleteId))
                        {
                            Console.WriteLine("\nAtleta eliminato con successo.");
                        }
                        else
                        {
                            Console.WriteLine("\nErrore durante l'eliminazione dell'atleta.");
                        }
                        Console.WriteLine("".PadRight(50, '-'));
                    }
                    else
                    {
                        Console.WriteLine("\nID non valido.");
                    }
                    Console.WriteLine();
                    break;
                case 6: // List all events
                    var eventi = eventoDAO.GetRecords();
                    Console.WriteLine("\nElenco degli eventi:\n");
                    Console.WriteLine("".PadRight(80, '-'));
                    Console.WriteLine($"{"ID",-5} {"TIPO",-25} {"ANNO",-6} {"LUOGO",-30}");
                    Console.WriteLine("".PadRight(80, '-'));
                    foreach (Evento evento in eventi)
                    {
                        Console.WriteLine(
                            $"{evento.Id,-5} " +
                            $"{evento.Tipo,-25} " +
                            $"{evento.Anno,-6} " +
                            $"{evento.Luogo,-30}");
                    }
                    Console.WriteLine("".PadRight(80, '-'));
                    Console.WriteLine();
                    break;

                case 7: // Find event by ID
                    Console.Write("\nInserisci l'ID dell'evento: ");
                    if (int.TryParse(Console.ReadLine(), out int eventoId))
                    {
                        var evento = eventoDAO.FindRecord(eventoId) as Evento;
                        if (evento != null)
                        {
                            Console.WriteLine("\nDettagli Evento:");
                            Console.WriteLine("".PadRight(80, '-'));
                            Console.WriteLine($"{"ID",-5} {"TIPO",-25} {"ANNO",-6} {"LUOGO",-30}");
                            Console.WriteLine("".PadRight(80, '-'));
                            Console.WriteLine(
                                $"{evento.Id,-5} " +
                                $"{evento.Tipo,-25} " +
                                $"{evento.Anno,-6} " +
                                $"{evento.Luogo,-30}");
                            Console.WriteLine("".PadRight(80, '-'));
                        }
                        else
                        {
                            Console.WriteLine("\nEvento non trovato.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nID non valido.");
                    }
                    Console.WriteLine();
                    break;

                case 8: // Create new event
                    var nuovoEvento = new Evento();
                    Console.WriteLine("\nInserimento nuovo evento:\n");
                    Console.WriteLine("".PadRight(50, '-'));

                    Console.Write("ID (lasciare vuoto per assegnazione automatica): ");
                    var eventoIdInput = Console.ReadLine();
                    if (!string.IsNullOrEmpty(eventoIdInput) && int.TryParse(eventoIdInput, out int newEventId))
                    {
                        nuovoEvento.Id = newEventId;
                    }

                    Console.Write("Tipo evento: ");
                    nuovoEvento.Tipo = Console.ReadLine();

                    Console.Write("Anno: ");
                    if (int.TryParse(Console.ReadLine(), out int anno))
                    {
                        nuovoEvento.Anno = anno;
                    }
                    else
                    {
                        Console.WriteLine("\nAnno non valido.");
                        break;
                    }

                    Console.Write("Luogo: ");
                    nuovoEvento.Luogo = Console.ReadLine();

                    Console.WriteLine("".PadRight(50, '-'));

                    if (eventoDAO.CreateRecord(nuovoEvento))
                    {
                        Console.WriteLine("\nNuovo evento creato con successo.");
                    }
                    else
                    {
                        Console.WriteLine("\nErrore durante la creazione del nuovo evento.");
                    }
                    Console.WriteLine();
                    break;

                case 9: // Update event
                    Console.Write("\nInserisci l'ID dell'evento da aggiornare: ");
                    if (int.TryParse(Console.ReadLine(), out int updateEventId))
                    {
                        var eventoDaAggiornare = eventoDAO.FindRecord(updateEventId) as Evento;
                        if (eventoDaAggiornare != null)
                        {
                            Console.WriteLine("\nAggiornamento dati evento:\n");
                            Console.WriteLine("".PadRight(50, '-'));

                            Console.Write("Nuovo tipo (invio per mantenere invariato): ");
                            var nuovoTipo = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovoTipo))
                                eventoDaAggiornare.Tipo = nuovoTipo;

                            Console.Write("Nuovo anno (invio per mantenere invariato): ");
                            var nuovoAnnoInput = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovoAnnoInput) && int.TryParse(nuovoAnnoInput, out int nuovoAnno))
                                eventoDaAggiornare.Anno = nuovoAnno;

                            Console.Write("Nuovo luogo (invio per mantenere invariato): ");
                            var nuovoLuogo = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovoLuogo))
                                eventoDaAggiornare.Luogo = nuovoLuogo;

                            Console.WriteLine("".PadRight(50, '-'));

                            if (eventoDAO.UpdateRecord(eventoDaAggiornare))
                            {
                                Console.WriteLine("\nEvento aggiornato con successo.");
                            }
                            else
                            {
                                Console.WriteLine("\nErrore durante l'aggiornamento dell'evento.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nEvento non trovato.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nID non valido.");
                    }
                    Console.WriteLine();
                    break;

                case 10: // Delete event
                    Console.Write("\nInserisci l'ID dell'evento da eliminare: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteEventId))
                    {
                        Console.WriteLine("".PadRight(50, '-'));
                        if (eventoDAO.DeleteRecord(deleteEventId))
                        {
                            Console.WriteLine("\nEvento eliminato con successo.");
                        }
                        else
                        {
                            Console.WriteLine("\nErrore durante l'eliminazione dell'evento.");
                        }
                        Console.WriteLine("".PadRight(50, '-'));
                    }
                    else
                    {
                        Console.WriteLine("\nID non valido.");
                    }
                    Console.WriteLine();
                    break;
                case 11: // List all competitions
                    var gare = garaDAO.GetRecords();
                    Console.WriteLine("\nElenco delle gare:\n");
                    Console.WriteLine("".PadRight(100, '-'));
                    Console.WriteLine($"{"ID",-5} {"NOME",-25} {"CATEGORIA",-20} {"INDOOR",-8} {"SQUADRA",-8}");
                    Console.WriteLine("".PadRight(100, '-'));
                    foreach (Gara gara in gare)
                    {
                        Console.WriteLine(
                            $"{gara.Id,-5} " +
                            $"{gara.Nome,-25} " +
                            $"{gara.Categoria,-20} " +
                            $"{gara.Indoor,-8} " +
                            $"{gara.Squadra,-8}");
                    }
                    Console.WriteLine("".PadRight(100, '-'));
                    Console.WriteLine();
                    break;

                case 12: // Find competition by ID
                    Console.Write("\nInserisci l'ID della gara: ");
                    if (int.TryParse(Console.ReadLine(), out int garaId))
                    {
                        var gara = garaDAO.FindRecord(garaId) as Gara;
                        if (gara != null)
                        {
                            Console.WriteLine("\nDettagli Gara:");
                            Console.WriteLine("".PadRight(100, '-'));
                            Console.WriteLine($"{"ID",-5} {"NOME",-25} {"CATEGORIA",-20} {"INDOOR",-8} {"SQUADRA",-8}");
                            Console.WriteLine("".PadRight(100, '-'));
                            Console.WriteLine(
                                $"{gara.Id,-5} " +
                                $"{gara.Nome,-25} " +
                                $"{gara.Categoria,-20} " +
                                $"{gara.Indoor,-8} " +
                                $"{gara.Squadra,-8}");
                            Console.WriteLine("".PadRight(100, '-'));
                        }
                        else
                        {
                            Console.WriteLine("\nGara non trovata.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nID non valido.");
                    }
                    Console.WriteLine();
                    break;

                case 13: // Create new competition
                    var nuovaGara = new Gara();
                    Console.WriteLine("\nInserimento nuova gara:\n");
                    Console.WriteLine("".PadRight(50, '-'));

                    Console.Write("ID (lasciare vuoto per assegnazione automatica): ");
                    var garaIdInput = Console.ReadLine();
                    if (!string.IsNullOrEmpty(garaIdInput) && int.TryParse(garaIdInput, out int newGaraId))
                    {
                        nuovaGara.Id = newGaraId;
                    }

                    Console.Write("Nome: ");
                    nuovaGara.Nome = Console.ReadLine();

                    Console.Write("Categoria: ");
                    nuovaGara.Categoria = Console.ReadLine();

                    Console.Write("La gara è indoor? (sì/no): ");
                    var indoorInput = Console.ReadLine();
                    nuovaGara.Indoor = indoorInput?.Equals("sì", StringComparison.OrdinalIgnoreCase) ?? false;

                    Console.Write("La gara è di squadra? (sì/no): ");
                    var squadraInput = Console.ReadLine();
                    nuovaGara.Squadra = squadraInput?.Equals("sì", StringComparison.OrdinalIgnoreCase) ?? false;

                    Console.WriteLine("".PadRight(50, '-'));

                    if (garaDAO.CreateRecord(nuovaGara))
                    {
                        Console.WriteLine("\nNuova gara creata con successo.");
                    }
                    else
                    {
                        Console.WriteLine("\nErrore durante la creazione della nuova gara.");
                    }
                    Console.WriteLine();
                    break;

                case 14: // Update competition
                    Console.Write("\nInserisci l'ID della gara da aggiornare: ");
                    if (int.TryParse(Console.ReadLine(), out int updateGaraId))
                    {
                        var garaDaAggiornare = garaDAO.FindRecord(updateGaraId) as Gara;
                        if (garaDaAggiornare != null)
                        {
                            Console.WriteLine("\nAggiornamento dati gara:\n");
                            Console.WriteLine("".PadRight(50, '-'));

                            Console.Write("Nuovo nome (invio per mantenere invariato): ");
                            var nuovoNomeGara = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovoNomeGara))
                                garaDaAggiornare.Nome = nuovoNomeGara;

                            Console.Write("Nuova categoria (invio per mantenere invariato): ");
                            var nuovaCategoriaGara = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovaCategoriaGara))
                                garaDaAggiornare.Categoria = nuovaCategoriaGara;

                            Console.Write("La gara è indoor? (sì/no) (invio per mantenere invariato): ");
                            var nuovaIndoorInput = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovaIndoorInput))
                                garaDaAggiornare.Indoor = nuovaIndoorInput.Equals("sì", StringComparison.OrdinalIgnoreCase);

                            Console.Write("La gara è di squadra? (sì/no) (invio per mantenere invariato): ");
                            var nuovaSquadraInput = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovaSquadraInput))
                                garaDaAggiornare.Squadra = nuovaSquadraInput.Equals("sì", StringComparison.OrdinalIgnoreCase);

                            Console.WriteLine("".PadRight(50, '-'));

                            if (garaDAO.UpdateRecord(garaDaAggiornare))
                            {
                                Console.WriteLine("\nGara aggiornata con successo.");
                            }
                            else
                            {
                                Console.WriteLine("\nErrore durante l'aggiornamento della gara.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nGara non trovata.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nID non valido.");
                    }
                    Console.WriteLine();
                    break;

                case 15: // Delete competition
                    Console.Write("\nInserisci l'ID della gara da eliminare: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteGaraId))
                    {
                        Console.WriteLine("".PadRight(50, '-'));
                        if (garaDAO.DeleteRecord(deleteGaraId))
                        {
                            Console.WriteLine("\nGara eliminata con successo.");
                        }
                        else
                        {
                            Console.WriteLine("\nErrore durante l'eliminazione della gara.");
                        }
                        Console.WriteLine("".PadRight(50, '-'));
                    }
                    else
                    {
                        Console.WriteLine("\nID non valido.");
                    }
                    Console.WriteLine();
                    break;
                case 16: // List all medals
                    var medaglie = medagliaDAO.GetRecords();
                    Console.WriteLine("\nElenco delle medaglie:\n");
                    Console.WriteLine("".PadRight(130, '-'));
                    Console.WriteLine($"{"PODIO",-10} {"EVENTO",-25} {"ANNO",-6} {"LUOGO",-20} {"GARA",-25} {"CATEGORIA",-20} {"INDOOR",-8} {"SQUADRA",-8}");
                    Console.WriteLine("".PadRight(130, '-'));
                    foreach (Medaglia medaglia in medaglie)
                    {
                        Console.WriteLine(
                            $"{medaglia.Podio,-10} " +
                            $"{medaglia.Evento?.Tipo,-25} " +
                            $"{(medaglia.Evento?.Anno > 0 ? medaglia.Evento?.Anno.ToString() : ""),-6} " +
                            $"{medaglia.Evento?.Luogo,-20} " +
                            $"{medaglia.Gara?.Nome,-25} " +
                            $"{medaglia.Gara?.Categoria,-20} " +
                            $"{medaglia.Gara?.Indoor,-8} " +
                            $"{medaglia.Gara?.Squadra,-8}");
                    }
                    Console.WriteLine("".PadRight(130, '-'));
                    Console.WriteLine();
                    break;

                case 17: // Find medal by ID
                    Console.Write("\nInserisci l'ID della medaglia: ");
                    if (int.TryParse(Console.ReadLine(), out int medagliaId))
                    {
                        var medaglia = medagliaDAO.FindRecord(medagliaId) as Medaglia;
                        if (medaglia != null)
                        {
                            Console.WriteLine("\nDettagli Medaglia:");
                            Console.WriteLine("".PadRight(130, '-'));
                            Console.WriteLine($"{"PODIO",-10} {"EVENTO",-25} {"ANNO",-6} {"LUOGO",-20} {"GARA",-25} {"CATEGORIA",-20} {"INDOOR",-8} {"SQUADRA",-8}");
                            Console.WriteLine("".PadRight(130, '-'));
                            Console.WriteLine(
                                $"{medaglia.Podio,-10} " +
                                $"{medaglia.Evento?.Tipo,-25} " +
                                $"{(medaglia.Evento?.Anno > 0 ? medaglia.Evento?.Anno.ToString() : ""),-6} " +
                                $"{medaglia.Evento?.Luogo,-20} " +
                                $"{medaglia.Gara?.Nome,-25} " +
                                $"{medaglia.Gara?.Categoria,-20} " +
                                $"{medaglia.Gara?.Indoor,-8} " +
                                $"{medaglia.Gara?.Squadra,-8}");
                            Console.WriteLine("".PadRight(130, '-'));
                        }
                        else
                        {
                            Console.WriteLine("\nMedaglia non trovata.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nID non valido.");
                    }
                    Console.WriteLine();
                    break;

                case 18: // Create new medal
                    var nuovaMedaglia = new Medaglia();
                    Console.WriteLine("\nInserimento nuova medaglia:\n");
                    Console.WriteLine("".PadRight(50, '-'));

                    Console.Write("ID medaglia (invio per assegnazione automatica): ");
                    var medagliaIdInput = Console.ReadLine();
                    if (!string.IsNullOrEmpty(medagliaIdInput) && int.TryParse(medagliaIdInput, out int newMedagliaId))
                    {
                        nuovaMedaglia.Id = newMedagliaId;
                    }
                    else
                    {
                        nuovaMedaglia.Id = medagliaDAO.GetNextId("Medagliere");
                    }

                    Console.Write("Podio (Oro, Argento, Bronzo): ");
                    nuovaMedaglia.Podio = Console.ReadLine();

                    Console.Write("ID evento (invio per lasciare vuoto): ");
                    var eventoInput = Console.ReadLine();
                    if (!string.IsNullOrEmpty(eventoInput) && int.TryParse(eventoInput, out int eventoIdCreate))
                    {
                        var eventoEsistente = eventoDAO.FindRecord(eventoIdCreate) as Evento;
                        if (eventoEsistente == null)
                        {
                            Console.WriteLine($"\nEvento con ID {eventoIdCreate} non trovato. Vuoi creare un nuovo Evento con questo ID? (sì/no)");
                            if (Console.ReadLine()?.Equals("sì", StringComparison.OrdinalIgnoreCase) == true)
                            {
                                var newEvento = new Evento { Id = eventoIdCreate };
                                eventoDAO.CreateRecord(newEvento);
                                nuovaMedaglia.Evento = newEvento;
                            }
                        }
                        else
                        {
                            nuovaMedaglia.Evento = eventoEsistente;
                        }
                    }

                    Console.Write("ID gara (invio per lasciare vuoto): ");
                    var garaInput = Console.ReadLine();
                    if (!string.IsNullOrEmpty(garaInput) && int.TryParse(garaInput, out int garaIdCreate))
                    {
                        var garaEsistente = garaDAO.FindRecord(garaIdCreate) as Gara;
                        if (garaEsistente == null)
                        {
                            Console.WriteLine($"\nGara con ID {garaIdCreate} non trovata. Vuoi creare una nuova Gara con questo ID? (sì/no)");
                            if (Console.ReadLine()?.Equals("sì", StringComparison.OrdinalIgnoreCase) == true)
                            {
                                var newGara = new Gara { Id = garaIdCreate };
                                garaDAO.CreateRecord(newGara);
                                nuovaMedaglia.Gara = newGara;
                            }
                        }
                        else
                        {
                            nuovaMedaglia.Gara = garaEsistente;
                        }
                    }

                    Console.Write("ID atleta (invio per lasciare vuoto): ");
                    var atletaIdCreateInput = Console.ReadLine();
                    int? atletaIdCreate = null;
                    if (!string.IsNullOrEmpty(atletaIdCreateInput) && int.TryParse(atletaIdCreateInput, out int idAtletaCreate))
                    {
                        var atletaEsistente = atletaDAO.FindRecord(idAtletaCreate) as Atleta;
                        if (atletaEsistente == null)
                        {
                            Console.WriteLine($"\nAtleta con ID {idAtletaCreate} non trovato. Vuoi creare un nuovo Atleta con questo ID? (sì/no)");
                            if (Console.ReadLine()?.Equals("sì", StringComparison.OrdinalIgnoreCase) == true)
                            {
                                var newAtleta = new Atleta { Id = idAtletaCreate };
                                atletaDAO.CreateRecord(newAtleta);
                                atletaIdCreate = idAtletaCreate;
                            }
                        }
                        else
                        {
                            atletaIdCreate = idAtletaCreate;
                        }
                    }

                    Console.WriteLine("".PadRight(50, '-'));

                    if (medagliaDAO.CreateRecordWithIdAtleta(nuovaMedaglia, atletaIdCreate))
                    {
                        Console.WriteLine("\nNuova medaglia creata con successo.");
                    }
                    else
                    {
                        Console.WriteLine("\nErrore durante la creazione della nuova medaglia.");
                    }
                    Console.WriteLine();
                    break;

                case 19: // Update medal
                    Console.Write("\nInserisci l'ID della medaglia da aggiornare: ");
                    if (int.TryParse(Console.ReadLine(), out int updateMedagliaId))
                    {
                        var medagliaDaAggiornare = medagliaDAO.FindRecord(updateMedagliaId) as Medaglia;
                        if (medagliaDaAggiornare != null)
                        {
                            Console.WriteLine("\nAggiornamento dati medaglia:\n");
                            Console.WriteLine("".PadRight(50, '-'));

                            Console.Write("Nuovo podio (invio per mantenere invariato): ");
                            var nuovoPodio = Console.ReadLine();
                            if (!string.IsNullOrEmpty(nuovoPodio))
                                medagliaDaAggiornare.Podio = nuovoPodio;

                            Console.Write("Nuovo ID evento (invio per mantenere invariato): ");
                            var eventoIdUpdateInput = Console.ReadLine();
                            if (!string.IsNullOrEmpty(eventoIdUpdateInput) && int.TryParse(eventoIdUpdateInput, out int eventoIdUpdate))
                            {
                                var eventoUpdate = eventoDAO.FindRecord(eventoIdUpdate) as Evento;
                                if (eventoUpdate != null)
                                    medagliaDaAggiornare.Evento = eventoUpdate;
                                else
                                    Console.WriteLine("\nEvento non trovato.");
                            }

                            Console.Write("Nuovo ID gara (invio per mantenere invariato): ");
                            var garaIdUpdateInput = Console.ReadLine();
                            if (!string.IsNullOrEmpty(garaIdUpdateInput) && int.TryParse(garaIdUpdateInput, out int garaIdUpdate))
                            {
                                var garaUpdate = garaDAO.FindRecord(garaIdUpdate) as Gara;
                                if (garaUpdate != null)
                                    medagliaDaAggiornare.Gara = garaUpdate;
                                else
                                    Console.WriteLine("\nGara non trovata.");
                            }

                            Console.Write("Nuovo ID atleta (invio per mantenere invariato): ");
                            var atletaIdUpdateInput = Console.ReadLine();
                            int? atletaIdUpdate = null;
                            if (!string.IsNullOrEmpty(atletaIdUpdateInput) && int.TryParse(atletaIdUpdateInput, out int idAtletaUpdate))
                            {
                                atletaIdUpdate = idAtletaUpdate;
                            }

                            Console.WriteLine("".PadRight(50, '-'));

                            if (medagliaDAO.UpdateRecordWithIdAtleta(medagliaDaAggiornare, atletaIdUpdate))
                            {
                                Console.WriteLine("\nMedaglia aggiornata con successo.");
                            }
                            else
                            {
                                Console.WriteLine("\nErrore durante l'aggiornamento della medaglia.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nMedaglia non trovata.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nID non valido.");
                    }
                    Console.WriteLine();
                    break;

                case 20: // Delete medal
                    Console.Write("\nInserisci l'ID della medaglia da eliminare: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteMedagliaId))
                    {
                        Console.WriteLine("".PadRight(50, '-'));
                        if (medagliaDAO.DeleteRecord(deleteMedagliaId))
                        {
                            Console.WriteLine("\nMedaglia eliminata con successo.");
                        }
                        else
                        {
                            Console.WriteLine("\nErrore durante l'eliminazione della medaglia.");
                        }
                        Console.WriteLine("".PadRight(50, '-'));
                    }
                    else
                    {
                        Console.WriteLine("\nID non valido.");
                    }
                    Console.WriteLine();
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
