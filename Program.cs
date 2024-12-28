using Utility;

var database = new Database("Generation");
var menuManagement = new MenuManagement(database);
menuManagement.StartMenu();