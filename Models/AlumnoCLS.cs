using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlanillaAlumnos.Models
{
    public class AlumnoCLS
    {

        [Required]
        [Display(Name = "Nombre")]
        public string NombreAlumno { get; set; }
        [Required]
        [Display(Name = "Apellido")]
        public string ApellidoAlumno { get; set; }
        [Required]
        [Display(Name = "Sexo")]
        public string SexoAlumno { get; set; }
        [Required]
        [Display(Name = "Edad")]
        public int EdadAlumno { get; set; }
        [Required]
        [Display(Name = "Dni")]
        public int DniAlumno { get; set; }
        [Required]
        [Display(Name = "Ciudad")]
        public int IdCiudad { get; set; }
        public string Ciudad { get; set; }




    }
    [MetadataType(typeof(AlumnoCLS))]

    public partial class Alumno
    {

    }

}