USE [WMS.ProductionLine]

INSERT INTO [dbo].[User] (Id, [Login], [Name], WarehouseId)
select Id, [Login], [Name], WarehouseId 
from WMS.management.[User]
	  
GO;

INSERT INTO [dbo].[Address] ([Id],[Code],[Description],[Enabled],[Sequence],[WarehouseId])
select wmsLocation.[Id],
       wmsLocation.[Code],
       wmsLocation.[Description],
       wmsLocation.[Enabled],
       wmsLocation.[PickPutFlow],
       wmsLocation.[WarehouseId]
from [WMS].management.[Location] wmsLocation
