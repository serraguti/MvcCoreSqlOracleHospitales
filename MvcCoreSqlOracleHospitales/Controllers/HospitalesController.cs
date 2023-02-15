using Microsoft.AspNetCore.Mvc;
using MvcCoreSqlOracleHospitales.Models;
using MvcCoreSqlOracleHospitales.Repositories;

namespace MvcCoreSqlOracleHospitales.Controllers
{
    public class HospitalesController : Controller
    {
        private IRepositoryHospital repo;

        public HospitalesController(IRepositoryHospital repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Hospital> hospitales = this.repo.GetHospitales();
            return View(hospitales);
        }

        public IActionResult Details(int id)
        {
            Hospital hospital = this.repo.FindHospital(id);
            return View(hospital);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Hospital hospital)
        {
            this.repo.InsertHospital(hospital.Nombre
                , hospital.Direccion, hospital.Telefono, hospital.Camas);
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            Hospital hospital = this.repo.FindHospital(id);
            return View(hospital);
        }

        [HttpPost]
        public IActionResult Update(Hospital hospital)
        {
            this.repo.UpdateHospital(hospital.IdHospital
                , hospital.Nombre, hospital.Direccion, hospital.Telefono
                , hospital.Camas);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            this.repo.DeleteHospital(id);
            return RedirectToAction("Index");
        }
    }
}
