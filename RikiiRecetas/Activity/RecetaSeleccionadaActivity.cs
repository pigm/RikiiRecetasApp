using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using cl.Common.RikiiRecetas.Models.Modelo;
using cl.Common.RikiiRecetas.Utils;

namespace RikiiRecetas
{
    /// <summary>
    /// Receta seleccionada activity.
    /// </summary>
    [Activity(Label = "Receta Seleccionada", Theme = "@style/ThemeNoActionBarRed", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize |
             Android.Content.PM.ConfigChanges.Orientation,
             ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class RecetaSeleccionadaActivity : AppCompatActivity
    {
        FloatingActionButton btnAgregarFovorito, btnCompartirReceta;
        CoordinatorLayout clRecetaSeleccionada;
        ImageView imgRecetaSeleccionada;
        TextView lblTituloReceta, lblPreparacion, lblTituloPreparacion, lblIngredientes, lblTituloIngredientes;
        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.receta_seleccionada_activity);
            var fontRegular = Typeface.CreateFromAsset(Assets, "fonts/Titillium_Web/TitilliumWeb-Regular.ttf");
            var fontBold = Typeface.CreateFromAsset(Assets, "fonts/Titillium_Web/TitilliumWeb-Bold.ttf");
            var charmonmanBold = Typeface.CreateFromAsset(Assets, "fonts/Charmonman/Charmonman-Bold.ttf");
            clRecetaSeleccionada = FindViewById<CoordinatorLayout>(Resource.Id.clRecetaSeleccionada);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = DataManager.RecetaSeleccionada.nombre;
            SetSupportActionBar(toolbar);
            if (SupportActionBar != null)
            {
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            }
            lblTituloReceta = FindViewById<TextView>(Resource.Id.lblTituloReceta);
            lblTituloReceta.Typeface = charmonmanBold;
            lblTituloIngredientes = FindViewById<TextView>(Resource.Id.lblTituloIngredientes);
            lblTituloIngredientes.Typeface = fontBold;
            lblIngredientes = FindViewById<TextView>(Resource.Id.lblIngredientes);
            lblIngredientes.Typeface = fontRegular;
            var listaIngredientes = DataManager.RecetaSeleccionada.insumos.ingredientes;
            lblIngredientes.Text = getIngredientes(listaIngredientes);
            lblTituloPreparacion = FindViewById<TextView>(Resource.Id.lblTituloPreparacion);
            lblTituloPreparacion.Typeface = fontBold;
            lblPreparacion = FindViewById<TextView>(Resource.Id.lblPreparacion);
            lblPreparacion.Typeface = fontRegular;
            lblPreparacion.Text = DataManager.RecetaSeleccionada.preparacion;
            imgRecetaSeleccionada = FindViewById<ImageView>(Resource.Id.imgRecetaSeleccionada);
            if (DataManager.RecetaSeleccionada.imagen != null)
            {
                Bitmap imagen = Base64ToBitmap(DataManager.RecetaSeleccionada.imagen);
                imgRecetaSeleccionada.SetImageBitmap(imagen);
            }
            btnAgregarFovorito = FindViewById<FloatingActionButton>(Resource.Id.btnAgregarFovorito);
            btnAgregarFovorito.Click += BtnAgregarFovorito_Click;
            btnCompartirReceta = FindViewById<FloatingActionButton>(Resource.Id.btnCompartirReceta);
            btnCompartirReceta.Click += BtnCompartirReceta_Click;
        }

        /// <summary>
        /// Ons the options item selected.
        /// </summary>
        /// <returns><c>true</c>, if options item selected was oned, <c>false</c> otherwise.</returns>
        /// <param name="item">Item.</param>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId != Android.Resource.Id.Home)
                return base.OnOptionsItemSelected(item);
            Finish();
            return true;
        }

