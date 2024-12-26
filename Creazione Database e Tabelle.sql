-- Create the database
CREATE DATABASE Generation;
GO

-- Use the new database
USE Generation;
GO

-- Create the Atleti table
CREATE TABLE Atleti (
    id INT PRIMARY KEY,
    nome VARCHAR(50),
    cognome VARCHAR(50),
    dob DATE,
    nazione VARCHAR(50)
);
GO

-- Create the Eventi table
CREATE TABLE Eventi (
    id INT PRIMARY KEY,
    tipo VARCHAR(50),
    luogo VARCHAR(50),
    anno INT
);
GO

-- Create the Gare table
CREATE TABLE Gare (
    id INT PRIMARY KEY,
    nome VARCHAR(50),
    categoria VARCHAR(50),
    indoor BIT,
    squadra BIT
);
GO

-- Create the Medagliere table with foreign key constraints
CREATE TABLE Medagliere (
    id INT PRIMARY KEY,
    idAtleta INT,
    idGara INT,
    idEvento INT,
    medaglia VARCHAR(20),
    FOREIGN KEY (idAtleta) REFERENCES Atleti(id)
        ON UPDATE CASCADE
        ON DELETE SET NULL,
    FOREIGN KEY (idGara) REFERENCES Gare(id)
        ON UPDATE CASCADE
        ON DELETE SET NULL,
    FOREIGN KEY (idEvento) REFERENCES Eventi(id)
        ON UPDATE CASCADE
        ON DELETE SET NULL
);
GO

