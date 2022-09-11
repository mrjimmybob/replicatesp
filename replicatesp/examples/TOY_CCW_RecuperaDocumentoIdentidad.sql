USE [Formacion_City]
GO
/****** Object:  StoredProcedure [dbo].[TOY_CCW_RecuperaDocumentoIdentidad]    Script Date: 29/07/2022 15:38:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mark J Sánchez 
-- Create date: 28/07/2022
-- Description:	Obtiene los documentos adjuntos de identificación (DNI, Pasaporte, NIE, CIF) insertados 
-- con el GO (en la creación o modificación de clientes) como documento escaneado (docid_...).
-- Si existe el 'docid_...' debe devolver este, pero sino, que devuelva un 'dni_...', o por defecto un 'anverso...'
-- =============================================

ALTER PROCEDURE [dbo].[TOY_CCW_RecuperaDocumentoIdentidad]
	@CodigoUsuario numeric(10, 0)
AS
-- DECLARE @CodigoUsuario numeric(10, 0)
-- SET @CodigoUsuario = 42221085
-- EXEC [dbo].[TOY_CCW_RecuperaDocumentoIdentidad] 42221085
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT TOP 1 [DocumName] AS [Name], [DocumContent] AS [Data], [DocumFecha] AS [UploadDate], [DocumOwner] AS [Owner], 
		[DocumSize] AS [Size], (SELECT LOWER (REVERSE(LEFT(REVERSE([DocumName]), CHARINDEX('.',REVERSE([DocumName]))-1)))) AS [FileExt]
	FROM dbo.tyDocumLink 
	WHERE EMP  = '1' AND Tabla = 'TGCLIENTE'
		AND ContadorValor = @CodigoUsuario
		AND (DocumName LIKE 'docid_%'
			OR DocumName LIKE 'dni_anverso%'
			OR DocumName LIKE 'anverso%')
	ORDER BY [DocumName] DESC
END
