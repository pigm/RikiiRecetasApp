using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Widget;
using Android.OS;
using cl.Common.RikiiRecetas.Service.Delegate;
using cl.Common.RikiiRecetas.Models.Modelo;
using cl.Common.RikiiRecetas.Utils;

namespace RikiiRecetas
{
    /// <summary>
    /// Main activity.
    /// </summary>
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme.Splash", MainLauncher = true, ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize |
             Android.Content.PM.ConfigChanges.Orientation,
             ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : Android.App.Activity, ILocationListener, ActivityCompat.IOnRequestPermissionsResultCallback
    {
        LocationManager locMgr;
        const int RequestLocationId = 0;

        /// <summary>
        /// The permissions location.
        /// </summary>
        readonly string[] PermissionsLocation =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };

        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main_activity);
            StreamReader reader = new StreamReader(Assets.Open("BD/PaisesBD.json"));
            string json = reader.ReadToEnd();
            var paises = await ServiceDelegate.Instance.GetPaises(json);
            if (paises.Success)
            {
                DataManager.Paises = paises.Response as List<Pais>;
                try
                {
                    OpenMaps(false);
                }
                catch (Exception ex)
                {
                    dialogWarning(ConstantesApp.APP_NAME
                                  , ConstantesApp.MSJ_ERROR_INESPERADO
                                  , ConstantesApp.BTN_ACEPTAR);
                }
            }
            else
            {
                dialogWarning(ConstantesApp.APP_NAME
                              , ConstantesApp.MSJ_ERROR_INESPERADO
                              , ConstantesApp.BTN_ACEPTAR);
            }
        }

        /// <summary>
        /// Opens the maps.
        /// </summary>
        /// <param name="activePais">If set to <c>true</c> active local.</param>
        public void OpenMaps(bool activePais = false)
        {
            locMgr = GetSystemService(Context.LocationService) as LocationManager;

            if (locMgr.AllProviders.Contains(LocationManager.GpsProvider)
                && locMgr.IsProviderEnabled(LocationManager.GpsProvider))
            {
                if (ActivityCompat.CheckSelfPermission(this, Android.Manifest.Permission.AccessFineLocation) ==
                    Permission.Granted &&
                    ActivityCompat.CheckSelfPermission(this, Android.Manifest.Permission.AccessCoarseLocation) ==
                    Permission.Granted)
                {
                    OpenSucursal(activePais);
                }
                else
                {
                    RequestPermissions(
                        new String[] { Android.Manifest.Permission.AccessCoarseLocation, Android.Manifest.Permission.AccessFineLocation }
                        , 0
                    );

                    OpenMaps(activePais);
                }
            }
            else
            {
                dialogWarning(ConstantesApp.APP_NAME
                              , ConstantesApp.SIN_GPS_ACCESS
                              , ConstantesApp.BTN_ACEPTAR);            
            }
        }

        /// <summary>
        /// Opens the sucursal.
        /// </summary>
        public async void OpenSucursal(bool seleccionada)
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(15));
                Console.WriteLine("Position Status: {0}", position.Timestamp);
                Console.WriteLine("Position Latitude: {0}", position.Latitude);
                Console.WriteLine("Position Longitude: {0}", position.Longitude);

                string lat = position.Latitude.ToString().Replace(',', '.');
                string lon = position.Longitude.ToString().Replace(',', '.');

                geolocalizacionPaises(lat, lon);
            }
            catch (System.Threading.Tasks.TaskCanceledException ex)
            {
                geolocalizacionPaises("-33.537091", "-70.5716911");
            }
        }

        /// <summary>
        /// Geolocalizacions the walmart.
        /// </summary>
        /// <param name="lat">Lat.</param>
        /// <param name="lon">Lon.</param>
        async void geolocalizacionPaises(string lat, string lon)
        {
            DataManager.actualLatitud = lat;
            DataManager.actualLongitud = lon;
          
            Intent i = new Intent(this, typeof(UbicacionPaisActivity));
            StartActivity(i);           
        }

        /// <summary>
        /// Dialogs the warning.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="message">Message.</param>
        /// <param name="positiveButton">Positive button.</param>
        public void dialogWarning(string title, string message, string positiveButton)
        {
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
            builder.SetTitle(title);
            builder.SetMessage(message);
            builder.SetCancelable(false);
            builder.SetPositiveButton(positiveButton, delegate {
                Cerrar.closeApplication(this);
            });
            builder.Show();
        }
        /// <summary>
        /// Ons the location changed.
        /// </summary>
        /// <param name="location">Location.</param>
        public void OnLocationChanged(Android.Locations.Location location)
        {
            return;
        }

        /// <summary>
        /// Ons the provider disabled.
        /// </summary>
        /// <param name="provider">Provider.</param>
        public void OnProviderDisabled(string provider)
        {
            return;
        }

        /// <summary>
        /// Ons the provider enabled.
        /// </summary>
        /// <param name="provider">Provider.</param>
        public void OnProviderEnabled(string provider)
        {
            return;
        }

        /// <summary>
        /// Ons the status changed.
        /// </summary>
        /// <param name="provider">Provider.</param>
        /// <param name="status">Status.</param>
        /// <param name="extras">Extras.</param>
        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            return;
        }

        /// <summary>
        /// Gets the location compat async.
        /// </summary>
        /// <returns>The location compat async.</returns>
        async Task GetLocationCompatAsync()
        {
            const string permission = Manifest.Permission.AccessFineLocation;
            if (ContextCompat.CheckSelfPermission(this, permission) == (int)Permission.Granted)
            {
                return;
            }

            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, permission))
            {
                var dialog = new Android.Support.V7.App.AlertDialog.Builder(this)
                .SetTitle("Acción requerida")
                                            .SetMessage("Esta aplicación necesita acceso a la localización del dispositivo")
                .SetPositiveButton("Aceptar", (senderAlert, args) =>
                {
                    RequestPermissions(PermissionsLocation, RequestLocationId);
                })
                .Show();
                return;
            }
            RequestPermissions(PermissionsLocation, RequestLocationId);
        }

        /// <summary>
        /// Ons the request permissions result.
        /// </summary>
        /// <param name="requestCode">Request code.</param>
        /// <param name="permissions">Permissions.</param>
        /// <param name="grantResults">Grant results.</param>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == (int)Permission.Granted)
                        {
                            var toast = Toast.MakeText(this, "Permisos asignados",
                                                       ToastLength.Short);
                            toast.Show();
                        }
                        else
                        {
                            var toast = Toast.MakeText(this, "Los permisos no fueron otorgados, la aplicación no se puede utilizar",
                                                      ToastLength.Long);
                            toast.Show();
                            closeApplication();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Closes the application.
        /// </summary>
        public void closeApplication()
        {
            var activity = this;
            activity.FinishAffinity();
        }
    }
}