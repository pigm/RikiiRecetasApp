using System;
using System.Collections.Generic;
using cl.Common.RikiiRecetas.Models.Modelo;
using Realms;

namespace cl.Common.RikiiRecetas.Utils
{
    /// <summary>
    /// Data manager.
    /// </summary>
    public class DataManager
    {
        /// <summary>
        /// Gets or sets the receta seleccionada.
        /// </summary>
        /// <value>The receta seleccionada.</value>
        public static Receta RecetaSeleccionada { get; set; }

        /// <summary>
        /// Gets or sets the paises.
        /// </summary>
        /// <value>The paises.</value>
        public static List<Pais> Paises { get; set; }

        /// <summary>
        /// Gets or sets the pais seleccionado.
        /// </summary>
        /// <value>The pais seleccionado.</value>
        public static Pais paisSeleccionado { get; set; }

        /// <summary>
        /// Gets or sets the actual latitud.
        /// </summary>
        /// <value>The actual latitud.</value>
        public static string actualLatitud { get; set; }

        /// <summary>
        /// Gets or sets the actual longitud.
        /// </summary>
        /// <value>The actual longitud.</value>
        public static string actualLongitud { get; set; }

        /// <summary>
        /// The realm.
        /// </summary>
        static Realm realm;

        public static bool estadoFiltro = false;
        public static bool VisualizarMisRecetas = false;

        /// <summary>
        /// Gets the realm instance.
        /// </summary>
        /// <value>The realm instance.</value>
        public static Realm RealmInstance
        {
            get
            {
                if (realm == null)
                {
                    try
                    {
                        RealmConfiguration config = new RealmConfiguration
                        {
                            SchemaVersion = 1,
                            ShouldDeleteIfMigrationNeeded = true
                        };
                        realm = Realm.GetInstance(config);
                    }
                    catch
                    {
                        realm = Realm.GetInstance();
                    }
                }
                return realm;
            }
        }
    }
}