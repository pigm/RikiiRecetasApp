using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.Gms.Location;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using System;
using System.Threading;
using Android.Content;
using System.Linq;
using System.Threading.Tasks;
using cl.Common.RikiiRecetas.Utils;
using RikiiRecetas.Adapter;
using RikiiRecetas.Activity;
using Android.Graphics;
using Android.Util;

namespace RikiiRecetas.Fragment
{
    public class FragmentPaises : Android.Support.V4.App.Fragment, IOnMapReadyCallback, GoogleMap.IOnMarkerClickListener, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        AdapterPais adapter;
        BottomSheetBehavior bottomSheetBehavior_suc;
        float scale;
        FragmentManager fm;      
        GoogleApiClient client;
        GoogleMap map;
        List<Marker> markerList;
        LinearLayout linear_mapa;
        LinearLayout sheet_suc;
        LinearLayout llBtnRecipe;
        LinearLayoutManager layoutManager;
        RecyclerView recycler;
        SupportMapFragment mf;
        Typeface fontRegular;
        Typeface fontBold;
        TextView lblNombreActivity;
        ViewGroup v;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RikiiRecetas.Fragment.FragmentPaises"/> class.
        /// </summary>
        /// <param name="fm">Fm.</param>
        public FragmentPaises(FragmentManager fm)
        {
            this.fm = fm;         
        }

        /// <summary>
        /// Ons the connected.
        /// </summary>
        /// <param name="connectionHint">Connection hint.</param>
        public void OnConnected(Bundle connectionHint)
        {
            return;
        }

        /// <summary>
        /// Ons the connection failed.
        /// </summary>
        /// <param name="result">Result.</param>
        public void OnConnectionFailed(ConnectionResult result)
        {
            return;
        }

