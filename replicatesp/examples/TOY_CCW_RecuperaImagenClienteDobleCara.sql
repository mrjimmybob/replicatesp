USE [Formacion_City]
GO
 Object  StoredProcedure [dbo].[TOY_CCW_RecuperaImagenClienteDobleCara_20220729]    Script Date 29072022 154743 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author		Mark J Sánchez 
-- Create date 27072022
-- Description	Obtiene los documentos adjuntos de identificación (DNI, Pasaporte, NIE, CIF) insertados 
-- con el GO (en la creación o modificación de clientes) como imagenes.
-- OBSOLETO: En desuso desde el 29/07/2022
-- =============================================

ALTER PROCEDURE [dbo].[TOY_CCW_RecuperaImagenClienteDobleCara_20220729]
	@CodigoUsuario numeric(10, 0),
	@Prefix varchar(20)
AS
--	DECLARE @CodigoUsuario numeric(10, 0),
--			@Prefix varchar(20)
--	SET @CodigoUsuario = 42221085
--	SET @Prefix = 'dni'
--  EXEC [TOY_CCW_RecuperaImagenClienteDobleCara] 42221085, 'dni'
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    	
	SELECT TOP 2 [DocumName] AS [Name], [DocumContent] AS [Image], [DocumFecha] AS [UploadDate], [DocumOwner] AS [Owner], [DocumSize] AS [Size]
	FROM dbo.tyDocumLink 
	WHERE EMP  = '1' AND Tabla = 'TGCLIENTE'
		AND ContadorValor = @CodigoUsuario
		AND (DocumName LIKE @Prefix + '_reverso%' 
			OR DocumName LIKE @Prefix + '_anverso%')
	ORDER BY DocumFecha DESC, Name
END
