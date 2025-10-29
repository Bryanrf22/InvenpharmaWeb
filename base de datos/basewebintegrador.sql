


use BDFarmaciaWeb

go



create table Inventario
(
InventarioID int primary key identity,
Nombre varchar(100),
TipoID int foreign key (TipoID) references Tipo(TipoID),
ProveedorID int foreign key (ProveedorID) references Proveedor(ProveedorID),
CategoriaID int foreign key (CategoriaID) references Categoria(CategoriaID),
Descripcion varchar(250),
PrecioUnitario decimal(10,2),
FechaCaducidad date,
rutaimagen varchar(500),
nombreimagen varchar(500),
Stock int
)
go


create table Proveedor
(
ProveedorID int primary key identity,
Nombre varchar(100),
Correo varchar(50),
Telefono varchar(20),
Direccion varchar(500)
);
go




create table Usuario
(
UsuarioID int primary key identity,
Correo varchar(500),
clave varchar(250),
administrador bit default 0,
activo bit default 1,
reestablecer bit NOT NULL DEFAULT 1
)
go



create table Venta
(
VentaID int primary key identity,
UsuarioID int foreign key (UsuarioID) references Usuario(UsuarioID),
CantidadVendida int,
MontoTotal decimal (10,2),
TransaccionID varchar(50),
FechaVenta date default getdate()
)
go

create table DetalleVenta(
DetalleVentaID int primary key identity,
VentaID int references Venta(VentaID),
InventarioID int foreign key (InventarioID) references Inventario(InventarioID),
Cantidad int,
Total decimal(10,2)
)


create table Tipo(
TipoID int primary key identity,
Nombre varchar(100)
)

create table Categoria
(CategoriaID int primary key identity,
Nombre varchar(100)
)




create table Carrito (
CarritoID int primary key identity,
UsuarioID int references Usuario(UsuarioID),
InventarioID int references Inventario(InventarioID),
Cantidad int
)
go

--procedimientos--


CREATE PROCEDURE ListarUsuarios
AS
BEGIN
    select UsuarioID, Correo, clave, administrador, activo, reestablecer
    FROM Usuario
END
go



CREATE PROCEDURE ListarClientes
AS
BEGIN
    select UsuarioID, Correo, clave, administrador, activo, reestablecer
    FROM Usuario where administrador = 0
END
go




EXEC ListarUsuarios
go



create proc AgregarUsuario(
@Correo varchar(500),
@Clave varchar(250),
@Admin bit,
@Activo bit,
@Mensaje varchar(500) output,
@Resultado int output
)
as
begin
	set @Resultado = 0
	if not exists (select * from Usuario where Correo = @Correo)
	begin
		insert into Usuario(Correo, clave, administrador, activo) values
		(@Correo, @Clave,@Admin,@Activo)

		set @Resultado = SCOPE_IDENTITY()
	end
	else
	 set @Mensaje ='El correo del usuario ya existe'
end
go



create proc AgregarCliente(
@Correo varchar(500),
@Clave varchar(250),
@Mensaje varchar(500) output,
@Resultado int output
)
as
begin
	set @Resultado = 0
	if not exists (select * from Usuario where Correo = @Correo)
	begin
		insert into Usuario(Correo, clave, administrador, activo, reestablecer) values
		(@Correo, @Clave,0,1, 0)

		set @Resultado = SCOPE_IDENTITY()
	end
	else
	 set @Mensaje ='El correo del usuario ya existe'
end
go


execute ListarUsuarios



create proc EditarUsuario(
@UsuarioID int,
@Correo varchar(500),
@Admin bit,
@Activo bit,
@Mensaje varchar(500) output,
@Resultado bit output
)
as
begin
	set @Resultado =0
	if not exists (select *from Usuario where Correo=@Correo and UsuarioID != @UsuarioID)
	begin
		update top (1) Usuario set 
		Correo = @Correo,
		administrador = @Admin,
		activo = @Activo
		where UsuarioID = @UsuarioID

		set @Resultado = 1
	end
	else
	set @Mensaje='El correo del usuario ya existe'
