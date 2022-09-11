USE [Formacion_City]
GO
/****** Object:  StoredProcedure [dbo].[TOY_CCW_RecuperaImagenDNI_20220729]    Script Date: 29/07/2022 15:49:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mark J Sánchez 
-- Create date: 27/07/2022
-- Description:	Obtiene la imagen DNI del cliente (reverso y anverso) insertados 
-- con el GO (en la creación o modificación de clientes).
-- OBSOLETO: En desuso desde el 29/07/2022
-- =============================================

ALTER PROCEDURE [dbo].[TOY_CCW_RecuperaImagenDNI_20220729]
	@CodigoUsuario numeric(10, 0)
AS
--	DECLARE @CodigoUsuario numeric(10, 0)
--	SET @CodigoUsuario = 42221085
--  EXEC TOY_CCW_RecuperaImagenClienteDobleCara 42221085, 'dni'
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	EXEC dbo.[TOY_CCW_RecuperaImagenClienteDobleCara] @CodigoUsuario, 'dni'
END