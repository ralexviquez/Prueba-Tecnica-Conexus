create procedure Ps_AddProducto @Nombre varchar(50), @CategoriaId int
as

insert into Productos (Nombre, categoriaId)
OUTPUT Inserted.Id
values (@Nombre, @CategoriaId);

go