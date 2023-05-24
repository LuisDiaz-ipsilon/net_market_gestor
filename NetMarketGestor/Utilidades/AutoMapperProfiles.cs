using AutoMapper;
using NetMarketGestor.DTOs;
using NetMarketGestor.Models;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Collections.Generic;

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

            CreateMap<CarritoPatchDTO, Carrito>().ReverseMap();
            CreateMap<PedidoPatchDTO, Pedido>().ReverseMap();
            CreateMap<ProductPatchDTO, Product>().ReverseMap();
            CreateMap<UserPatchDTO, User>().ReverseMap();

            //No es necesario un Map de CarritoCreacion ya que este se crea al crear el usuario.
            /*CreateMap<CarritoCreacionDTO, Carrito>()
                .ForMember(carrito => carrito.user, opciones => opciones.MapFrom(MapUserCarrito));*/

            //PedidoCreacionDTO
            CreateMap<PedidoCreacionDTO, Pedido>()
                .ForMember(Pedido => Pedido.Productos, opciones => opciones.MapFrom<List<Product>>(MapCarritoPedido))
                .ForMember(Pedido => Pedido.User, opciones => opciones.MapFrom<User>(MapPedidoUser));

            CreateMap<ProductCreacionDTO, Product>();

            //Para crear un usuario es obligatorio crearle un Carrito vacio.
            CreateMap<UserCreacionDTO, User>()
                .ForMember(User => User.Carrito, opciones => opciones.MapFrom<Carrito>(MapCarritoUser));

        }

        private async Task<List<PedidoDTO>> MapUserDTOConPedidos(User user, GetUserDTO getUserDTO)
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
        private async Task<CarritoDTO>  MapUserDTOConCarrito(User user, UserDTOConCarrito userDTO)
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


        private User MapUserCarrito(CarritoCreacionDTO carritoCreacionDTO, User user)
        {
            var resultado = new User();

            if(carritoCreacionDTO.user == null)
            {
                return resultado;
            }

            if(carritoCreacionDTO.user.Id == user.Id)
            {
                resultado = user;
            }

            return resultado;
        }

        private List<Product> MapCarritoPedido(PedidoCreacionDTO pedidoCreacionDTO, Carrito carrito)
        {
            List<Product> resultado = new List<Product>();

            if(pedidoCreacionDTO.Productos.Count == 0)
            {
                return resultado;
            }

            if(pedidoCreacionDTO.User.Id == carrito.user.Id)
            {
                resultado.AddRange(carrito.productos);
            }

            return resultado;
        }

        private User MapPedidoUser(PedidoCreacionDTO pedidoCreacionDTO, User user)
        {
            User resultado = new User();

            if(pedidoCreacionDTO.User is null)
            {
                return resultado;
            }

            if(pedidoCreacionDTO.User.Id == pedidoCreacionDTO.User.Id)
            {
                resultado = user;
            }

            return resultado;
        }

        private Carrito MapCarritoUser(UserCreacionDTO userCreacionDTO, Carrito carrito)
        {
            var resultado = new Carrito();

            if (userCreacionDTO.Carrito is null)
            {
                return resultado;
            }

            if (userCreacionDTO.Carrito.user.Id == carrito.user.Id)
            {
                resultado = carrito;
            }

            return resultado;
        }

    }
}