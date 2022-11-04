using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlanillaAlumnos.Models
{
    public class AlumnoMateriaCLS
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int AñoCursada { get; set; }
        [Required]
        public int IdAlumno { get; set; }
        [Required]
        public string NombreAlumno { get; set; }
        [Required]
        public int DniAlumno { get; set; }
        [Required]
        public int IdMateria { get; set; }
        [Required]
        public string NombreMateria { get; set; }

    }
}