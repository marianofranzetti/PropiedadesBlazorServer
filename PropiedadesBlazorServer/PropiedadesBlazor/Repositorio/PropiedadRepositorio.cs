using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PropiedadesBlazor.Data;
using PropiedadesBlazor.Modelos;
using PropiedadesBlazor.Modelos.DTO;
using PropiedadesBlazor.Repositorio.IRepositorio;

namespace PropiedadesBlazor.Repositorio
{
    public class PropiedadRepositorio : IPropiedadRepositorio
    {
        private readonly ApplicationDbContext _bd;
        private readonly IMapper _mapper;

        public PropiedadRepositorio(ApplicationDbContext bd, IMapper mapper)
        {
            _bd = bd;
            _mapper = mapper;
        }


        public async Task<PropiedadDTO> ActualizarPropiedad(int propiedadId, PropiedadDTO propiedadDTO)
        {
            try
            {
                if (propiedadId == propiedadDTO.Id)
                {
                    //Válido para actualizar
                    Propiedad propiedad = await _bd.Propiedad.FindAsync(propiedadId);
                    Propiedad propie = _mapper.Map<PropiedadDTO, Propiedad>(propiedadDTO, propiedad);
                    propie.FechaActualizacion = DateTime.Now;
                    var propiedadActualizada = _bd.Propiedad.Update(propie);
                    await _bd.SaveChangesAsync();
                    return _mapper.Map<Propiedad, PropiedadDTO>(propiedadActualizada.Entity);
                }
                else
                {
                    //Inválido
                    return null;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<int> BorrarPropiedad(int propiedadId)
        {
            var propiedad = await _bd.Propiedad.FindAsync(propiedadId);
            if (propiedad != null)
            {
                var todasImagenes = await _bd.ImagenPropiedad.Where(x => x.Id == propiedadId).ToListAsync();
                _bd.ImagenPropiedad.RemoveRange(todasImagenes);
                _bd.Propiedad.Remove(propiedad);
                return await _bd.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<PropiedadDTO> CrearPropiedad(PropiedadDTO propiedadDTO)
        {
            Propiedad propiedad = _mapper.Map<PropiedadDTO, Propiedad>(propiedadDTO);
            propiedad.FechaCreacion = DateTime.Now;
            var propiedadAgregada = await _bd.Propiedad.AddAsync(propiedad);
            await _bd.SaveChangesAsync();
            return _mapper.Map<Propiedad, PropiedadDTO>(propiedadAgregada.Entity);
        }

        public async Task<IEnumerable<PropiedadDTO>> GetAllPropiedades()
        {
            //Versión 1
            //try
            //{
            //    IEnumerable<PropiedadDTO> propiedadesDTO =
            //                _mapper.Map<IEnumerable<Propiedad>, IEnumerable<PropiedadDTO>>
            //                (_bd.Propiedad);
            //    return (propiedadesDTO);
            //}
            //catch (Exception ex)
            //{

            //    return null;
            //}

            //Versión 2
            try
            {
                IEnumerable<PropiedadDTO> propiedadesDTO =
                            _mapper.Map<IEnumerable<Propiedad>, IEnumerable<PropiedadDTO>>
                            (_bd.Propiedad.Include(x => x.ImagenPropiedad).Include(c => c.Categoria));
                return propiedadesDTO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<PropiedadDTO> GetPropiedad(int propiedadId)
        {
            try
            {
                PropiedadDTO propiedadDTO =
                           _mapper.Map<Propiedad, PropiedadDTO>
                           (await _bd.Propiedad.FirstOrDefaultAsync(c => c.Id == propiedadId));
                return propiedadDTO;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<PropiedadDTO> NombrePropiedadExiste(string nombre)
        {
            try
            {
                PropiedadDTO propiedadDTO =
                           _mapper.Map<Propiedad, PropiedadDTO>
                           (await _bd.Propiedad.FirstOrDefaultAsync(
                               c => c.Nombre.ToLower() == nombre.ToLower()));
                return propiedadDTO;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
