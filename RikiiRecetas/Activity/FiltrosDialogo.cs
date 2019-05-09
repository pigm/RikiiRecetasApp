using Android.App;
using Android.OS;

namespace RikiiRecetas
{
    /// <summary>
    /// Ingreso usuario responsable activity.
    /// </summary>
    [Activity(Label = "Filtros", Theme = "@style/ThemeNoActionBarTranslucent", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize |
             Android.Content.PM.ConfigChanges.Orientation,
             ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class FiltrosDialogo : Android.App.Activity
    {
        static Dialog customDialog = null;

        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.filtros_dialog);
        }

        /// <summary>
        /// Views the formulario user.
        /// </summary>
        /// <param name="activity">Activity.</param>
        public static void ViewFilters(Android.App.Activity activity)
        {
            customDialog = new Dialog(activity, Resource.Style.Theme_Dialog_Translucent);
            customDialog.SetCancelable(true);
            customDialog.SetContentView(Resource.Layout.filtros_dialog);
            customDialog.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            customDialog.Show();
        }
    }
}