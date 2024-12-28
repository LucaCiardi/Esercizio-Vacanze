using System;
using DAOs;
using Utility;
using Entities;

public class MenuManagement
{
    private readonly IDatabase _database;

    public MenuManagement(IDatabase database)
    {
        _database = database;
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
                    // Call method to get all eventi
                    break;
                case 7:
                    // Call method to find an evento by ID
                    break;
                case 8:
                    // Call method to create a new evento
                    break;
                case 9:
                    // Call method to update an existing evento
                    break;
                case 10:
                    // Call method to delete an evento
                    break;
                case 11:
                    // Call method to get all gare
                    break;
                case 12:
                    // Call method to find a gara by ID
                    break;
                case 13:
                    // Call method to create a new gara
                    break;
                case 14:
                    // Call method to update an existing gara
                    break;
                case 15:
                    // Call method to delete a gara
                    break;
                case 16:
                    // Call method to get all medaglie
                    break;
                case 17:
                    // Call method to find a medaglia by ID
                    break;
                case 18:
                    // Call method to create a new medaglia
                    break;
                case 19:
                    // Call method to update an existing medaglia
                    break;
                case 20:
                    // Call method to delete a medaglia
                    break;
                case 21:
                    // Call method to display medals for an atleta
                    break;
                case 22:
                    // Call method to display events for an evento ID
                    break;
                case 23:
                    // Call method to display medal count for an atleta
                    break;
                case 24:
                    // Call method to display atleti by nation
                    break;
                case 25:
                    // Call method to display oldest gold medalists
                    break;
                case 26:
                    // Call method to display team sport medals
                    break;
                case 27:
                    // Call method to display most won category
                    break;
                case 28:
                    // Call method to update data through .txt files
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
