using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using cl.Common.RikiiRecetas.Models.Modelo;
using cl.Common.RikiiRecetas.Utils;

namespace RikiiRecetas.Adapter
{
    public class AdapterListaRecetas : RecyclerView.Adapter
    {
        AppCompatActivity appCompatActivity;
        List<Receta> list;
        List<Receta> originList;
        List<object> lproducto;
        public event EventHandler<List<Object>> itemClick;
        Typeface fontRegular;
        Typeface fontBold;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TrazabilidadCarnes.Adapters.AdapterSucursal"/> class.
        /// </summary>
        /// <param name="lista">Lista.</param>
        /// <param name="appCompatActivity">Fragment.</param>
        public AdapterListaRecetas(List<Receta> lista, AppCompatActivity appCompatActivity)
        {
            fontRegular = Typeface.CreateFromAsset(appCompatActivity.Assets, "fonts/Titillium_Web/TitilliumWeb-Regular.ttf");
            fontBold = Typeface.CreateFromAsset(appCompatActivity.Assets, "fonts/Titillium_Web/TitilliumWeb-Bold.ttf");
            list = lista;
            originList = lista;
            this.appCompatActivity = appCompatActivity;
        }

        /// <summary>
        /// Gets the item count.
        /// </summary>
        /// <value>The item count.</value>
        public override int ItemCount
        {
            get
            {
                return list.Count;
            }
        }

        /// <summary>
        /// Sucursal view holder.
        /// </summary>
        public class RecetaEnListaViewHolder : RecyclerView.ViewHolder
        {

            public View mView;
            public TextView lblNombreReceta { get; private set; }
            public TextView lblDificultadReceta { get; private set; }  
            public TextView lblTiempoReceta { get; private set; }
            public LinearLayout fila_receta_selector { get; private set; }   
            public ImageView imgIdentificacionReceta { get; private set; }
            public ImageView imgRecetaFavoritaSeleccionada { get; private set; }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:TrazabilidadCarnes.Adapters.AdapterSucursal.SucursalViewHolder"/> class.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="listener">Listener.</param>
        public RecetaEnListaViewHolder(View view, Action<List<Object>> listener) : base(view)
            {
                imgRecetaFavoritaSeleccionada = view.FindViewById<ImageView>(Resource.Id.imgRecetaFavoritaSeleccionada);
                imgIdentificacionReceta = view.FindViewById<ImageView>(Resource.Id.imgIdentificacionReceta);
                fila_receta_selector = view.FindViewById<LinearLayout>(Resource.Id.fila_receta_selector);
                lblNombreReceta = view.FindViewById<TextView>(Resource.Id.lblNombreReceta);
                lblDificultadReceta = view.FindViewById<TextView>(Resource.Id.lblDificultadReceta);
                lblTiempoReceta = view.FindViewById<TextView>(Resource.Id.lblTiempoReceta);
            }
        }

        /// <summary>
        /// Ons the bind view holder.
        /// </summary>
        /// <param name="holder">Holder.</param>
        /// <param name="position">Position.</param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RecetaEnListaViewHolder miholder = holder as RecetaEnListaViewHolder;
            LinearLayout view = miholder.fila_receta_selector;
            Receta receta = list[position];
            if (receta.imagen != null){
                Bitmap imagen = Base64ToBitmap(receta.imagen);
                miholder.imgIdentificacionReceta.SetImageBitmap(imagen);
            }              
            miholder.lblNombreReceta.Text = receta.nombre;
            miholder.lblDificultadReceta.Text = "Dificultad: "+receta.dificultad;
            miholder.lblTiempoReceta.Text = receta.tiempo;
            miholder.lblNombreReceta.Typeface = fontBold;
            miholder.lblDificultadReceta.Typeface = fontRegular;
            miholder.lblTiempoReceta.Typeface = fontRegular;
            miholder.imgRecetaFavoritaSeleccionada.Visibility = ViewStates.Invisible;
            var realm = DataManager.RealmInstance;
            if (realm == null)
            {
                return;
            }
            else {
                var existeReceta = DataManager.RealmInstance.All<RecetaFavorita>().Where(x => x.id == receta.id && x.pais == receta.pais).FirstOrDefault();
                if (existeReceta != null)
                {
                    if (existeReceta.isFavorita)
                    {
                        miholder.imgRecetaFavoritaSeleccionada.Visibility = ViewStates.Visible;
                    }
                }
            }
            
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

        /// <summary>
        /// Ons the create view holder.
        /// </summary>
        /// <returns>The create view holder.</returns>
        /// <param name="parent">Parent.</param>
        /// <param name="viewType">View type.</param>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View card = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.row_receta_selector, parent, false);
            RecetaEnListaViewHolder viewholder = new RecetaEnListaViewHolder(card, onClick);

            viewholder.fila_receta_selector.Click += (sender, e) =>
            {
                Receta recetaSeleccionada = list[viewholder.AdapterPosition];
                lproducto = new List<object>();
                lproducto.Add(recetaSeleccionada.id);
                lproducto.Add(recetaSeleccionada.nombre);
                lproducto.Add(recetaSeleccionada.insumos);
                lproducto.Add(recetaSeleccionada.tiempo);
                lproducto.Add(recetaSeleccionada.cantidad);
                lproducto.Add(recetaSeleccionada.preparacion);
                lproducto.Add(recetaSeleccionada.dificultad);
                lproducto.Add(recetaSeleccionada.pais);
                lproducto.Add(recetaSeleccionada.imagen);          

                appCompatActivity.FinishActivity(100);
                itemClick(sender, lproducto);
                DataManager.RecetaSeleccionada = recetaSeleccionada;
            };
            return viewholder;
        }

        /// <summary>
        /// Ons the click.
        /// </summary>
        /// <param name="locationName">Location name.</param>
        void onClick(List<Object> locationName)
        {
            Intent intentAHomeAgregaProducto = new Intent(appCompatActivity, typeof(RecetaSeleccionadaActivity));
            appCompatActivity.StartActivity(intentAHomeAgregaProducto);
        }
    }
}