end
go



CREATE PROCEDURE EliminarUsuario
    @UsuarioID INT,
    @Resultado BIT OUTPUT,
    @Mensaje NVARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DELETE TOP (1) FROM Usuario
        WHERE UsuarioID = @UsuarioID;

        IF @@ROWCOUNT > 0
        BEGIN
            SET @Resultado = 1;
            SET @Mensaje = 'Usuario eliminado correctamente';
        END
        ELSE
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'No se encontró usuario para eliminar';
        END
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
go



create proc ListarCategoria
AS
BEGIN
    select CategoriaID, Nombre from Categoria
END
go



create proc AgregarCategoria (
@Nombre varchar(100),
@Resultado int output,
@Mensaje varchar(500) output
)
as
begin
	set @Resultado = 0
	if not exists (select * from Categoria where Nombre = @Nombre)
	begin
		insert into Categoria(Nombre) values (@Nombre)

		set @Resultado = SCOPE_IDENTITY()
	end
	else
	set @Mensaje = 'La categoria ya existe'
end
go



create proc EditarCategoria(
@CategoriaID int,
@Nombre varchar(100),
@Resultado int output,
@Mensaje varchar(500) output
)
as
begin
	set @Resultado = 0
	if not exists (select * from Categoria where Nombre = @Nombre and CategoriaID != @CategoriaID)
		begin
		update top (1) Categoria set Nombre = @Nombre where CategoriaID = @CategoriaID

		set @Resultado = 1
		end
		else
		 set @Mensaje = 'La categoria ya existe'
end
go



create proc EliminarCategoria(
@CategoriaID int,
@Resultado int output,
@Mensaje varchar(500) output
)
as
begin
	set @Resultado = 0
	if not exists (select * from Inventario i inner join Categoria c on c.CategoriaID = i.CategoriaID where i.CategoriaID = @CategoriaID)
	begin
		delete top (1) from Categoria where CategoriaID = @CategoriaID
		set @Resultado =1
	end
	else
	set @Mensaje = 'La categoria está relacionada a un producto y no se puede eliminar'
end
go





create proc ListarTipo
AS
BEGIN
    select TipoID, Nombre from Tipo
END
go



create proc AgregarTipo (
@Nombre varchar(100),
@Resultado int output,
@Mensaje varchar(500) output
)
as
begin
	set @Resultado = 0
	if not exists (select * from Tipo where Nombre = @Nombre)
	begin
		insert into Tipo(Nombre) values (@Nombre)

		set @Resultado = SCOPE_IDENTITY()
	end
	else
	set @Mensaje = 'Ese tipo de medicamento ya existe'
end
go




create proc EditarTipo(
@TipoID int,
@Nombre varchar(100),
@Resultado int output,
@Mensaje varchar(500) output
)
as
begin
	set @Resultado = 0
	if not exists (select * from Tipo where Nombre = @Nombre and TipoID != @TipoID)
		begin
		update top (1) Tipo set Nombre = @Nombre where TipoID = @TipoID

		set @Resultado = 1
		end
		else
		 set @Mensaje = 'Ese tipo de medicamento ya existe'
end
go





create proc EliminarTipo(
@TipoID int,
@Resultado int output,
@Mensaje varchar(500) output
)
as
begin
	set @Resultado = 0
	if not exists (select * from Inventario i inner join Tipo t on t.TipoID = i.TipoID where i.TipoID = @TipoID)
	begin
		delete top (1) from Tipo where TipoID = @TipoID
		set @Resultado =1
	end
	else
	set @Mensaje = 'El tipo está relacionado a un producto y no se puede eliminar'
end
go




create proc ListarProveedor
AS
BEGIN
    select ProveedorID, Nombre, Correo, Telefono, Direccion from Proveedor
END
go



