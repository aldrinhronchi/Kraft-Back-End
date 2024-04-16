/****** DataBase ******/
CREATE DATABASE KSystemDB
GO
USE KSystemDB
GO

/****** Erros ******/

CREATE TABLE ErrosLog(
	ID int IDENTITY(1,1) NOT NULL,
	Aplicacao nvarchar(255) NULL,
	Nome nvarchar(255) NULL,
	Data datetime2(7) NOT NULL DEFAULT GETDATE(),
	Tipo nvarchar(255) NULL,
	Mensagem nvarchar(255) NULL,
	Arquivo nvarchar(255) NULL,
	Stack text NULL,
 CONSTRAINT PK_ErrosLog_ID PRIMARY KEY (ID)
);
GO
CREATE TABLE OcorrenciaLog(
	ID int IDENTITY(1,1) NOT NULL,
	IDErro int NOT NULL,
	Aplicacao nvarchar(255) NULL,
	Data datetime2(7) NOT NULL DEFAULT GETDATE(),
 CONSTRAINT PK_OcorrenciaLog_ID PRIMARY KEY (ID)
);
GO

/****** Modulos ******/
CREATE TABLE Modulos(
	ID int IDENTITY(1,1) NOT NULL,
	Nome nvarchar(100) NOT NULL,
	Icone nvarchar(100) NOT NULL,
	Ativo bit NOT NULL DEFAULT 1,
 CONSTRAINT PK_Modulos_ID PRIMARY KEY (ID)
);
GO

/****** Cargos ******/
CREATE TABLE Cargos(
	ID int IDENTITY(1,1) NOT NULL,
	Nome nvarchar(100) NOT NULL,
	Ativo bit NOT NULL DEFAULT 1,
	DataCriado datetime2(7) NOT NULL DEFAULT GETDATE(),
	DataAlterado datetime2(7) NULL,
	UsuarioCriado nvarchar(255) NULL DEFAULT 'SISTEMA',
	UsuarioAlterado nvarchar(255) NULL,
 	CONSTRAINT PK_Cargos_ID PRIMARY KEY (ID)
);
GO

/****** Paginas ******/
CREATE TABLE Paginas (
	ID int IDENTITY(1,1) NOT NULL,
	IDModulo int NOT NULL,
	Nome nvarchar(100) NOT NULL,
	Url nvarchar(100) NULL,
	Ordem int NOT NULL,
	Ativo bit NOT NULL DEFAULT 1,
 CONSTRAINT PK_Paginas_ID PRIMARY KEY (ID)
);
GO

/******  Permissoes ******/
CREATE TABLE Permissoes (
	ID int IDENTITY(1,1) NOT NULL,
	IDCargo int NOT NULL,
	IDModulo int NOT NULL,
	IDPagina int NOT NULL,
	Criar bit NOT NULL DEFAULT 0,
	Revisar bit NOT NULL DEFAULT 0,
	Editar bit NOT NULL DEFAULT 0,
	Deletar bit NOT NULL DEFAULT 0,
 CONSTRAINT PK_Permissoes_ID PRIMARY KEY (ID)
);
GO

/****** Usuarios ******/
CREATE TABLE Usuarios(
	ID int IDENTITY(1,1) NOT NULL,
	Nome nvarchar(100) NOT NULL,
	Login nvarchar(30) NOT NULL,
	Senha nvarchar(200) NOT NULL,
	IDCargo int NOT NULL,
	Email nvarchar(200) NULL,
	Ativo bit NOT NULL DEFAULT 1,
	DataCriado datetime2(7) NOT NULL DEFAULT GETDATE(),
	DataAlterado datetime2(7) NULL,
	UsuarioCriado nvarchar(255) NULL DEFAULT 'SISTEMA',
	UsuarioAlterado nvarchar(255) NULL,
 CONSTRAINT PK_Usuarios_ID PRIMARY KEY (ID)
);
GO



