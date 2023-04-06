-- vue pour lier les bouquets et les chaines
CREATE VIEW bouquetChaine AS
SELECT 
  b.*, c.*
FROM
  bouquetDetails as bd
  JOIN
    bouquet as b on bd.id_bouquet = b.id_bouquet
  JOIN
    chaine as c on bd.id_chaine = c.id_chaine;
go

-- vue pour calculer le prix d'un bouquet sans remise
CREATE VIEW VIEW bouquetPrix AS
SELECT
  SUM(prix_chaine) AS prix, id_bouquet
FROM
  bouquetChaine 
  GROUP BY
    id_bouquet;
go

-- vue pour calculer le prix de bouquet avec la remise
CREATE OR ALTER VIEW prixBouquetAvecRemise AS
SELECT
  prix - (prix * (SELECT remise from bouquet where id_bouquet = bp.id_bouquet) / 100) as prix, id_bouquet, nom_bouquet
FROM
  bouquetPrix as bp;
go

-- vue pour reccuperer la frequence minimale des chaine dans un bouquet
CREATE VIEW FrequenceMinimumChaineBouquet AS
SELECT
  MIN(frequence_chaine) as frequence, id_bouquet
FROM
  bouquetChaine 
  GROUP BY
    id_bouquet;
go

-- vue pour avoir la liste des chaines qui sont disponibles dans une region
CREATE VIEW chaineDisposParRegion AS
SELECT
  c.*, r.*
FROM 
  chaine as c 
  JOIN
    region as r on c.frequence_chaine >= r.frequence_region;
go

-- vue pour reccuperer la liste des bouquets dans une region
CREATE VIEW bouquetDisposParRegion AS
SELECT 
  b.*, r.*
FROM 
  FrequenceMinimumChaineBouquet as fmcb
  JOIN  
    bouquet as b on fmcb.id_bouquet = b.id_bouquet
  JOIN
    region as r on fmcb.frequence >= r.frequence_region;
go

-- vue pour lier les clients avec leurs regions
CREATE VIEW clientRegion AS
SELECT
  c.*, r.nom_region
FROM
  client as c
  JOIN
    region as r on c.id_region = r.id_region;
go

-- vue pour avoir tous les abonnements d'un client
CREATE VIEW abonnementClient AS
SELECT
  c.*, b.id_bouquet, b.nom_bouquet, b.isPerso, b.remise, a.id_abonnement, a.prix_bouquet, a.date_abonnement, a.debut_abonnement, a.fin_abonnement
FROM
  abonnement AS a
  JOIN
    client AS c on a.id_client = c.id_client
  JOIN
    bouquet AS b on a.id_bouquet = b.id_bouquet;
go

-- creation d'une vue pour lier les bouquets avec leurs prix
CREATE VIEW bouquetInfos AS
SELECT 
  b.*, bp.prix as prix, pbar.prix as prixAR
FROM 
  bouquet as b
  JOIN
    bouquetPrix as bp on b.id_bouquet = bp.id_bouquet
  JOIN
    prixBouquetAvecRemise as pbar on b.id_bouquet = pbar.id_bouquet
go