create proc AgregarProveedor (
@Nombre varchar(100),
@Correo varchar(50),
@Telefono varchar(20),
@Direccion varchar(500),
@Resultado int output,
@Mensaje varchar(500) output
)
as
begin
	set @Resultado = 0
	if not exists (select * from Proveedor where Nombre = @Nombre)
	begin
		insert into Proveedor(Nombre, Correo, Telefono, Direccion) values (@Nombre, @Correo,@Telefono,@Direccion)

		set @Resultado = SCOPE_IDENTITY()
	end
	else
	set @Mensaje = 'Ese proveedor de medicamentos ya existe'
end
go



CREATE PROC EditarProveedor(
    @ProveedorID INT,
    @Nombre VARCHAR(100),
    @Correo VARCHAR(50),
    @Telefono VARCHAR(20),
    @Direccion VARCHAR(500),
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET @Resultado = 0
    SET @Mensaje = ''
    IF NOT EXISTS (
        SELECT 1
        FROM Proveedor
        WHERE Nombre = @Nombre
          AND ProveedorID != @ProveedorID
    )
    BEGIN
        UPDATE Proveedor
        SET Nombre = @Nombre,
            Correo = @Correo,
            Telefono = @Telefono,
            Direccion = @Direccion
        WHERE ProveedorID = @ProveedorID

        IF @@ROWCOUNT > 0
        BEGIN
            SET @Resultado = 1
        END
        ELSE
        BEGIN
            SET @Mensaje = 'No se encontró el proveedor para actualizar.'
        END
    END
    ELSE
    BEGIN
        SET @Mensaje = 'Ese Proveedor ya existe.'
    END
END
GO



create proc EliminarProveedor(
@ProveedorID int,
@Resultado int output,
@Mensaje varchar(500) output
)
as
begin
	set @Resultado = 0
	if not exists (select * from Inventario i inner join Proveedor p on p.ProveedorID = i.ProveedorID where i.ProveedorID = @ProveedorID)
	begin
		delete top (1) from Proveedor where ProveedorID = @ProveedorID
		set @Resultado =1
	end
	else
	set @Mensaje = 'El proveedor está relacionado a un producto y no se puede eliminar'
end
go




create proc ListarInventario
as
begin
	select 
	i.InventarioID, i.Nombre,
	t.TipoID, t.Nombre[NomTipo],
	p.ProveedorID, p.Nombre[NomProv],
	c.CategoriaID, c.Nombre[NomCat],
	i.Descripcion, i.PrecioUnitario, i.FechaCaducidad, i.rutaimagen, i.nombreimagen, i.Stock
	from Inventario i
	inner join Tipo t on t.TipoID = i.TipoID
	inner join Proveedor p on p.ProveedorID = i.ProveedorID
	inner join Categoria c on c.CategoriaID = i.CategoriaID
end
go

execute ListarInventario
go


create proc AgregarProducto (
@Nombre varchar(100),
@TipoID int,
@ProveedorID int,
@CategoriaID int,
@Descripcion varchar(250),
@PrecioUnitario decimal(10,2),
@FechaCaducidad date,
@Stock int,
@Resultado int output,
@Mensaje varchar(500) output
)
as
begin
	set @Resultado = 0
	if not exists (select * from Proveedor where Nombre = @Nombre)
	begin
		insert into Inventario(Nombre, TipoID, ProveedorID, CategoriaID, Descripcion, PrecioUnitario, FechaCaducidad, Stock)
		values (@Nombre,@TipoID,@ProveedorID,@CategoriaID,@Descripcion,@PrecioUnitario,@FechaCaducidad, @Stock)

		set @Resultado = SCOPE_IDENTITY()
	end
	else
	set @Mensaje = 'Ese medicamento ya existe'
end
go



create proc GuardarDatosImagen
@InventarioID int,
@rutaimagen varchar(500),
@nombreimagen varchar(500)
as
begin
	update top (1) Inventario set rutaimagen = @rutaimagen, nombreimagen=@nombreimagen where InventarioID = @InventarioID
end
go


create proc EditarProducto(
@InventarioID int,
@Nombre varchar(100),
@TipoID int,
@ProveedorID int,
@CategoriaID int,
@Descripcion varchar(250),
@PrecioUnitario decimal(10,2),
@FechaCaducidad date,
@Stock int,
@Resultado int output,
@Mensaje varchar(500) output
)
as
begin
	set @Resultado = 0
	if not exists (select * from Inventario where Nombre = @Nombre and InventarioID != @InventarioID)
		begin
		update top (1) Inventario set Nombre = @Nombre,TipoID=@TipoID,ProveedorID=@ProveedorID,CategoriaID=@CategoriaID,Descripcion=@Descripcion,PrecioUnitario=@PrecioUnitario,FechaCaducidad=@FechaCaducidad, Stock=@Stock
		where ProveedorID = @ProveedorID

		set @Resultado = 1
		end
		else
		 set @Mensaje = 'Ese medicamento ya existe'
end
go



create proc EliminarProducto(
@InventarioID int,
@Resultado int output,
@Mensaje varchar(500) output
)
as
begin
	set @Resultado = 0
	if not exists (select * from DetalleVenta dv inner join Inventario i on i.InventarioID = dv.InventarioID where i.InventarioID = @InventarioID)
	begin
		delete top (1) from Inventario where InventarioID = @InventarioID
		set @Resultado =1
	end
	else
	set @Mensaje = 'El producto está relacionado a una venta y no se puede eliminar'
end
go



create proc ReporteDashboard
as
begin
select
(select COUNT(*) from Usuario) [TotalCliente],
(select ISNULL(SUM(cantidad),0) from DetalleVenta) [TotalVenta],
(select COUNT(*) from Inventario) [TotalProductos],
(select count(*) from Proveedor)[TotalProveedor]
end
go



create proc HistorialVentas
(
@fechaInicio varchar(10),
@fechaFin varchar(10),
@idTransaccion varchar(50)
)
as
begin
	
	set dateformat dmy;

	select CONVERT(char(10),v.FechaVenta,103)[FechaVenta], u.Correo[Usuario], i.Nombre[Producto], i.PrecioUnitario[Precio], dv.Cantidad, dv.Total, v.TransaccionID
	from DetalleVenta dv
	inner join Inventario i on i.InventarioID = dv.InventarioID
	inner join Venta v on v.VentaID = dv.VentaID
	inner join Usuario u on u.UsuarioID = v.UsuarioID
	where CONVERT(date, v.FechaVenta) between @fechaInicio and @fechaFin
	and v.TransaccionID = iif(@idTransaccion = '', v.TransaccionID, @idTransaccion)
end
go



CREATE PROCEDURE CambiarClaveUsuario
    @UsuarioID INT,
    @NuevaClave VARCHAR(250),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
	IF EXISTS (SELECT 1 FROM Usuario WHERE UsuarioID = @UsuarioID)
	BEGIN
	    UPDATE Usuario SET Clave = @NuevaClave, reestablecer = 0 WHERE UsuarioID = @UsuarioID;
	
	    SET @Resultado = 1;
	    SET @Mensaje = 'Contraseña actualizada correctamente.';
	END
	ELSE
	BEGIN
	    SET @Resultado = 0;
	    SET @Mensaje = 'No se encontró el usuario especificado.';
	END
END;
GO






execute CambiarClaveUsuario @UsuarioID = 20, @NuevaClave = '8100e127c51d089ba552c2d0d76dc9c812b9c529da6670052a5bce72714f62a3', @Resultado =1, @Mensaje ='Probando si funciona el proc'


execute ListarUsuarios


CREATE PROCEDURE ReestablecerClaveUsuario
    @UsuarioID INT,
    @Clave VARCHAR(250),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
	IF EXISTS (SELECT 1 FROM Usuario WHERE UsuarioID = @UsuarioID)
	BEGIN
	    UPDATE Usuario
	    SET Clave = @Clave, reestablecer = 1
	    WHERE UsuarioID = @UsuarioID;
	
	    SET @Resultado = 1;
	    SET @Mensaje = 'Contraseña actualizada correctamente.';
	END
	ELSE
	BEGIN
	    SET @Resultado = 0;
	    SET @Mensaje = 'No se encontró el usuario especificado.';
	END
END;
GO




select * from Usuario

























