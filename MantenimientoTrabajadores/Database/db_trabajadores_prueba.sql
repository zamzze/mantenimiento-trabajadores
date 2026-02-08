/* ============================================================
   BASE DE DATOS: TrabajadoresPrueba
   Proyecto: Mantenimiento de Trabajadores
   ============================================================ */

-- (Opcional) Crear base de datos
-- Si ya existe, este bloque puede omitirse
IF DB_ID('TrabajadoresPrueba') IS NULL
BEGIN
    CREATE DATABASE TrabajadoresPrueba;
END
GO

USE TrabajadoresPrueba;
GO

/* ============================================================
   TABLA: Trabajadores
   ============================================================ */

IF OBJECT_ID('dbo.Trabajadores', 'U') IS NOT NULL
    DROP TABLE dbo.Trabajadores;
GO

CREATE TABLE [dbo].[Trabajadores] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Nombres] NVARCHAR(MAX) NOT NULL,
    [Apellidos] NVARCHAR(MAX) NOT NULL,
    [TipoDocumento] NVARCHAR(MAX) NOT NULL,
    [NumeroDocumento] NVARCHAR(MAX) NOT NULL,
    [Sexo] NVARCHAR(MAX) NOT NULL,
    [FechaNacimiento] DATETIME2(7) NOT NULL,
    [Foto] NVARCHAR(MAX) NULL,
    [Direccion] NVARCHAR(MAX) NULL,
    CONSTRAINT [PK_Trabajadores] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

/* ============================================================
   PROCEDIMIENTO: Listar todos los trabajadores
   ============================================================ */

IF OBJECT_ID('dbo.sp_ListarTrabajadores', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ListarTrabajadores;
GO

CREATE PROCEDURE [dbo].[sp_ListarTrabajadores]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Id,
        Nombres,
        Apellidos,
        TipoDocumento,
        NumeroDocumento,
        Sexo,
        FechaNacimiento,
        Foto,
        Direccion
    FROM Trabajadores;
END;
GO

/* ============================================================
   PROCEDIMIENTO: Listar trabajadores por sexo (filtro opcional)
   ============================================================ */

IF OBJECT_ID('dbo.sp_ListarTrabajadoresPorSexo', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ListarTrabajadoresPorSexo;
GO

CREATE PROCEDURE [dbo].[sp_ListarTrabajadoresPorSexo]
    @Sexo NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Trabajadores
    WHERE (@Sexo IS NULL OR Sexo = @Sexo);
END;
GO
