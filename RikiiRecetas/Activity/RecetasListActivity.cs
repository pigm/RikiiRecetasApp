
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using cl.Common.RikiiRecetas.Models.Modelo;
using cl.Common.RikiiRecetas.Service.Delegate;
using cl.Common.RikiiRecetas.Service.Result;
using cl.Common.RikiiRecetas.Utils;
using Com.Airbnb.Lottie;
using RikiiRecetas.Adapter;
using RikiiRecetas.UtilsRikiiInternacional;

namespace RikiiRecetas.Activity
{
    [Activity(Label = "Lista Recetas", Theme = "@style/ThemeNoActionBarRed", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize |
             Android.Content.PM.ConfigChanges.Orientation,
             ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class RecetasListActivity : AppCompatActivity
    {
        AdapterListaRecetas adapter;
        ImageView btnFilter;
        LinearLayoutManager layoutManager;
        LottieAnimationView animation_view;
        RecyclerView recyclerView;
        LinearLayout llBtnBack, llBtnPlace, llBtnFilter, llSinData;
        TextView lblNombreActivity, lblTituloPais, lblInstruccionesListaRecetas, lblSinRecetas, lblTituloSinRecetas;

        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.recetas_list_activity);
            var fontRegular = Typeface.CreateFromAsset(Assets, "fonts/Titillium_Web/TitilliumWeb-Regular.ttf");
            var fontBold = Typeface.CreateFromAsset(Assets, "fonts/Titillium_Web/TitilliumWeb-Bold.ttf");
            lblNombreActivity = FindViewById<TextView>(Resource.Id.lblNombreActivity);
            lblNombreActivity.Typeface = fontBold;
            lblTituloPais = FindViewById<TextView>(Resource.Id.lblTituloPais);
            if (DataManager.VisualizarMisRecetas){
                lblNombreActivity.Text = "Mis recetas";
                lblTituloPais.Text = "Mís recetas típicas";
            }else{
                lblNombreActivity.Text = DataManager.paisSeleccionado.name;
                lblTituloPais.Text = "Recetas típicas de " + DataManager.paisSeleccionado.name;
            }
            lblTituloPais.Typeface = fontBold;
            animation_view = FindViewById<LottieAnimationView>(Resource.Id.animation_view);
            lblInstruccionesListaRecetas = FindViewById<TextView>(Resource.Id.lblInstruccionesListaRecetas);
            lblInstruccionesListaRecetas.Typeface = fontRegular;
            btnFilter = FindViewById<ImageView>(Resource.Id.btnFilter);
            llBtnPlace = FindViewById<LinearLayout>(Resource.Id.llBtnPlace);
            llBtnPlace.Visibility = ViewStates.Visible;
            llBtnPlace.Click += delegate { ToolbarActions.actionPlace(this); DataManager.VisualizarMisRecetas = false; };
            llBtnFilter = FindViewById<LinearLayout>(Resource.Id.llBtnFilter);
            llBtnFilter.Visibility = ViewStates.Visible;
            llBtnFilter.Click += LlBtnFilter_Click;
            llBtnBack = FindViewById<LinearLayout>(Resource.Id.llBtnAtras);
            llBtnBack.Visibility = ViewStates.Visible;
            llBtnBack.Click += delegate { Finish(); DataManager.VisualizarMisRecetas = false; };

            layoutManager = new LinearLayoutManager(this);
            recyclerView = FindViewById<RecyclerView>(Resource.Id.rvListaRecetas);
            llSinData = FindViewById<LinearLayout>(Resource.Id.llSinData);
            lblTituloSinRecetas = FindViewById<TextView>(Resource.Id.lblTituloSinRecetas);
            lblTituloSinRecetas.Typeface = fontBold;
            lblSinRecetas = FindViewById<TextView>(Resource.Id.lblSinRecetas);
            lblSinRecetas.Typeface = fontRegular;
        }

        /// <summary>
        /// Ons the resume.
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            animation_view.Visibility = ViewStates.Visible;
            recyclerView.Visibility = ViewStates.Gone;
            llSinData.Visibility = ViewStates.Gone;
            Task startupwork = new Task(() =>
            {
                Task.Delay(5000).Wait();

            });
            startupwork.ContinueWith(t =>
            {
                PrepareRecycler();
                animation_view.Visibility = ViewStates.Gone;
            }, TaskScheduler.FromCurrentSynchronizationContext());

            startupwork.Start();
        }

