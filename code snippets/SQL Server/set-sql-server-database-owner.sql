--https://www.mssqltips.com/sqlservertip/2477/property-owner-is-not-available-for-database-ssms-error/
--Set SQL Server Database Owner from SQL script

sp_helpdb ExcelInsight

use ExcelInsight
go
sp_changedbowner 'sa'
go