        /// <summary>
        /// Buttons the agregar fovorito click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void BtnAgregarFovorito_Click(object sender, EventArgs e)
        {
            var existeReceta = DataManager.RealmInstance.All<RecetaFavorita>().Where(x => x.id == DataManager.RecetaSeleccionada.id && x.pais == DataManager.RecetaSeleccionada.pais);
            if (!existeReceta.Any())
            {
                Snackbar.Make(clRecetaSeleccionada, "¿Estás seguro de agregar esta receta a favoritos?", Snackbar.LengthLong)
                            .SetAction("OK", (view) => 
                { 
                    RecetaFavorita recetaFavorita = new RecetaFavorita();
                    recetaFavorita.id = DataManager.RecetaSeleccionada.id;
                    recetaFavorita.nombre = DataManager.RecetaSeleccionada.nombre;
                    recetaFavorita.tiempo = DataManager.RecetaSeleccionada.tiempo;
                    recetaFavorita.cantidad = DataManager.RecetaSeleccionada.cantidad;
                    recetaFavorita.dificultad = DataManager.RecetaSeleccionada.dificultad;
                    recetaFavorita.pais = DataManager.RecetaSeleccionada.pais;
                    recetaFavorita.imagen = DataManager.RecetaSeleccionada.imagen;
                    recetaFavorita.isFavorita = true;

                    DataManager.RealmInstance.Write(() =>
                    {
                        DataManager.RealmInstance.Add(recetaFavorita);

                    });
                    Snackbar.Make(clRecetaSeleccionada, "Receta agregada a favoritos", Snackbar.LengthShort).SetAction("", (viewmsj) => { }).Show();
                }).Show();
            }
            else
            {
                var dataUp = DataManager.RealmInstance.All<RecetaFavorita>().
                                           Where(f => f.id == DataManager.RecetaSeleccionada.id && f.pais == DataManager.RecetaSeleccionada.pais).FirstOrDefault();
                if (dataUp.isFavorita)
                {
                    Snackbar.Make(clRecetaSeleccionada, "¿Estás seguro de eliminar esta receta de favoritos?", Snackbar.LengthLong)
                            .SetAction("OK", (view) =>
                    {
                    DataManager.RealmInstance.Write(() => {
                            dataUp.isFavorita = false;
                        });
                        Snackbar.Make(clRecetaSeleccionada, "Receta eliminada de favoritos", Snackbar.LengthShort).SetAction("", (viewmsj) => { }).Show();
                    }).Show();
                }
                else
                {
                    Snackbar.Make(clRecetaSeleccionada, "¿Estás seguro de agregar esta receta a favoritos?", Snackbar.LengthLong)
                            .SetAction("OK", (view) =>
                    {
                        DataManager.RealmInstance.Write(() => {
                            dataUp.isFavorita = true;
                        });
                        Snackbar.Make(clRecetaSeleccionada, "Receta agregada a favoritos", Snackbar.LengthShort).SetAction("", (viewmsj) => { }).Show();
                    }).Show();
                }

            }
        }

        /// <summary>
        /// Buttons the compartir receta click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void BtnCompartirReceta_Click(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(LottieActivity));
            StartActivity(i);
            Toast.MakeText(ApplicationContext, "al lottie", ToastLength.Short).Show();
        }


        /// <summary>
        /// Gets the ingredientes.
        /// </summary>
        /// <returns>The ingredientes.</returns>
        /// <param name="listaIngredientes">Lista ingredientes.</param>
        String getIngredientes(List<string> listaIngredientes)
        {

            string acum = string.Empty;
            foreach (var aux in listaIngredientes)
            {
                acum = acum + aux + " \n";
            }
            return acum;
        }

        /// <summary>
        /// Base64s to bitmap.
        /// </summary>
        /// <returns>The to bitmap.</returns>
        /// <param name="base64String">Base64 string.</param>
        public Bitmap Base64ToBitmap(String base64String)
        {
            byte[] imageAsBytes = Base64.Decode(base64String, Base64Flags.Default);
            return BitmapFactory.DecodeByteArray(imageAsBytes, 0, imageAsBytes.Length);
        }
    }
}
