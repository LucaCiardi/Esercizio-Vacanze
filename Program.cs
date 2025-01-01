using System;
using Utility;

var databasePath = @"C:\USERS\NOIVO\DOCUMENTS\GENERATION.MDF";
var database = new Database(databasePath);
var menuManagement = MenuManagement.GetInstance(database);
menuManagement.StartMenu();
