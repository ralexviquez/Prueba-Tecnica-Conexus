
create procedure Ps_AddProducto @Nombre varchar(50), @CategoriaId int
as
DECLARE @table table (id int)

insert into Productos (Nombre, categoriaId)
OUTPUT Inserted.Id into @table
values (@Nombre, @CategoriaId);


SELECT id from @table
go


create procedure Ps_AddProductosClientes @ClienteId int, @ProductoId int
as

DECLARE @table table (id int)

insert into ProductosClientes(clienteId, productoId )
OUTPUT Inserted.Id into @table
values (@ClienteId, @ProductoId);


SELECT id from @table

go


