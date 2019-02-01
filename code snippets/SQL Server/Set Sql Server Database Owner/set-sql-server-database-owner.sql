--https://www.mssqltips.com/sqlservertip/2477/property-owner-is-not-available-for-database-ssms-error/
--Set SQL Server Database Owner from SQL script

--To check the current owner:
sp_helpdb ExcelInsight

--To change the db owner:
use ExcelInsight
go
sp_changedbowner 'sa'
go