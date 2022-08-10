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

            AlumnosContext db = new AlumnosContext();

            return View(db.AlumnoMateria.ToList());
        }
    }


    
}