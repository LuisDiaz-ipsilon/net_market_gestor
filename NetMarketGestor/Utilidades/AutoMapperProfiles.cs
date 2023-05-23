using AutoMapper;
using NetMarketGestor.DTOs;
using NetMarketGestor.Models;
using System.Net.NetworkInformation;

namespace NetMarketGestor.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserDTO, User>();
            CreateMap<PedidoDTO, Pedido>();
            CreateMap<CarritoDTO, Carrito>();
            CreateMap<ProductDTO, Product>();

            CreateMap<User, GetUserDTO>();
            CreateMap<Pedido, GetPedidoDTO>();
            CreateMap<Carrito, GetCarritoDTO>();
            CreateMap<Product, GetProductDTO>();


            CreateMap<User, UserDTOConPedidos>()
                .ForMember(UserDTO => UserDTO.Pedidos, opciones => opciones.MapFrom(MapUserDTOConPedidos));
            CreateMap<User, UserDTOConCarrito> ()
                .ForMember(UserDTO => UserDTO.Carrito, opciones => opciones.MapFrom(MapUserDTOConCarrito));
            CreateMap <Carrito, CarritoDTOConProductos> ()
                .ForMember(CarritoDTO => CarritoDTO.Productos, opciones => opciones.MapFrom(MapCarritoDTOConProductos));
            CreateMap <Carrito, CarritoDTOConUser> ()
                .ForMember(CarritoDTO => CarritoDTO.User, opciones => opciones.MapFrom(MapCarritoDTOConUser));
            CreateMap <Pedido, PedidoDTOConProductos> ()
                .ForMember(PedidoDTO => PedidoDTO.Productos, opciones => opciones.MapFrom(MapPedidoDTOConProductos));
            CreateMap <Pedido, PedidoDTOConUsers> ()
                .ForMember(PedidoDTO => PedidoDTO.User, opciones => opciones.MapFrom(MapPedidoDTOConUsers));
            /*CreateMap <Product, ProductoDTOConPedido> () //No existe relacion desde producto a pedido relevante
                .ForMember(ProductDTO => ProductDTO., opciones => opciones.MapFrom(MapProductoDTOConPedido));*/ 


            /*//Tipos de CreateMap que hacen falta 
             * CreateMap<ClaseCreacionDTO, Clase>()
                .ForMember(clase => clase.AlumnoClase, opciones => opciones.MapFrom(MapAlumnoClase));

            CreateMap<Clase, ClaseDTOConAlumnos>()
                .ForMember(claseDTO => claseDTO.Alumnos, opciones => opciones.MapFrom(MapClaseDTOAlumnos));
            CreateMap<ClasePatchDTO, Clase>().ReverseMap();*/


        }

        private List<PedidoDTO> MapUserDTOConPedidos(User user, GetUserDTO getUserDTO)
        {
            var result = new List<PedidoDTO>();

            if(user.Pedidos.Count == 0)
            {
                return result;
            }

            foreach(var userPedido in user.Pedidos)
            {
                result.Add(new PedidoDTO()
                {
                    Id = userPedido.id,
                    Estatus = userPedido.Estatus
                });
            }
            return result;

        }
        private CarritoDTO MapUserDTOConCarrito(User user, UserDTOConCarrito userDTO)
        {
            var result =  new CarritoDTO();

            if(user.Carrito == null)
            {
                return result;
            }

            result.Id = user.Carrito.id;

            return result;
        }

        private List<ProductDTO> MapCarritoDTOConProductos(Carrito carrito, CarritoDTOConProductos carritoDTO)
        {
            var result = new List<ProductDTO>();

            if (carrito.productos.Count == 0)
            {
                return result;
            }

            foreach (var producto in carrito.productos)
            {
                result.Add(new ProductDTO()
                {
                    Precio = producto.Precio,
                    Id = producto.Id,
                    Nombre = producto.Nombre
                });
            }

            return result;
        }

        private UserDTO MapCarritoDTOConUser(Carrito carrito, CarritoDTOConUser carritoDTO)
        {

            var result = new UserDTO();

            if(carrito.user == null) { return result; }

            result.Id = carrito.user.Id;
            result.Nombre = carrito.user.Nombre;

            return result;
        }

        private List<ProductDTO> MapPedidoDTOConProductos(Pedido pedido, PedidoDTOConProductos pedidoDTO)
        {
            var result = new List<ProductDTO>();

            if (pedido.Productos.Count == 0)
            {
                return result;
            }

            foreach (var producto in pedido.Productos)
            {
                result.Add(new ProductDTO()
                {
                    Id = producto.Id,
                    Nombre = producto.Nombre,
                    Categoria = producto.Categoria,
                    Precio = producto.Precio

                });
            }

            return result;
        }

        private UserDTO MapPedidoDTOConUsers(Pedido pedido, PedidoDTOConUsers pedidoDTO)
        {
            var result = new UserDTO();

            if (pedido.User == null) { return result; }

            result.Id = pedido.User.Id;
            result.Nombre = pedido.User.Nombre;

            return result;
        }





    }
}