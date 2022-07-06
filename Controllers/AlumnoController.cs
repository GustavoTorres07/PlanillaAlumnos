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

                        //return View(db.Alumno.Where(a => a.Nombre.StartsWith(nombreFiltro)).ToList()); || a.aApellido.StartsWith(nombreFiltro)).ToList();
                        return View(db.Alumno.Where(a =>
                        a.Nombre.StartsWith(Busqueda) ||
                        a.Apellido.StartsWith(Busqueda) ||
                        a.Sexo.StartsWith(Busqueda) ||
                        a.Edad.ToString().StartsWith(Busqueda) ||
                        a.Dni.ToString().StartsWith(Busqueda) ||
                        a.FechaRegistro.ToString().StartsWith(Busqueda)).ToList()
                        );



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

        //public ActionResult Agregar2()
        //{

        //    return View();

        //}

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

            Paragraph titulo = new Paragraph("Lista de Alumnos",_titulo);
            titulo.Alignment = Element.ALIGN_CENTER;

            doc.Add(titulo);

            Paragraph espacio = new Paragraph(" ");
            doc.Add(espacio);

            //Columnas (tabla)
            PdfPTable tabla = new PdfPTable(7);

            //Ancho columnas
            //float[] Valores = new float[6] { 90, 90, 80, 80, 80, 170 };
            float[] medidaCeldas = { 4.00f, 4.00f, 4.00f, 4.00f, 4.00f, 4.00f, 4.50f };
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
            }

            int nregistros = lista.Count;
            for (int i = 0; i < nregistros; i++)
            {

        
                PdfPCell clINombre = new PdfPCell(new Phrase(lista[i].Nombre,_standardFont));
                clINombre.VerticalAlignment = Element.ALIGN_MIDDLE;
                clINombre.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla.AddCell(clINombre);

   
                PdfPCell clIApellido = new PdfPCell(new Phrase(lista[i].Apellido,_standardFont));
                clIApellido.VerticalAlignment = Element.ALIGN_MIDDLE;
                clIApellido.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla.AddCell(clIApellido);

                PdfPCell clISexo = new PdfPCell(new Phrase(lista[i].Sexo,_standardFont));
                clISexo.VerticalAlignment = Element.ALIGN_MIDDLE;
                clISexo.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla.AddCell(clISexo);
       
                PdfPCell clIEdad = new PdfPCell(new Phrase(lista[i].Edad.ToString(),_standardFont));
                clIEdad.VerticalAlignment = Element.ALIGN_MIDDLE;
                clIEdad.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla.AddCell(clIEdad);
           
                PdfPCell clIDni = new PdfPCell(new Phrase(lista[i].Dni.ToString(),_standardFont));
                clIDni.VerticalAlignment = Element.ALIGN_MIDDLE;
                clIDni.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla.AddCell(clIDni);

                PdfPCell clICiudad = new PdfPCell(new Phrase(lista[i].CodCiudad.ToString(), _standardFont));
                clICiudad.VerticalAlignment = Element.ALIGN_MIDDLE;
                clICiudad.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla.AddCell(clICiudad);

                PdfPCell clIFechaRegistro = new PdfPCell(new Phrase(lista[i].FechaRegistro.ToShortDateString(),_standardFont));
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
