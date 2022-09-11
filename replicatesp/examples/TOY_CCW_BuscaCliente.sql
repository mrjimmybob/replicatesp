USE [Formacion_City]
GO
/****** Object:  StoredProcedure [dbo].[TOY_CCW_BuscaCliente]    Script Date: 01/09/2022 11:50:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--####################################################--
--##       Asistente de Creación de Clientes        ##--
--##  --------------------------------------------  ##--
--##                                                ##--
--## Realiza la búsqueda de un cliente a partir de  ##--
--##           su documento de identidad            ##--
--####################################################--
ALTER PROCEDURE [dbo].[TOY_CCW_BuscaCliente]
	@Usuario varchar(15)
	,@Passwd varchar(15)
	,@CIF varchar(15)
	,@Emp varchar(3)
AS
SET LANGUAGE Español
SET DATEFIRST 1
SET DATEFORMAT dmy
SET NOCOUNT ON

--## Variables del procedimiento
--DECLARE	@CIF varchar(15)

--## Asignaciones de ejemplo
--SET	@CIF = 'X0325561L'

--## Variables de la consulta
DECLARE	@Estado int

--## Inicio de la consulta
EXEC	@Estado = dbo.TOY_CCW_CompruebaUsuario @Codigo = @Usuario, @Passwd = @Passwd, @Emp = @Emp
IF	@Estado = 0
BEGIN
	SELECT	A.Codigo as codigo,
		rtrim(A.CIF) as cif,
		rtrim(A.Nombre) as nombre,
		rtrim(A.Apellido1) as apellido1,
		rtrim(A.Apellido2) as apellido2,
		rtrim(A.DireccionEditada) as direccion,
		rtrim(A.EMailPro) as emailPro,
		rtrim(A.EMail) as email,
		rtrim(A.NumTel1) as telefonoPro,
		isnull(CASE WHEN rtrim(A.NumGSM) = '' THEN rtrim(A.NumTel2) ELSE rtrim(A.NumGSM) END, '') as telefono,
		PerJuridica as perJuridica,
		rtrim(DenominacionComercial) as denominacionComercial
	FROM	dbo.tgCliente A
--	WHERE	isnull(rtrim(A.CIF), '') = @CIF
--  WHERE	REPLACE(isnull(rtrim(A.CIF), '') + '|' + isnull(rtrim(A.ApellidoSearch), '') + '|' + isnull(rtrim(A.EMail), '') + '|' + isnull(rtrim(A.NumTel2), '') + '|' + isnull(rtrim(A.NumGSM), ''), '||', '|') like '%' + @CIF + '%'
	WHERE	REPLACE(isnull(rtrim(A.CIF), '') + '|' + isnull(rtrim(A.ApellidoSearch), '') + '|' + isnull(rtrim(A.EMail), '') + '|' + isnull(rtrim(A.EmailPro), '') + '|' + isnull(rtrim(A.NumTel2), '') + '|' + isnull(rtrim(A.NumGSM), '') + '|' + isnull(rtrim(A.Codigo), ''), '||', '|') like '%' + trim(@CIF) + '%'
	AND	ISNULL(A.Baja, 0) = 0
END
ELSE
BEGIN
	RETURN(2)
END

