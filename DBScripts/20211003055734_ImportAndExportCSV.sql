IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'CustomerInfo')
  BEGIN
    Create Database CustomerInfo
  END
-------------------------------------------------------------------------

-------------------------------------------------------------------------
    GO
       Use CustomerInfo
    GO
-------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sysobjects WHERE name='Customers' and xtype='U')
BEGIN
   Drop table Customers
END
-------------------------------------------------------------------------

Create Table Customers
(
Id int identity(1,1),
CustomerName varchar(250),
City varchar(250),
State varchar(250),
Country varchar(250)
)


GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('dbo.GetCSVData'))
   exec('CREATE PROCEDURE [dbo].[GetCSVData] AS BEGIN SET NOCOUNT ON; END')
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('dbo.CSVDataTotalCount'))
   exec('CREATE PROCEDURE [dbo].[CSVDataTotalCount] AS BEGIN SET NOCOUNT ON; END')
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('dbo.CSVDataExport'))
   exec('CREATE PROCEDURE [dbo].[CSVDataExport] AS BEGIN SET NOCOUNT ON; END')
GO
-----------------------------------------------------------------------------------------
Go
Alter Procedure GetCSVData
@Page int = null,    
@PageSize int = null
AS
BEGIN

Declare @Query nvarchar(max)    

if @Page is null    
begin    
select @Page=1    
end    
    
if @PageSize is null    
begin    
select @PageSize=10    
end   
set @Query=N'Select CustomerName,City,State,Country from Customers order by Id OFFSET ((@Page - 1)* @PageSize ) ROWS    
Fetch Next @PageSize ROWS only'


EXEC sp_executesql @Query, N'@Page int,@PageSize int', 
@Page=@Page,@PageSize=@PageSize    
END
--------------------------------------------------------------------------------------
GO
Alter Procedure CSVDataTotalCount
AS
BEGIN
select count(*) from Customers
END
--------------------------------------------------------------------------------------
GO
Alter Procedure CSVDataExport
AS
BEGIN
Select CustomerName,City,State,Country from Customers
END
---------------------------------------------------------------------------------------