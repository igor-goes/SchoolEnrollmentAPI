IF DB_ID('SchoolEnrollment') IS NULL CREATE DATABASE SchoolEnrollment;
GO
USE SchoolEnrollment;
GO
CREATE TABLE Aluno (
  Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Aluno PRIMARY KEY,
  Nome NVARCHAR(150) NOT NULL,
  Email NVARCHAR(150) NOT NULL,
  DataNascimento DATE NOT NULL,
  Ativo BIT NOT NULL CONSTRAINT DF_Aluno_Ativo DEFAULT 1
);
INSERT INTO Aluno (Nome, Email, DataNascimento, Ativo) VALUES
 (N'Ana Souza', N'ana.souza@escola.com', '2008-04-12', 1), (N'Bruno Lima', N'bruno.lima@escola.com', '2007-09-21', 1), (N'Carlos Silva', N'carlos.silva@escola.com', '2008-01-08', 1);
