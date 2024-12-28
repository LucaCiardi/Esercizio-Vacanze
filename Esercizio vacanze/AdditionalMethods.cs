using System;
using System.Collections.Generic;
using DAOs;
using Utility;
using Microsoft.Data.SqlClient;

public class AdditionalMethods
{
    private static AdditionalMethods? _instance;
    private static readonly object _lock = new();
    private readonly IDatabase _database;

    private AdditionalMethods(IDatabase database)
    {
        _database = database;
    }

    public static AdditionalMethods GetInstance(IDatabase database)
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                _instance ??= new AdditionalMethods(database);
            }
        }
        return _instance;
    }

    public void DisplayMedals(int athleteId)
    {
        var query = @"
            SELECT 
                CONCAT(a.Nome, ' ', a.Cognome) AS Atleta,
                m.Podio AS Medaglia,
                g.Nome AS Gara,
                e.Tipo AS Evento,
                e.Luogo AS Luogo,
                e.Anno AS Anno
            FROM 
                Medagliere m
            JOIN 
                Atleti a ON m.IdAtleta = a.Id
            JOIN 
                Gare g ON m.IdGara = g.Id
            JOIN 
                Eventi e ON m.IdEvento = e.Id
            WHERE 
                a.Id = @athleteId
            ORDER BY 
                CASE 
                    WHEN m.Podio = 'Oro' THEN 1
                    WHEN m.Podio = 'Argento' THEN 2
                    WHEN m.Podio = 'Bronzo' THEN 3
                END,
                e.Anno ASC;
        ";

        using var command = new SqlCommand(query);
        command.Parameters.AddWithValue("@athleteId", athleteId);

        var results = _database.ReadDb(command);

        if (results == null || !results.Any())
        {
            Console.WriteLine("No medals found for the athlete.");
            return;
        }

        string lastMedalType = "";
        Console.WriteLine($"{results[0]["Atleta"]}");
        foreach (var medal in results)
        {
            if (lastMedalType != medal["Medaglia"].ToString())
            {
                lastMedalType = medal["Medaglia"].ToString();
                Console.WriteLine($"  - {lastMedalType}");
            }
            Console.WriteLine($"    - {medal["Anno"]}: {medal["Gara"]}, {medal["Evento"]} {medal["Luogo"]} {medal["Anno"]}");
        }
    }
}
