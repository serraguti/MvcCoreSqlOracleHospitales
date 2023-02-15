using MvcCoreSqlOracleHospitales.Models;

namespace MvcCoreSqlOracleHospitales.Repositories
{
    public interface IRepositoryHospital
    {
        List<Hospital> GetHospitales();
        Hospital FindHospital(int idhospital);
        void InsertHospital(string nombre, string direccion, string telefono, int camas);
        void UpdateHospital(int idhospital, string nombre
            , string direccion, string telefono, int camas);
        void DeleteHospital(int idhospital);
    }
}
