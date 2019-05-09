using System;
namespace cl.Common.RikiiRecetas.Utils
{
    /// <summary>
    /// Geolocation.
    /// </summary>
    public class Geolocation
    {
        /// <summary>
        /// Calculates the distance.
        /// </summary>
        /// <returns>The distance.</returns>
        /// <param name="sLatitude">S latitude.</param>
        /// <param name="sLongitude">S longitude.</param>
        /// <param name="eLatitude">E latitude.</param>
        /// <param name="eLongitude">E longitude.</param>
        public static double CalculateDistance(double sLatitude, double sLongitude, double eLatitude,
                               double eLongitude)
        {
            var sLatitudeRadians = sLatitude * (Math.PI / 180.0);
            var sLongitudeRadians = sLongitude * (Math.PI / 180.0);
            var eLatitudeRadians = eLatitude * (Math.PI / 180.0);
            var eLongitudeRadians = eLongitude * (Math.PI / 180.0);

            var dLongitude = eLongitudeRadians - sLongitudeRadians;
            var dLatitude = eLatitudeRadians - sLatitudeRadians;

            var result1 = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                          Math.Cos(sLatitudeRadians) * Math.Cos(eLatitudeRadians) *
                          Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

            var result2 = 6371.0 * 2.0 *
                          Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1));

            return result2;
        }
    }
}