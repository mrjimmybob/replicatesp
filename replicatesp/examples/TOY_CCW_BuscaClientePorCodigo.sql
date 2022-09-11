USE [Formacion_City]
GO
/****** Object:  StoredProcedure [dbo].[TOY_CCW_BuscaClientePorCodigo]    Script Date: 01/09/2022 11:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--####################################################--
--##       Asistente de Creación de Clientes        ##--
--##  --------------------------------------------  ##--
--##                                                ##--
--## Realiza la búsqueda de un cliente a partir de  ##--
--##           su código interno de Autonet         ##--
--####################################################--
ALTER PROCEDURE [dbo].[TOY_CCW_BuscaClientePorCodigo]
	@Usuario varchar(15),
	@Passwd varchar(15),
	@Codigo varchar(15),
	@Emp varchar(3)
AS
SET LANGUAGE Español
SET DATEFIRST 1
SET DATEFORMAT dmy
SET NOCOUNT ON

--## Variables del procedimiento
--DECLARE	@CIF varchar(15)

--## Asignaciones de ejemplo
--SET	@Codigo  = 265958
--SET   @Usuario = 'UTN8086'
--SET   @Passwd  = 'UTN8086'
--SET	@Emp     = '1'

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
	WHERE	isnull(rtrim(A.Codigo), '') = @Codigo
	AND	ISNULL(A.Baja, 0) = 0
END
ELSE
BEGIN
	RETURN(2)
END