        /// <summary>
        /// Ons the connection suspended.
        /// </summary>
        /// <param name="cause">Cause.</param>
        public void OnConnectionSuspended(int cause)
        {
            return;
        }

        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
        }

        /// <summary>
        /// Ons the create view.
        /// </summary>
        /// <returns>The create view.</returns>
        /// <param name="inflater">Inflater.</param>
        /// <param name="container">Container.</param>
        /// <param name="savedInstanceState">Saved instance state.</param>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            fontRegular = Typeface.CreateFromAsset(Activity.Assets, "fonts/Titillium_Web/TitilliumWeb-Regular.ttf");
            fontBold = Typeface.CreateFromAsset(Activity.Assets, "fonts/Titillium_Web/TitilliumWeb-Bold.ttf");
            scale = Context.Resources.DisplayMetrics.Density;
            int pixels = 0;
            if (v == null)
            {
                v = (ViewGroup)inflater.Inflate(Resource.Layout.paises_fragment, container, false);
            }

            lblNombreActivity = v.FindViewById<TextView>(Resource.Id.lblNombreActivity);
            lblNombreActivity.Text = "Gastronomia internacional";
            lblNombreActivity.Typeface = fontBold;
            llBtnRecipe = v.FindViewById<LinearLayout>(Resource.Id.llBtnRecipe);
            llBtnRecipe.Visibility = ViewStates.Visible;
            llBtnRecipe.Click += delegate {
                DataManager.VisualizarMisRecetas = true;
                Intent intentAHomeAgregaProducto = new Intent(Activity, typeof(RecetasListActivity));
                StartActivity(intentAHomeAgregaProducto);
            };
            sheet_suc = v.FindViewById<LinearLayout>(Resource.Id.linear_sheet_sucursales);
            linear_mapa = v.FindViewById<LinearLayout>(Resource.Id.linear_mapa);
            CoordinatorLayout.LayoutParams linearLayoutParams = new CoordinatorLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);          
            pixels = (int)(160 * scale + 0.5f);
            bottomSheetBehavior_suc = BottomSheetBehavior.From(sheet_suc);
            bottomSheetBehavior_suc.PeekHeight = pixels;            
            linearLayoutParams.SetMargins(0, 0, 0, pixels);
            linear_mapa.LayoutParameters = linearLayoutParams;
            recycler = v.FindViewById<RecyclerView>(Resource.Id.mi_recycler_view_sucursales);
            adapter = new AdapterPais(DataManager.Paises, this);
            adapter.itemClick += OnItemClick;
            recycler.SetAdapter(adapter);
            layoutManager = new LinearLayoutManager(Activity);
            recycler.SetLayoutManager(layoutManager);
            mf = (SupportMapFragment)ChildFragmentManager.FindFragmentById(Resource.Id.map_sucursales);
            mf.GetMapAsync(this);
            RetainInstance = true;
            ThreadPool.QueueUserWorkItem(o => cargaServicios());
            return v;
        }

        /// <summary>
        /// Ons the marker click.
        /// </summary>
        /// <returns><c>true</c>, if marker click was oned, <c>false</c> otherwise.</returns>
        /// <param name="marker">Marker.</param>
        public bool OnMarkerClick(Marker marker)
        {
            if (map != null)
            {
                if (DataManager.Paises != null)
                {               
                    if (ValidationUtils.GetNetworkStatus())
                    {
                        var sucursal = DataManager.Paises.Where(x => x.location.address == marker.Snippet).FirstOrDefault();
                        DataManager.paisSeleccionado = sucursal;
                        Intent intentAHomeAgregaProducto = new Intent(Activity, typeof(RecetasListActivity));
                        StartActivity(intentAHomeAgregaProducto);
                    }else{
                        //Mensaje sin conexion a internet
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Cargas the servicios.
        /// </summary>
        private void cargaServicios()
        {
            try
            {
                Activity.RunOnUiThread(async () => await saveServices());
            }
            catch (Exception ex)
            {
                Activity.Finish();
            }
        }

        /// <summary>
        /// Saves the services.
        /// </summary>
        /// <param name="sucursales">Sucursales.</param>
        public async Task saveServices()
        {
            if (map != null)
            {
                drawMarker(map);
                adapter.refreshAdapter(DataManager.Paises, map);
            }
        }

        /// <summary>
        /// Draws the marker.
        /// </summary>
        /// <param name="map2">Map2.</param>
        public void drawMarker(GoogleMap map2)
        {
            BitmapDescriptor mapIcon;

            if (map2 != null)
            {
                markerList = new List<Marker>();
                if (DataManager.Paises != null && DataManager.Paises.Count > 0)
                {
                    for (int i = 0; i < DataManager.Paises.Count; i++)
                    {
                        string pinName = DataManager.Paises[i].name.ToLower();
                        switch (pinName)
                        {
                            case ("alemania"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.alemania);
                                break;
                            case ("arabia saudita"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.arabiasaudi);
                                break;
                            case ("australia"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.australia);
                                break;
                            case ("brasil"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.brazil);
                                break;
                            case ("canadá"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.canada);
                                break;                            
                            case ("chile"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.chile);
                                break;
                            case ("china"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.china);
                                break;
                            case ("colombia"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.colombia);
                                break;
                            case ("cuba"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.cuba);
                                break;
                            case ("egipto"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.egipto);
                                break;
                            case ("estados unidos"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.estadosunidos);
                                break;
                            case ("francia"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.francia);
                                break;
                            case ("grecia"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.grecia);
                                break;
                            case ("india"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.india);
                                break;
                            case ("italia"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.italia);
                                break;
                            case ("japon"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.japon);
                                break;
                            case ("korea"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.korea);
                                break;
                            case ("méxico"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.mexico);
                                break;
                            case ("perú"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.peru);
                                break;
                            case ("portugal"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.portugal);
                                break;
                            case ("puerto rico"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.puertorico);
                                break;
                            case ("república dominicana"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.republicadominicana);
                                break;
                            case ("rusia"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.russia);
                                break;
                            case ("españa"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.spain);
                                break;
                            case ("sudáfrica"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.sudafrica);
                                break;
                            case ("tailandia"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.tailandia);
                                break;
                            case ("turquia"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.turquia);
                                break;
                            case ("uruguay"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.uruguay);
                                break;
                            case ("venezuela"):
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.venezuela);
                                break;
                            default:
                                mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.chile);
                                break;

                        }


                        if (DataManager.Paises[i].HasValidCoordinates())
                        {
                            string nombre = DataManager.Paises[i].name;

                            map2.MyLocationEnabled = true;
                            var point = new LatLng(DataManager.Paises[i].location.position.coordinates[1], DataManager.Paises[i].location.position.coordinates[0]);
                            MarkerOptions markerOpt1 = new MarkerOptions();
                            markerOpt1.SetPosition(point);
                            markerOpt1.SetTitle(nombre);
                            markerOpt1.SetIcon(mapIcon);
                            markerOpt1.SetSnippet(DataManager.Paises[i].location.address);
                            markerList.Add(map2.AddMarker(markerOpt1));


                            if (DataManager.actualLatitud != null)
                            {
                                var lat = DataManager.actualLatitud.Replace("\r", "").Replace(',', '.');
                                var lon = DataManager.actualLongitud.Replace("\r", "").Replace(',', '.');
                                var latitude = double.Parse(lat, System.Globalization.CultureInfo.InvariantCulture);
                                var longitud = double.Parse(lon, System.Globalization.CultureInfo.InvariantCulture);
                                double diff = Geolocation.
                                                         CalculateDistance(latitude, longitud, DataManager.Paises[i].location.position.coordinates[1], DataManager.Paises[i].location.position.coordinates[0]);
                                DataManager.Paises[i].UserDistance = diff;
                                DataManager.Paises[i].HasUserDistance = true;
                            }
                        }

                    }


                }
                else
                {
                    Toast.MakeText(Activity, "Lista de paises vacia", ToastLength.Short).Show();
                }
            }
        }

        /// <summary>
        /// Aplis the client build.
        /// </summary>
        public void ApliClientBuild()
        {
            client = new GoogleApiClient.Builder(Activity)
                                                        .AddConnectionCallbacks(this)
                                                        .AddOnConnectionFailedListener(this)
                                                        .AddApi(LocationServices.API)
                                                        .EnableAutoManage(Activity, this)
                                                        .Build();
        }

        /// <summary>
        /// Ons the map ready.
        /// </summary>
        /// <param name="googleMap">Google map.</param>
        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            ApliClientBuild();
            if (map != null)
            {
                LatLng location;
                map.MyLocationEnabled = true;
                var horaActual = DateTime.Now.Hour;
                if (horaActual <= 6)
                {
                    //GoogleMap noche
                    map.SetMapStyle(MapStyleOptions.LoadRawResourceStyle(this.Activity, Resource.Raw.googlemap_style_json_noche));
                }
                else if (horaActual >= 20)
                {
                    //GoogleMap noche
                    map.SetMapStyle(MapStyleOptions.LoadRawResourceStyle(this.Activity, Resource.Raw.googlemap_style_json_noche));
                }
                else
                {
                    //GoogleMap dia
                    map.SetMapStyle(MapStyleOptions.LoadRawResourceStyle(this.Activity, Resource.Raw.googlemap_style_json));
                }

                if (DataManager.actualLatitud != null)
                {
                    var lat = DataManager.actualLatitud.Replace("\r", "").Replace(',', '.');
                    var lon = DataManager.actualLongitud.Replace("\r", "").Replace(',', '.');
                    var latitude = double.Parse(lat, System.Globalization.CultureInfo.InvariantCulture);
                    var longitud = double.Parse(lon, System.Globalization.CultureInfo.InvariantCulture);
                    location = new LatLng(latitude, longitud);

                    map.MyLocationButtonClick += delegate
                    {
                        map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(location, 1.0f));
                    };

                }
                else
                {
                    location = new LatLng(-33.53058, -70.674187);
                    map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(location, 1.0f));
                }
                map.MoveCamera(CameraUpdateFactory.NewLatLng(location));
                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(location);
                builder.Zoom(1);
                builder.Bearing(0);
                CameraPosition cameraPosition = builder.Build();
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                map.MoveCamera(cameraUpdate);
                map.SetOnMarkerClickListener(this);
            }
            else
            {
                Toast.MakeText(Activity, "No se ha podido cargar el mapa", ToastLength.Short).Show();
            }
        }



        //EVENTO CLICK ITEMS RECYCLERVIEW
        /// <summary>
        /// Ons the item click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnItemClick(object sender, List<Object> e)
        {
            var descripcionPais = (string)e[0];
            if (DataManager.Paises != null)
            {
                if (ValidationUtils.GetNetworkStatus())
                {
                    var sucursal = DataManager.Paises.Where(x => x.name == descripcionPais).FirstOrDefault();
                    DataManager.paisSeleccionado = sucursal;
                    Intent intentAHomeAgregaProducto = new Intent(Activity, typeof(RecetasListActivity));
                    StartActivity(intentAHomeAgregaProducto);
                }
                else
                {
                    //Mensaje sin conexion a internet
                }
            }
        }

        /// <summary>
        /// Ons the start.
        /// </summary>
        /*
        public override void OnStart()
        {
            base.OnStart();
            if (client == null)
                client.Connect();
        }*/

        /// <summary>
        /// Ons the stop.
        /// </summary>
        public override void OnStop()
        {           
            base.OnStop();
            try
            {
                if (client != null && client.IsConnected)
                {
                    client.StopAutoManage(this.Activity);
                    client.Disconnect();
                }
            }
            catch (Exception)
            {
                if (client == null)
                    client.Connect();
            }

        }
    }
}