USE [Formacion_City]
GO
/****** Object:  StoredProcedure [dbo].[TOY_CCW_RecuperaCliente]    Script Date: 01/09/2022 13:22:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--####################################################--
--##       Asistente de Creación de Clientes        ##--
--##  --------------------------------------------  ##--
--##                                                ##--
--##    Devuelve los datos de un cliente para su    ##--
--##         edición (búsqueda por código).         ##--
--##  Características a partir de SQL Server 2005   ##--
--####################################################--
ALTER PROCEDURE [dbo].[TOY_CCW_RecuperaCliente]
	@Usuario varchar(15),
	@Passwd varchar(15),
	@Codigo numeric(10, 0),
	@Emp varchar(3)
AS
SET LANGUAGE Español
SET DATEFIRST 1
SET DATEFORMAT dmy
SET NOCOUNT ON

--## Variables del procedimiento
--DECLARE	@Usuario varchar(15)
--	,@Passwd varchar(15)
--	,@Codigo numeric(10, 0)
--	,@Emp varchar(3)

--## Asignaciones de ejemplo
--SET	@Usuario = 'UTN0753'
--SET	@Passwd = 'UTN0753'
--SET	@Codigo = 15
--SET	@Emp = '1'

--## Variables de la consulta
DECLARE	@Estado int,
	@CIF varchar(15),
	@MisValores varchar(128)

--## Trabajo con variables
SET	@CIF = (SELECT TOP 1 CIF FROM dbo.tgCliente WHERE Codigo = @Codigo)

SELECT	TOP 1
	@MisValores = Reemplazo
FROM	TOY_CCW_Reemplazos
WHERE	Tabla = 'TipoCliente'
AND	Campo = 'autonomo'

--## Inicio de la consulta
EXEC	@Estado = dbo.TOY_CCW_CompruebaUsuario @Codigo = @Usuario, @Passwd = @Passwd, @Emp = @Emp
IF	@Estado = 0
BEGIN
	-- Cliente
	SELECT	TOP 1
		A.Codigo as codigo
		,B.IDTIPO as tipoDocumentoIdentidad
		,rtrim(A.CIF) as cif
		,rtrim(A.Cortesia) as cortesia
		,rtrim(G.TextoImp) as cortesiaImp
		,rtrim(A.Nombre) as nombre
		,rtrim(A.Apellido1) as apellido1
		,rtrim(A.Apellido2) as apellido2
		,rtrim(A.Direccion) as direccion
		,rtrim(A.CPostal) as codigoPostal
		,A.Pobla as poblacion
		,rtrim(H.Descrip) as poblacionDescrip
		,A.Provin as provincia
		,rtrim(I.Descrip) as provinciaDescrip
		,A.Pais as pais
		,A.Sexo as sexo
		,CONVERT(varchar, A.FechaNacim, 103) as fechaNacimiento
		,rtrim(A.LugarNacim) as lugarNacimiento
		--,CONVERT(varchar, CONVERT(datetime, CASE ISDATE(B.IDCADUCIDAD) WHEN 1 THEN CONVERT(datetime, B.IDCADUCIDAD, 111) ELSE Null END), 103) as fCaducidadCIF
		,CONVERT(varchar, CONVERT(datetime, CASE ISDATE(CONVERT(datetime, B.IDCADUCIDAD, 111)) WHEN 1 THEN CONVERT(datetime, B.IDCADUCIDAD, 111) ELSE Null END), 103) as fCaducidadCIF
		,rtrim(A.NumGSM) as numGSM
		,rtrim(A.NumTel2) as numTel2
		,rtrim(A.NumTel1) as numTel1
		,rtrim(A.EMail) as email
		,rtrim(A.EMailPro) as emailPro
		,rtrim(A.Profesion) as profesion
		,rtrim(A.SectorActividad) as sectorActividad
		,rtrim(A.CNAE) as CNAE
		,rtrim(C.Vendedor) as vendedorVN
		,rtrim(C.VendedorVO) as vendedorVO
		,rtrim(C.FuenteContacto1) as fuenteContacto1
		,rtrim(C.FuenteContacto2) as fuenteContacto2
		,rtrim(C.Zona) as zona
		,rtrim(C.Sector) as sector
		,rtrim(A.Permiso) as numeroPermiso
		,rtrim(A.EntregaPermiso) as lugarExpedicionPermiso
		,CONVERT(varchar, A.FechaEntregaPermiso, 103) as fechaExpedicionPermiso
		,CONVERT(varchar, A.FecFinPermiso, 103) as fechaValidezPermiso
		,rtrim(B.PERMISOS) as permisos
		,rtrim(A.Hobby1) as aficion1
		,rtrim(A.SubHobby1) as subaficion1
		,rtrim(A.Hobby2) as aficion2
		,rtrim(A.SubHobby2) as subaficion2
		,rtrim(A.Hobby3) as aficion3
		,rtrim(A.SubHobby3) as subaficion3
		,rtrim(A.Hobby4) as aficion4
		,rtrim(A.SubHobby4) as subaficion4
		,rtrim(A.Hobby5) as aficion5
		,rtrim(A.SubHobby5) as subaficion5
		,rtrim(A.Hobby6) as aficion6
		,rtrim(A.SubHobby6) as subaficion6
		,rtrim(A.Regalo) as tieneHijos
		,A.Hijos as numeroHijos
		,rtrim(B.SEXOH1) as sexoHijo1
		,rtrim(B.EDADH1) as edadHijo1
		,rtrim(B.SEXOH2) as sexoHijo2
		,rtrim(B.EDADH2) as edadHijo2
		,rtrim(B.SEXOH3) as sexoHijo3
		,rtrim(B.EDADH3) as edadHijo3
		,rtrim(B.SEXOH4) as sexoHijo4
		,rtrim(B.EDADH4) as edadHijo4
		,rtrim(B.SEXOH5) as sexoHijo5
		,rtrim(B.EDADH5) as edadHijo5
		,CASE WHEN rtrim(isnull(B.RSFACEBOOK, '0')) in ('0', '2') THEN 'false' else 'true' END as rsFacebook
		,CASE WHEN rtrim(isnull(B.RSINSTAGRAM, '0')) in ('0', '2') THEN 'false' else 'true' END as rsInstagram
		,CASE WHEN rtrim(isnull(B.RSTWITTER, '0')) in ('0', '2') THEN 'false' else 'true' END as rsTwitter
		,CASE WHEN rtrim(isnull(B.RSLINKEDIN, '0')) in ('0', '2') THEN 'false' else 'true' END as rsLinkedin
		,CASE WHEN rtrim(isnull(B.RSTUMBLR, '0')) in ('0', '2') THEN 'false' else 'true' END as rsTumblr
		,CASE WHEN rtrim(isnull(B.RSSNAPCHAT, '0')) in ('0', '2') THEN 'false' else 'true' END as rsSnapchat
		,CASE WHEN rtrim(isnull(B.RSTELEGRAM, '0')) in ('0', '2') THEN 'false' else 'true' END as rsTelegram
		,CASE WHEN rtrim(isnull(B.RSVKONTAKTE, '0')) in ('0', '2') THEN 'false' else 'true' END as rsVkontakte
		,CASE WHEN rtrim(isnull(B.RSPINTEREST, '0')) in ('0', '2') THEN 'false' else 'true' END as rsPinterest
		,rtrim(B.RSOTRAS) as rsOtras
		,rtrim(B.CLUB1) as clubAsociacion1
		,rtrim(B.CLUB2) as clubAsociacion2
		,rtrim(B.CLUB3) as clubAsociacion3
		,CASE D.tieneDocId WHEN 0 THEN 'true' ELSE CASE WHEN isnull(B.IDCADUCIDAD, '') = '' OR CONVERT(datetime, B.IDCADUCIDAD, 111) < GETDATE() THEN 'true' ELSE 'false' END END as pedirDocId
		,E.CialTipoFactura
		,E.CialIVACliente
		,E.CialIVATipo
		,F.Porcen as CialIVAPor
		,E.CialAgrupCliente
		,E.CialCopiasFactur
		,E.CialCopiasFactur
		,E.CialFormaPago
		,E.CialModoPago
		,CASE	
			WHEN isnull(A.Flota, '') in (SELECT Data FROM dbo.TOY_GENERAL_Split(@MisValores, ',')) THEN 1
			ELSE 0
		END as autonomo,
		PerJuridica as perJuridica,
		DenominacionComercial as denominacionComercial,
		DirResto as dirResto,
		FechaCreacionEmp as fechaCreacionEmp
	FROM	dbo.tgCliente A
	LEFT JOIN	(
		SELECT	[CLIENTE], [SEXOH1], [EDADH1], [SEXOH2], [EDADH2], [SEXOH3], [EDADH3], [SEXOH4], [EDADH4], [SEXOH5], [EDADH5], [RSFACEBOOK], [RSINSTAGRAM], [RSTWITTER], [RSLINKEDIN], [RSTUMBLR], [RSSNAPCHAT], [RSTELEGRAM], [RSVKONTAKTE], [RSPINTEREST], [RSOTRAS], [CLUB1], [CLUB2], [CLUB3], [PERMISOS], [IDTIPO], [IDCADUCIDAD]
		FROM	(
			SELECT	B.Referencia, D.Valor
			FROM	dbo.tgAgendaTareaCodigo A
			INNER JOIN	dbo.tyFormularioModeloLinea B
				ON	B.Emp = A.Emp
				AND	B.Codigo = A.FormulCodigo
			CROSS APPLY	(
				SELECT	TOP 1
					*
				FROM	dbo.tyFormularioLink
				WHERE	Tabla = 'TGCLIENTE'
				AND	Emp = A.Emp
				AND	ContadorValor = @Codigo
				AND	FormulCodigo = A.FormulCodigo
				ORDER BY	Linea
				) C
			INNER JOIN	dbo.tyFormularioLinkLinea D
				ON	D.Tabla = C.Tabla
				AND	D.Emp = C.Emp
				AND	D.ContadorValor = C.ContadorValor
				AND	D.Linea = C.Linea
				AND	D.Item = B.Item
			WHERE	A.Emp = @Emp
			AND	A.Codigo = 'CCW_DATOSCLIENTE'
			UNION
			SELECT	'CLIENTE', CAST(@Codigo as varchar(10))
			) A
		PIVOT	(
			MAX(Valor)
			FOR Referencia IN ([CLIENTE], [SEXOH1], [EDADH1], [SEXOH2], [EDADH2], [SEXOH3], [EDADH3], [SEXOH4], [EDADH4], [SEXOH5], [EDADH5], [RSFACEBOOK], [RSINSTAGRAM], [RSTWITTER], [RSLINKEDIN], [RSTUMBLR], [RSSNAPCHAT], [RSTELEGRAM], [RSVKONTAKTE], [RSPINTEREST], [RSOTRAS], [CLUB1], [CLUB2], [CLUB3], [PERMISOS], [IDTIPO], [IDCADUCIDAD])
			) B
		) B
		ON	B.Cliente = A.Codigo
	LEFT JOIN	dbo.tgClienteContact C
		ON	C.Emp = @Emp
		AND	C.Codigo = A.Codigo
	OUTER APPLY	(
		SELECT	isnull(B.res, A.res) * isnull(C.res, A.res) as tieneDocId
		FROM	(
			SELECT 0 AS res
			) A
		OUTER APPLY	(
			SELECT	TOP 1
				1 as res
			FROM	dbo.tyDocumLink A
			WHERE	A.Tabla = 'TGCLIENTE'
			AND	A.Emp = @Emp
			AND	A.ContadorValor = @Codigo
			AND	(
				A.DocumName like 'anverso%'
				AND
				isnumeric(substring(A.DocumName, 8, 8)) = 1
				)
			) B
		OUTER APPLY	(
			SELECT	TOP 1
				1 as res
			FROM	dbo.tyDocumLink A
			WHERE	A.Tabla = 'TGCLIENTE'
			AND	A.Emp = @Emp
			AND	A.ContadorValor = @Codigo
			AND	(
				A.DocumName like 'reverso%'
				AND
				isnumeric(substring(A.DocumName, 8, 8)) = 1
				)
			) C
		) D
	LEFT JOIN	dbo.tgClienteFac E
		ON	E.Emp = @Emp
		AND	E.Codigo = A.Codigo
	OUTER APPLY	(
		SELECT	TOP 1
			Codigo
			,FechaInicio
			,Porcen
		FROM	dbo.tgIVAPor
		WHERE	Codigo = E.CialIVATipo
		ORDER BY	FechaInicio ASC
		) F
	LEFT JOIN	dbo.tgCortesia G
		ON	G.Codigo = A.Cortesia
	LEFT JOIN	dbo.tgPobla H
		ON	H.Id = A.Pobla
		AND	H.IdProvin = A.Provin
		AND	H.Pais = A.pais
	LEFT JOIN	dbo.tgProvin I
		ON	I.Id = A.Provin
		AND	I.Pais = A.Pais
	WHERE	A.Codigo = @Codigo

	-- Vehículos conocidos
	SELECT	rtrim(A.TipoRelacion) as tipoRelacion
		,CONVERT(varchar, A.Fecha, 103) as fecha
		,A.VehiculoMaestro as vehiculoMaestro
		,A.Codigo as codigo
		,rtrim(B.Matric) as matric
		,rtrim(B.Chasis) as chasis
		,B.UltOTKM as km
		,rtrim(X3.Descrip) as categoria
		,rtrim(X0.Descrip) as marca
		,rtrim(X1.Descrip) as modelo
		,rtrim(X2.Descrip) as version
		,rtrim(X4.Descrip) as carroceria
		,rtrim(X5.Descrip) as combustible
		,CONVERT(varchar, B.FechaCompra, 103) as fecCompra
		,CONVERT(varchar, B.FecMatricPrimera, 103) as fecMatricPrimera
		,rtrim(B.Obser) as obser
		,rtrim(B.Marca) as marcaCodigo
	FROM	dbo.tgVehCliente A
	INNER JOIN	dbo.ttVeh B
		ON	B.NumInterno = A.VehiculoMaestro
	OUTER APPLY	(
		SELECT	*
		FROM	dbo.tgVehCliente
		WHERE	Codigo = A.Codigo
		AND	VehiculoMaestro = A.VehiculoMaestro
		AND	Fecha >= A.Fecha
		AND	TipoRelacion = 'X'
		) C
	INNER JOIN	dbo.tgMarca X0
		ON	X0.Marca = B.Marca
	LEFT JOIN	dbo.tgModelo X1
		ON	X1.Marca = B.Marca
		AND	X1.Modelo = B.Modelo
	LEFT JOIN	dbo.tgVersion X2
		ON	X2.Marca = B.Marca
		AND	X2.Modelo = B.Modelo
		AND	X2.Version = B.Version
	LEFT JOIN	dbo.tgCategoria X3
		ON	X3.Categoria = B.Categoria
	LEFT JOIN	dbo.tgCarroceria X4
		ON	X4.Codigo = B.Carroceria
	LEFT JOIN	dbo.tgCombustible X5
		ON	X5.Codigo = B.Combustible
	WHERE	A.Codigo = @Codigo
	AND	A.TipoRelacion != 'X'
	AND	C.TipoRelacion is Null

	-- Otros vehículos
	SELECT	A.Codigo as codigo
		,A.Linea as linea -- El contador del vehículo (TGCLIENTEVEHPROP)
		,rtrim(A.Matric) as matric
		,rtrim(A.Chasis) as chasis
		,A.Km as km
		,rtrim(A.Categoria) as categoria
		,rtrim(A.Marca) as marca
		,rtrim(A.Modelo) as modelo
		,rtrim(A.version) as version
		,rtrim(A.Carroceria) as carroceria
		,rtrim(A.Combustible) as combustible
		,CONVERT(varchar, A.FecCompra, 103) as fecCompra
		,CONVERT(varchar, A.FecMatricPrimera, 103) as fecMatricPrimera
		,A.ImporteValor as importeValor
		,rtrim(A.Observ) as observ
	FROM	dbo.tgClienteVehProp A
	WHERE	A.Codigo = @Codigo

	-- Escritura en tabla log
	INSERT INTO	dbo.TOY_CCW_Log
		(
		Usuario,
		Emp,
		Fecha,
		Accion,
		Codigo,
		CIF,
		Consulta,
		Resultado
		)
	VALUES	(
		@Usuario,
		@Emp,
		GETDATE(),
		'Lectura',
		@Codigo,
		@CIF,
		'EXEC dbo.TOY_CCW_RecuperaCliente @Usuario = ''' + @Usuario + ''', @Passwd = ''' + @Passwd + ''', @Codigo = ' + CAST(@Codigo as varchar(10)) + ', @Emp = ''' + @Emp + '''',
		'Completado'
		)
END
ELSE
BEGIN
	RETURN 2
END

