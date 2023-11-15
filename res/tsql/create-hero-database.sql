CREATE DATABASE CrudDaJustica
GO

USE CrudDaJustica
GO

CREATE TABLE Hero (
	Id uniqueidentifier not null primary key,
	Alias varchar(30) not null,
	Debut DATE not null,
	FirstName varchar(15) not null,
	LastName varchar(15) not null,
);