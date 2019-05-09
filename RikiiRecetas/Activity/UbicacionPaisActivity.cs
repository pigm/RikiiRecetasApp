
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using cl.Common.RikiiRecetas.Utils;
using RikiiRecetas.Fragment;

namespace RikiiRecetas
{
    /// <summary>
    /// Ubicacion pais activity.
    /// </summary>
    [Activity(Label = "¿Gastronomia internacional?", Theme = "@style/ThemeNoActionBarRed", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize |
             Android.Content.PM.ConfigChanges.Orientation,
             ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class UbicacionPaisActivity : AppCompatActivity
    {
        Android.Support.V4.App.FragmentTransaction ft;       
        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ubicacion_pais_activity);
            ft = SupportFragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.linear_wrapper_mapa, new FragmentPaises(SupportFragmentManager));
            ft.Commit();
        }

        /// <summary>
        /// Ons the back pressed.
        /// </summary>
        public override void OnBackPressed()
        {
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
            builder.SetTitle(ConstantesApp.APP_NAME);
            builder.SetMessage(ConstantesApp.MENSAJE_EXIT);
            builder.SetPositiveButton(ConstantesApp.BTN_ACEPTAR, delegate {
                Cerrar.closeApplication(this);
            });
            builder.SetNegativeButton(ConstantesApp.BTN_CANCELAR, delegate { });
            builder.Show();
        }
    }
}
