using System;
using Realms;

namespace cl.Common.RikiiRecetas.Models.Modelo
{
    /// <summary>
    /// Receta favorita.
    /// </summary>
    public class RecetaFavorita : RealmObject
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string tiempo { get; set; }
        public string cantidad { get; set; }
        public string dificultad { get; set; }
        public string pais { get; set; }
        public string imagen { get; set; }
        public bool isFavorita { get; set; }
    }
}
