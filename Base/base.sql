create database canal;
go
use canal;
go

-- creation de la table region
CREATE TABLE region (
  id_region VARCHAR(10) PRIMARY KEY,
  nom_region VARCHAR(30) NOT NULL,
  frequence_region FLOAT NOT NULL DEFAULT 0
);
go

-- creation de la table client
CREATE TABLE client (
  id_client VARCHAR(10) PRIMARY KEY,
  nom_client VARCHAR(30) NOT NULL,
  id_region VARCHAR(10) REFERENCES region(id_region) NOT NULL
);
go

-- creation de la table chaine
CREATE TABLE chaine (
  id_chaine VARCHAR(10) PRIMARY KEY,
  nom_chaine VARCHAR(30) NOT NULL,
  prix_chaine FLOAT NOT NULL DEFAULT 0,
  frequence_chaine FLOAT NOT NULL DEFAULT 0
);
go

-- creation de table bouquet
CREATE TABLE bouquet (
  id_bouquet VARCHAR(10) PRIMARY KEY NOT NULL,
  nom_bouquet VARCHAR(30) NOT NULL,
  isPerso BIT NOT NULL DEFAULT 0,
  id_client VARCHAR(10) REFERENCES client(id_client),
  remise FLOAT NOT NULL DEFAULT 0
);
go

-- creation de la table bouquetDetails qui fait reference a la liste des chaines d'un bouquet
CREATE TABLE bouquetDetails (
  id_bouquet VARCHAR(10) REFERENCES bouquet(id_bouquet),
  id_chaine VARCHAR(10) REFERENCES chaine(id_chaine)
);
go

-- creation de la table abonnement qui stocke les abonnements
CREATE TABLE abonnement (
  id_abonnement VARCHAR(10) PRIMARY KEY,
  id_client VARCHAR(10) REFERENCES client(id_client),
  id_bouquet VARCHAR(10) REFERENCES bouquet(id_bouquet),
  prix_bouquet FLOAT NOT NULL, 
  date_abonnement DATE NOT NULL,
  debut_abonnement DATE NOT NULL,
  fin_abonnement DATE NOT NULL
);
go

-- creation de la id_client qui est une sequence pour les clients
CREATE SEQUENCE id_client
START WITH 1;
go
-- creation de la id_region qui est une sequence pour les regions
CREATE SEQUENCE id_region
START WITH 1;
go
-- creation de la id_bouquet qui est une sequence pour les bouquets
CREATE SEQUENCE id_bouquet
START WITH 1;
go
-- creation de la id_chaine qui est une sequence pour les chaines
CREATE SEQUENCE id_chaine
START WITH 1;
go
-- creation de la sequence pour les abonnements
CREATE SEQUENCE id_abonnement
START WITH 1;
go

INSERT INTO REGION 
(id_region, nom_region, frequence_region)
VALUES
('reg00001', 'Andoharanofotsy', 30),
('reg00002', 'Analakely', 35);
go

INSERT INTO chaine
(id_chaine, nom_chaine, prix_chaine, frequence_chaine)
VALUES
('ch00001', 'Canal + Sport', '1500', 75),
('ch00002', 'M6', '1000', 85),
('ch00003', 'W9', '1300', 80),
('ch00004', 'Game One', '2000', 90),
('ch00005', 'MTV Hits', '2300', 91);
go


INSERT INTO bouquet 
(id_bouquet, nom_bouquet, remise)
VALUES
('bou00001', 'Bouquet simple', 20),
('bou00002', 'Bouquet double', 30);
go

INSERT INTO bouquetDetails 
(id_bouquet, id_chaine)
VALUES
('bou00001', 'ch00004'),
('bou00001', 'ch00005'),
('bou00002', 'ch00004'),
('bou00002', 'ch00005'),
('bou00002', 'ch00003'),
('bou00002', 'ch00002');
go

INSERT INTO client
(id_client, nom_client, id_region)
VALUES
('cli00001', 'Rakoto', 'reg00002'),
('cli00002', 'Rasoa', 'reg00002');
go


insert into bouquet VALUES
('bou00003', 'Bouquet de CM', 1, 'cli00001', 0);
INSERT INTO bouquetDetails 
(id_bouquet, id_chaine)
VALUES
('bou00003', 'ch00005');
go