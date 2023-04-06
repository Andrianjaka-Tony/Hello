delete from abonnement;
delete from bouquetDetails;
delete from bouquet;
delete from chaine;
delete from client;
delete from region;
go


insert into region
(id_region, nom_region, frequence_region)
values
('reg00001', 'Analamanga', 65),
('reg00002', 'Itasy', 74);
go

insert into client
(id_client, nom_client, id_region)
values
('cli00001', 'Itachi Uchiha', (select id_region from region where nom_region = 'Analamanga')),
('cli00002', 'Sasuke Uchiha', (select id_region from region where nom_region = 'Itasy'));