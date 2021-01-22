INSERT INTO [Wms.ProductionLine].[dbo].[Item]
([Id],[Description],[Code],[ItemType])

select monolithItem.Id, monolithItem.[Description], monolithItem.Code, monolithItem.Tipo
from [WMS].[management].[Item] as monolithItem
left join [Wms.ProductionLine].[dbo].[Item] serviceItem on monolithItem.Id=serviceItem.Id
where serviceItem.Id is null