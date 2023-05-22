using AutoMapper;
using NetMarketGestor.DTOs;
using NetMarketGestor.Models;

namespace WebApiAlumnosSeg.Utilidades
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

            //Modificar acorde a nuestro caso lineas 22 a FIN
            CreateMap<Alumno, AlumnoDTOConClases>()
                .ForMember(alumnoDTO => alumnoDTO.Clases, opciones => opciones.MapFrom(MapAlumnoDTOClases));
            CreateMap<ClaseCreacionDTO, Clase>()
                .ForMember(clase => clase.AlumnoClase, opciones => opciones.MapFrom(MapAlumnoClase));
            CreateMap<Clase, ClaseDTO>();
            CreateMap<Clase, ClaseDTOConAlumnos>()
                .ForMember(claseDTO => claseDTO.Alumnos, opciones => opciones.MapFrom(MapClaseDTOAlumnos));
            CreateMap<ClasePatchDTO, Clase>().ReverseMap();
            CreateMap<CursoCreacionDTO, Cursos>();
            CreateMap<Cursos, CursoDTO>();
        }

        private List<ClaseDTO> MapAlumnoDTOClases(Alumno alumno, GetAlumnoDTO getAlumnoDTO)
        {
            var result = new List<ClaseDTO>();

            if (alumno.AlumnoClase == null) { return result; }

            foreach (var alumnoClase in alumno.AlumnoClase)
            {
                result.Add(new ClaseDTO()
                {
                    Id = alumnoClase.ClaseId,
                    Nombre = alumnoClase.Clase.Nombre
                });
            }

            return result;
        }

        private List<GetAlumnoDTO> MapClaseDTOAlumnos(Clase clase, ClaseDTO claseDTO)
        {
            var result = new List<GetAlumnoDTO>();

            if (clase.AlumnoClase == null)
            {
                return result;
            }

            foreach (var alumnoclase in clase.AlumnoClase)
            {
                result.Add(new GetAlumnoDTO()
                {
                    Id = alumnoclase.AlumnoId,
                    Nombre = alumnoclase.Alumno.Nombre
                });
            }

            return result;
        }

        private List<AlumnoClase> MapAlumnoClase(ClaseCreacionDTO claseCreacionDTO, Clase clase)
        {
            var resultado = new List<AlumnoClase>();

            if (claseCreacionDTO.AlumnosIds == null) { return resultado; }
            foreach (var alumnoId in claseCreacionDTO.AlumnosIds)
            {
                resultado.Add(new AlumnoClase() { AlumnoId = alumnoId });
            }
            return resultado;
        }
    }
}