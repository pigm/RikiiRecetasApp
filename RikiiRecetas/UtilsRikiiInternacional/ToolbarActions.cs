using System;
using Android.Content;

namespace RikiiRecetas.UtilsRikiiInternacional
{
    /// <summary>
    /// Toolbar actions.
    /// </summary>
    public class ToolbarActions
    {
        /// <summary>
        /// Actions the place.
        /// </summary>
        /// <param name="activity">Activity.</param>
        public static void actionPlace(Android.App.Activity activity)
        {
            activity.Finish();
        }

        /// <summary>
        /// Actions the favorite.
        /// </summary>
        /// <param name="activity">Activity.</param>
        public static void actionFavorite(Android.App.Activity activity)
        {
            Intent i = new Intent(activity, typeof(UbicacionPaisActivity));
            activity.StartActivity(i);
        }

        /// <summary>
        /// Actions the back.
        /// </summary>
        /// <param name="activity">Activity.</param>
        public static void actionBack(Android.App.Activity activity)
        {
            activity.Finish();
        }
    }
}
