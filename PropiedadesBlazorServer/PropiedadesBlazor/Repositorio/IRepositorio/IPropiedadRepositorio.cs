using PropiedadesBlazor.Modelos.DTO;

namespace PropiedadesBlazor.Repositorio.IRepositorio
{
    public interface IPropiedadRepositorio
    {
        public Task<IEnumerable<PropiedadDTO>> GetAllPropiedades();
        public Task<PropiedadDTO> GetPropiedad(int propiedadId);
        public Task<PropiedadDTO> CrearPropiedad(PropiedadDTO propiedadDTO);

        public Task<PropiedadDTO> ActualizarPropiedad(int propiedadId, PropiedadDTO propiedadDTO);

        public Task<int> BorrarPropiedad(int propiedadId);

        public Task<PropiedadDTO> NombrePropiedadExiste(string nombre);       

    }
}
