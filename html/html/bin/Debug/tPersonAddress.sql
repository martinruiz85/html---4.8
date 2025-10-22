--INICIA
--@Item:PID
IF COL_LENGTH('tPersonAddress','PID') IS NULL
	ALTER TABLE [tPersonAddress] ADD [PID] [int] NOT NULL
ELSE
	PRINT 'LA COLUMNA YA EXISTE: PID'
--@Item:HomeAddress
IF COL_LENGTH('tPersonAddress','HomeAddress') IS NULL
	ALTER TABLE [tPersonAddress] ADD [HomeAddress] [nvarchar](255) NULL
ELSE
	PRINT 'LA COLUMNA YA EXISTE: HomeAddress'
--@Item:HomeCity
IF COL_LENGTH('tPersonAddress','HomeCity') IS NULL
	ALTER TABLE [tPersonAddress] ADD [HomeCity] [nvarchar](50) NULL
ELSE
	PRINT 'LA COLUMNA YA EXISTE: HomeCity'
--@Item:HomeStateID
IF COL_LENGTH('tPersonAddress','HomeStateID') IS NULL
	ALTER TABLE [tPersonAddress] ADD [HomeStateID] [int] NULL
ELSE
	PRINT 'LA COLUMNA YA EXISTE: HomeStateID'
--@Item:HomeCtryID
IF COL_LENGTH('tPersonAddress','HomeCtryID') IS NULL
	ALTER TABLE [tPersonAddress] ADD [HomeCtryID] [int] NULL
ELSE
	PRINT 'LA COLUMNA YA EXISTE: HomeCtryID'
--@Item:HomePostalCode
IF COL_LENGTH('tPersonAddress','HomePostalCode') IS NULL
	ALTER TABLE [tPersonAddress] ADD [HomePostalCode] [nvarchar](20) NULL
ELSE
	PRINT 'LA COLUMNA YA EXISTE: HomePostalCode'
--@Item:HomeAddress2
IF COL_LENGTH('tPersonAddress','HomeAddress2') IS NULL
	ALTER TABLE [tPersonAddress] ADD [HomeAddress2] [nvarchar](255) NULL
ELSE
	PRINT 'LA COLUMNA YA EXISTE: HomeAddress2'
--@Item:HomeCity2
IF COL_LENGTH('tPersonAddress','HomeCity2') IS NULL
	ALTER TABLE [tPersonAddress] ADD [HomeCity2] [nvarchar](50) NULL
ELSE
	PRINT 'LA COLUMNA YA EXISTE: HomeCity2'
--@Item:HomeStateID2
IF COL_LENGTH('tPersonAddress','HomeStateID2') IS NULL
	ALTER TABLE [tPersonAddress] ADD [HomeStateID2] [int] NULL
ELSE
	PRINT 'LA COLUMNA YA EXISTE: HomeStateID2'
--@Item:HomeCtryID2
IF COL_LENGTH('tPersonAddress','HomeCtryID2') IS NULL
	ALTER TABLE [tPersonAddress] ADD [HomeCtryID2] [int] NULL
ELSE
	PRINT 'LA COLUMNA YA EXISTE: HomeCtryID2'
--@Item:HomePostalCode2
IF COL_LENGTH('tPersonAddress','HomePostalCode2') IS NULL
	ALTER TABLE [tPersonAddress] ADD [HomePostalCode2] [nvarchar](20) NULL
ELSE
	PRINT 'LA COLUMNA YA EXISTE: HomePostalCode2'
--@Item:EMPHCITY
IF COL_LENGTH('tPersonAddress','EMPHCITY') IS NULL
	ALTER TABLE [tPersonAddress] ADD [EMPHCITY] [varchar](30) NULL
ELSE
	PRINT 'LA COLUMNA YA EXISTE: EMPHCITY'
--@Item:HXIGCOLONIA
IF COL_LENGTH('tPersonAddress','HXIGCOLONIA') IS NULL
	ALTER TABLE [tPersonAddress] ADD [HXIGCOLONIA] [varchar](150) NULL
ELSE
	PRINT 'LA COLUMNA YA EXISTE: HXIGCOLONIA'
--@Item:XIGCOLONIAID
IF COL_LENGTH('tPersonAddress','XIGCOLONIAID') IS NULL
	ALTER TABLE [tPersonAddress] ADD [XIGCOLONIAID] [int] NULL
ELSE
	PRINT 'LA COLUMNA YA EXISTE: XIGCOLONIAID'
--@Item:MUNICIPIOID
IF COL_LENGTH('tPersonAddress','MUNICIPIOID') IS NULL
	ALTER TABLE [tPersonAddress] ADD [MUNICIPIOID] [int] NULL
ELSE
	PRINT 'LA COLUMNA YA EXISTE: MUNICIPIOID'
--EXITO
