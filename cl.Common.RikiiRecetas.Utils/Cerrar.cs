using System;
using Android.App;

namespace cl.Common.RikiiRecetas.Utils
{
    /// <summary>
    /// Cerrar.
    /// </summary>
    public class Cerrar
    {
        /// <summary>
        /// Closes the application.
        /// </summary>
        public static void closeApplication(Activity actividad)
        {
            var activity = actividad;
            activity.FinishAffinity();
        }
    }
}
