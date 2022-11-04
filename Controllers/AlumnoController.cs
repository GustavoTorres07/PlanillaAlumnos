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
        public ActionResult Index(string Busqueda)
        {
            if (Busqueda != null)
            {
                try
                {
                    using (AlumnosContext db = new AlumnosContext())
                    {
                        var data = db.Alumno.Where(a =>
                        a.Ciudad.NombreCiudad.StartsWith(Busqueda) ||
                        a.NombreAlumno.StartsWith(Busqueda) ||
                        a.ApellidoAlumno.StartsWith(Busqueda) ||
                        a.SexoAlumno.StartsWith(Busqueda) ||
                        a.EdadAlumno.ToString().StartsWith(Busqueda) ||
                        a.DniAlumno.ToString().StartsWith(Busqueda) ||
                        a.FechaRegistroAlumno.ToString().StartsWith(Busqueda)).ToList();
                        return View(data);




                    }

                }
                catch (Exception)
                {

                    throw;
                }

            }
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

        [HttpPost]
        public ActionResult Index(FilterDTO dto)
        {
            ViewBag.Search = dto.Search;
            using (AlumnosContext db = new AlumnosContext())
            {
                if (!string.IsNullOrEmpty(dto.Search) || !string.IsNullOrWhiteSpace(dto.Search))
                {
                    var data = db.Alumno.Where(a => a.ApellidoAlumno.Contains(dto.Search));
                    if (dto.Toggle)
                        data = db.Alumno.Where(a => a.DniAlumno.ToString().Contains(dto.Search));
                    return View("Index", data.ToList());
                }
                return View("Index", db.Alumno.ToList());
            }
        }

        public ActionResult AgregarAlumno()
        {

            return View();

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("AgregarAlumno")]
        public ActionResult AgregarAlumno(Alumno a)
        {

            if (!ModelState.IsValid)
                return View();
            try
            {
                using (AlumnosContext db = new AlumnosContext())
                {
                    a.FechaRegistroAlumno = DateTime.Now;

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


        public ActionResult ListaCiudades()
        {

            using (var db = new AlumnosContext())
            {

                return PartialView(db.Ciudad.ToList());

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
                    al.NombreAlumno = a.NombreAlumno;
                    al.ApellidoAlumno = a.ApellidoAlumno;
                    al.SexoAlumno = a.SexoAlumno;
                    al.EdadAlumno = a.EdadAlumno;
                    al.DniAlumno = a.DniAlumno;

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
                alu.Ciudad = db.Ciudad.Where(i => i.Id == alu.IdCiudad).First();
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

        public static string NombreCiudad(int CodCiudad)
        {
            using (var db = new AlumnosContext())
            {

                return db.Ciudad.Find(CodCiudad).NombreCiudad;

            }


        }


        public FileResult generarPDF()
        {

            Document doc = new Document(PageSize.LETTER);
            byte[] buffer;
            MemoryStream ms = new MemoryStream();
            //using (MemoryStream ms = new MemoryStream())
            //{

            PdfWriter.GetInstance(doc, ms);
            doc.Open();

            //defino tipo de letra, tamaño y color
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 14, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _titulo = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 20, iTextSharp.text.Font.NORMAL, BaseColor.DARK_GRAY);

            Paragraph titulo = new Paragraph("Lista de Alumnos", _titulo);
            titulo.Alignment = Element.ALIGN_CENTER;

            doc.Add(titulo);

            Paragraph espacio = new Paragraph(" ");
            doc.Add(espacio);

            //Columnas (tabla)
            PdfPTable tabla = new PdfPTable(7);

            //Ancho columnas
            //float[] Valores = new float[6] { 90, 90, 80, 80, 80, 170 };
            float[] medidaCeldas = { 10.00f, 10.00f, 10.00f, 10.00f, 12.00f, 15.00f, 13.00f };
            //asigno esos anchos a la tabla
            tabla.SetWidths(medidaCeldas);

            //creo la celda (le pongo contenido) color y alineado al centro
            /*PdfPCell celda1 = new PdfPCell(new Paragraph("Nombre"));
            celda1.VerticalAlignment = Element.ALIGN_MIDDLE;
            celda1.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla.AddCell(celda1);*/

            PdfPCell celda1 = new PdfPCell(new Phrase("Nombre"));
            celda1.BackgroundColor = new BaseColor(243, 179, 92);
            celda1.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla.AddCell(celda1);

            PdfPCell celda2 = new PdfPCell(new Phrase("Apellido"));
            celda2.BackgroundColor = new BaseColor(243, 179, 92);
            celda2.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla.AddCell(celda2);

            PdfPCell celda3 = new PdfPCell(new Phrase("Sexo"));
            celda3.BackgroundColor = new BaseColor(243, 179, 92);
            celda3.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla.AddCell(celda3);

            PdfPCell celda4 = new PdfPCell(new Phrase("Edad"));
            celda4.BackgroundColor = new BaseColor(243, 179, 92);
            celda4.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla.AddCell(celda4);

            PdfPCell celda5 = new PdfPCell(new Phrase("Dni"));
            celda5.BackgroundColor = new BaseColor(243, 179, 92);
            celda5.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla.AddCell(celda5);

            PdfPCell celda6 = new PdfPCell(new Phrase("Ciudad"));
            celda6.BackgroundColor = new BaseColor(243, 179, 92);
            celda6.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla.AddCell(celda6);

            PdfPCell celda7 = new PdfPCell(new Phrase("Fecha de Registro"));
            celda7.BackgroundColor = new BaseColor(243, 179, 92);
            celda7.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla.AddCell(celda7);


            List<Alumno> lista = new List<Alumno>();
            using (AlumnosContext db = new AlumnosContext())
            {
                lista = db.Alumno.ToList();
                //}

                int nregistros = lista.Count;
                for (int i = 0; i < nregistros; i++)
                {

                    PdfPCell clINombre = new PdfPCell(new Phrase(lista[i].NombreAlumno, _standardFont));
                    clINombre.VerticalAlignment = Element.ALIGN_MIDDLE;
                    clINombre.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabla.AddCell(clINombre);


                    PdfPCell clIApellido = new PdfPCell(new Phrase(lista[i].ApellidoAlumno, _standardFont));
                    clIApellido.VerticalAlignment = Element.ALIGN_MIDDLE;
                    clIApellido.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabla.AddCell(clIApellido);

                    PdfPCell clISexo = new PdfPCell(new Phrase(lista[i].SexoAlumno, _standardFont));
                    clISexo.VerticalAlignment = Element.ALIGN_MIDDLE;
                    clISexo.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabla.AddCell(clISexo);

                    PdfPCell clIEdad = new PdfPCell(new Phrase(lista[i].EdadAlumno.ToString(), _standardFont));
                    clIEdad.VerticalAlignment = Element.ALIGN_MIDDLE;
                    clIEdad.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabla.AddCell(clIEdad);

                    PdfPCell clIDni = new PdfPCell(new Phrase(lista[i].DniAlumno.ToString(), _standardFont));
                    clIDni.VerticalAlignment = Element.ALIGN_MIDDLE;
                    clIDni.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabla.AddCell(clIDni);

                    PdfPCell clICiudad = new PdfPCell(new Phrase(lista[i].Ciudad.NombreCiudad.ToString(), _standardFont));
                    clICiudad.VerticalAlignment = Element.ALIGN_MIDDLE;
                    clICiudad.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabla.AddCell(clICiudad);

                    PdfPCell clIFechaRegistro = new PdfPCell(new Phrase(lista[i].FechaRegistroAlumno.ToShortDateString(), _standardFont));
                    clIFechaRegistro.VerticalAlignment = Element.ALIGN_MIDDLE;
                    clIFechaRegistro.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabla.AddCell(clIFechaRegistro);


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



}