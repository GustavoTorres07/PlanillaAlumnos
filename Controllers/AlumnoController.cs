using iTextSharp.text;
using iTextSharp.text.pdf;
using PlanillaAlumnos.Models;
using PlanillaAlumnos.Permisos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanillaAlumnos.Controllers
{
    [ValidarSesion]
    public class AlumnoController : Controller
    {
        // GET: Alumno
        public ActionResult Index()
        {
            try
            {
                using (AlumnosContext db = new AlumnosContext())
                {

                    return View(db.Alumno.ToList());

                }

            }
            catch (Exception)
            {

                throw;
            }


        }


        public ActionResult AgregarAlumno()
        {

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarAlumno(Alumno a)
        {

            if (!ModelState.IsValid)

                return View();

            try
            {
                using (AlumnosContext db = new AlumnosContext())
                {
                    a.FechaRegistro = DateTime.Now;

                    db.Alumno.Add(a);
                    db.SaveChanges();


                    return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Error al Agregar el Alumno - " + ex.Message);
                return View();
            }



        }


        public ActionResult EditarAlumno(int id)
        {
            try
            {
                using (AlumnosContext db = new AlumnosContext())
                {
                    /* el where nos permite usarlo en todos los casos, al find lo usamos cuando solo hay una llave primaria en la tabla */
                    //Alumno al = db.Alumno.Where(a => a.Id == id).FirstOrDefault();
                    Alumno alu = db.Alumno.Find(id);

                    return View(alu);

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarAlumno(Alumno a)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();


                using (AlumnosContext db = new AlumnosContext())
                {
                    Alumno al = db.Alumno.Find(a.Id);
                    al.Nombre = a.Nombre;
                    al.Apellido = a.Apellido;
                    al.Sexo = a.Sexo;
                    al.Edad = a.Edad;
                    al.Dni = a.Dni;

                    db.SaveChanges();

                    return RedirectToAction("Index");

                }

            }
            catch (Exception)
            {

                throw;
            }



        }

        public ActionResult DetallesAlumno(int id)
        {

            using (AlumnosContext db = new AlumnosContext())
            {

                Alumno alu = db.Alumno.Find(id);

                return View(alu);
            }


        }


        public ActionResult EliminarAlumno(int id)
        {

            using (AlumnosContext db = new AlumnosContext())
            {

                Alumno alu = db.Alumno.Find(id);
                db.Alumno.Remove(alu);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

        }


        public FileResult generarPDF()
        {

            Document doc = new Document();
            byte[] buffer;
            MemoryStream ms = new MemoryStream();
            //using (MemoryStream ms = new MemoryStream())
            //{

            PdfWriter.GetInstance(doc, ms);
            doc.Open();

            Paragraph titulo = new Paragraph("Lista Alumnos");
            titulo.Alignment = Element.ALIGN_CENTER;
            doc.Add(titulo);

            Paragraph espacio = new Paragraph(" ");
            doc.Add(espacio);

            //Columnas (tabla)
            PdfPTable tabla = new PdfPTable(2);
            //Ancho columnas
            float[] Valores = new float[2] { 30, 40 };
            //asigno esos anchos a la tabla
            tabla.SetWidths(Valores);
            //creo la celda (le pongo contenido) color y alineado al centro
            PdfPCell celda1 = new PdfPCell(new Phrase("Nombre Alumno"));
            celda1.BackgroundColor = new BaseColor(130, 130, 130);
            celda1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            tabla.AddCell(celda1);

            PdfPCell celda2 = new PdfPCell(new Phrase("Apellido Alumno"));
            celda1.BackgroundColor = new BaseColor(130, 130, 130);
            celda1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            tabla.AddCell(celda2);
            List<Alumno> lista = new List<Alumno>();
            using (AlumnosContext db = new AlumnosContext())
            {
                lista = db.Alumno.ToList();
            }

            int nregistros = lista.Count;
            for (int i = 0; i < nregistros; i++)
            {

                tabla.AddCell(lista[i].Nombre);
                tabla.AddCell(lista[i].Apellido);


            }

            doc.Add(tabla);
            doc.Close();
            buffer = ms.ToArray();
            //ms1 = ms;


            //}
            return File(buffer, "application/pdf");

        }

    }



}
