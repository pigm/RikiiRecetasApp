using System;
using Realms;

namespace cl.Common.RikiiRecetas.Models.Modelo
{
    public class Receta
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public Insumos insumos { get; set; }
        public string tiempo { get; set; }
        public string cantidad { get; set; }
        public string preparacion { get; set; }
        public string dificultad { get; set; }
        public string pais { get; set; }
        public string imagen { get; set; }
        public bool isFavorita = false;
    }
}
