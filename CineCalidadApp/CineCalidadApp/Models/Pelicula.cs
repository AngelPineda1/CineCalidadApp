using System;
using System.Collections.Generic;
using System.Text;

namespace CineCalidadApp.Models
{
    public enum Sala { A1,B1,A2,B2,A3,B3};
    public class Pelicula
    {
        public string Titulo { get; set; }
        public string Genero { get;set; }
        public Sala Sala { get; set; }
        public string Duracion { get; set; }
        public string Imagen { get; set; }

    }
}
