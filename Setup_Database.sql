
/* Run this in SQL Server to prepare the database 
*/
CREATE DATABASE InternalStorage;
GO
USE InternalStorage;
GO
CREATE TABLE RepositoryNotes (
    Id INT PRIMARY KEY IDENTITY,
    Header NVARCHAR(100),
    Body NVARCHAR(MAX)
);
GO
INSERT INTO RepositoryNotes (Header, Body) VALUES ('Welcome', 'This is a demo repository.');