        /// <summary>
        /// Prepares the recycler.
        /// </summary>
        async void PrepareRecycler()
        {
            StreamReader reader;
            string json;
            List<Receta> listaRecetas = null;
            if (DataManager.paisSeleccionado != null)
            {
                if (DataManager.paisSeleccionado.name.Equals("Chile"))
                {
                    reader = new StreamReader(Assets.Open("BD/RecetasChileBD.json"));
                    json = reader.ReadToEnd();
                    var recetas = await ServiceDelegate.Instance.GetRecetas(json);
                    listaRecetas = recetas.Response as List<Receta>;
                }
                else if (DataManager.paisSeleccionado.name.Equals("Estados Unidos"))
                {
                    reader = new StreamReader(Assets.Open("BD/RecetasEstadosUnidosBD.json"));
                    json = reader.ReadToEnd();
                    var recetas = await ServiceDelegate.Instance.GetRecetas(json);
                    listaRecetas = recetas.Response as List<Receta>;
                }
                else if (DataManager.paisSeleccionado.name.Equals("México"))
                {
                    reader = new StreamReader(Assets.Open("BD/RecetasMexicoBD.json"));
                    json = reader.ReadToEnd();
                    var recetas = await ServiceDelegate.Instance.GetRecetas(json);
                    listaRecetas = recetas.Response as List<Receta>;
                }
            }
            else if (DataManager.VisualizarMisRecetas)
            {
                reader = new StreamReader(Assets.Open("BD/RecetasMexicoBD.json"));
                json = reader.ReadToEnd();
                var recetas = await ServiceDelegate.Instance.GetRecetas(json);
                listaRecetas = recetas.Response as List<Receta>;
            }

            if (listaRecetas != null)
            {
                if (listaRecetas.Count >= 1)
                {
                    recyclerView.Visibility = ViewStates.Visible;
                    llSinData.Visibility = ViewStates.Gone;
                    adapter = new AdapterListaRecetas(listaRecetas, this);
                    adapter.itemClick += OnItemClick;
                    recyclerView.SetAdapter(adapter);
                    recyclerView.SetLayoutManager(layoutManager);
                    llBtnFilter.Visibility = ViewStates.Visible;
                }
                else
                {
                    recyclerView.Visibility = ViewStates.Gone;
                    llSinData.Visibility = ViewStates.Visible;
                    llBtnFilter.Visibility = ViewStates.Gone;
                }
            }
            else
            {
                recyclerView.Visibility = ViewStates.Gone;
                llSinData.Visibility = ViewStates.Visible;
                llBtnFilter.Visibility = ViewStates.Gone;
            }

        }

        /// <summary>
        /// Lls the button filter click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void LlBtnFilter_Click(object sender, EventArgs e)
        {
            /*if (!DataManager.estadoFiltro)
            {
                DataManager.estadoFiltro = true;
                btnFilter.SetImageResource(Resource.Drawable.ic_confilter);
                Toast.MakeText(ApplicationContext, "con filtro", ToastLength.Short).Show();
            }else{
                DataManager.estadoFiltro = false;
                btnFilter.SetImageResource(Resource.Drawable.ic_sinfilter);
                Toast.MakeText(ApplicationContext, "sin filtro", ToastLength.Short).Show();
            }*/
            FiltrosDialogo.ViewFilters(this);
        }

        //EVENTO CLICK ITEMS RECYCLERVIEW
        /// <summary>
        /// Ons the item click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnItemClick(object sender, List<Object> e)
        {
            Intent intentAAgregaProducto = new Intent(this, typeof(RecetaSeleccionadaActivity));
            StartActivity(intentAAgregaProducto, ActivityOptions.MakeSceneTransitionAnimation(this).ToBundle());
        }

        /// <summary>
        /// Ons the back pressed.
        /// </summary>
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            DataManager.VisualizarMisRecetas = false;
        }
    }
}