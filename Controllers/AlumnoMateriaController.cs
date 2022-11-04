using PlanillaAlumnos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanillaAlumnos.Controllers
{
    public class AlumnoMateriaController : Controller
    {
        // GET: AlumnoMateria
        public ActionResult Index()
        {
            using (AlumnosContext db = new AlumnosContext())
            {

                return View(db.AlumnoMateria.ToList());

            }

        }

        public ActionResult AsignarAlumnoMateria()
        {

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Route("AgregarAlumno")]
        public ActionResult AsignarAlumnoMateria(AlumnoMateria am)
        {

            if (!ModelState.IsValid)
                return View();
            try
            {
                using (AlumnosContext db = new AlumnosContext())
                {
                    db.AlumnoMateria.Add(am);
                    db.SaveChanges();


                    return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Error al Asignar el Alumno a Materia - " + ex.Message);
                return View();
            }

        }

        public static string NombreAlumno(int IdAlumno)
        {
            using (var db = new AlumnosContext())
            {

                return db.Alumno.Find(IdAlumno).NombreAlumno;

            }


        }

        public static string NombreMateria(int IdMateria)
        {
            using (var db = new AlumnosContext())
            {

                return db.Materia.Find(IdMateria).NombreMateria;

            }


        }

        public static string DniAlumno(int IdAlumno)
        {
            using (var db = new AlumnosContext())
            {

                return db.Alumno.Find(IdAlumno).DniAlumno.ToString();

            }


        }


        public ActionResult ListaAlumnos()
        {

            using (var db = new AlumnosContext())
            {

                return PartialView(db.Alumno.ToList());

            }
        }

        public ActionResult ListaMaterias()
        {

            using (var db = new AlumnosContext())
            {

                return PartialView(db.Materia.ToList());

            }
        }
    }
}