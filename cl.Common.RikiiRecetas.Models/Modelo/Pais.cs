using System;
namespace cl.Common.RikiiRecetas.Models.Modelo
{
    /// <summary>
    /// Sucursal.
    /// </summary>
    public class Pais
    {
        public int id { get; set; }
        public Location location { get; set; }
        public string format { get; set; }
        public string name { get; set; }
        public string longitud { get; set; }
        public string latitud { get; set; }
        public double UserDistance { get; set; }
        public bool HasUserDistance { get; set; }

        /// <summary>
        /// Gets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude
        {
            get
            {
                if (location.position.coordinates != null)
                {
                    var longitude = location.position.coordinates[0];

                    return longitude;
                }
                return 1000;
            }
        }

        /// <summary>
        /// Gets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude
        {
            get
            {
                if (location.position.coordinates != null)
                {
                    var latitude = location.position.coordinates[1];
                    return latitude;
                }
                return 1000;
            }
        }

        /// <summary>
        /// Hases the valid coordinates.
        /// </summary>
        /// <returns><c>true</c>, if valid coordinates was hased, <c>false</c> otherwise.</returns>
        public bool HasValidCoordinates()
        {
            double lat = Latitude;
            double lon = Longitude;
            if (lat >= -90 && lat <= 90 && lon >= -180 && lon <= 180)
            {
                return true;
            }
            return false;
        }
    }
}
