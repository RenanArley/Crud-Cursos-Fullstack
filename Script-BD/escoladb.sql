create database escoladb

use escoladb

create table curso (
	Id int identity(1,1),
	Nome varchar( 150) not null,
	CHECK(LEN(Nome) BETWEEN 3 and 150),
	CargaHoraria int not null,
	Check(CargaHoraria >0),
	Valor decimal(10,2) not null,
	CHECK (Valor >= 0),
	DataInicio DateTime not null,
	Online Bit not null,
	Ativo Bit not null,
	constraint pk_curso primary key (id)
);

INSERT INTO curso (Nome, CargaHoraria, Valor, DataInicio, Online, Ativo)
VALUES
('C# e Programação Orientada a Objetos', 40, 399.90, '2026-09-10T19:00:00', 0, 1),
('Desenvolvimento Web com ASP.NET', 60, 549.90, '2026-09-15T19:00:00', 0, 1),
('Angular para Frontend', 40, 429.90, '2026-09-20T19:00:00', 1, 1),
('SQL Server e ADO.NET', 32, 299.90, '2026-09-25T19:00:00', 0, 1),
('Git e GitHub', 20, 0.00, '2026-10-01T19:00:00', 1, 1),
('Fundamentos de Algoritmos', 50, 199.90, '2026-08-20T19:00:00', 0, 0),
('HTML e CSS', 30, 149.90, '2026-08-15T19:00:00', 1, 0),
('JavaScript Essencial', 40, 249.90, '2026-08-10T19:00:00', 1, 0),
('APIs REST', 36, 349.90, '2026-09-30T19:00:00', 0, 1),
('Testes de Software', 24, 219.90, '2026-10-05T19:00:00', 1, 0);
	