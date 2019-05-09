using System;
using System.Collections.Generic;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using cl.Common.RikiiRecetas.Models.Modelo;
using cl.Common.RikiiRecetas.Utils;
using RikiiRecetas.Activity;
using RikiiRecetas.Fragment;

namespace RikiiRecetas.Adapter
{
    /// <summary>
    /// Adapter sucursal.
    /// </summary>
    public class AdapterPais : RecyclerView.Adapter
    {
        List<Pais> list;
        List<Pais> originList;
        List<object> lNombreLatLong;
        public event EventHandler<List<Object>> itemClick;
        Typeface fontRegular;
        Typeface fontBold;
        FragmentPaises fragment;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RikiiRecetas.Adapter.AdapterPais"/> class.
        /// </summary>
        /// <param name="lista">Lista.</param>
        /// <param name="fragment">Fragment.</param>
        public AdapterPais(List<Pais> lista, FragmentPaises fragment)
        {
            fontRegular = Typeface.CreateFromAsset(fragment.Activity.Assets, "fonts/Titillium_Web/TitilliumWeb-Regular.ttf");
            fontBold = Typeface.CreateFromAsset(fragment.Activity.Assets, "fonts/Titillium_Web/TitilliumWeb-Bold.ttf");
            list = lista;
            originList = lista;
            this.fragment = fragment;
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
        public class PaisViewHolder : RecyclerView.ViewHolder
        {

            public View mView;
            public TextView mNombre { get; private set; }
            public TextView mDistancia { get; private set; }
            public ImageView mImage { get; set; }
            public LinearLayout row { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="T:RikiiRecetas.Adapter.AdapterPais.PaisViewHolder"/> class.
            /// </summary>
            /// <param name="view">View.</param>
            /// <param name="listener">Listener.</param>
            public PaisViewHolder(View view, Action<List<Object>> listener) : base(view)
            {
                mNombre = view.FindViewById<TextView>(Resource.Id.nombre_sucursal);
                mDistancia = view.FindViewById<TextView>(Resource.Id.distancia_sucursal);
                mImage = view.FindViewById<ImageView>(Resource.Id.image_sucursal);
                row = view.FindViewById<LinearLayout>(Resource.Id.fila_sucursal);
            }
        }

        /// <summary>
        /// Ons the bind view holder.
        /// </summary>
        /// <param name="holder">Holder.</param>
        /// <param name="position">Position.</param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            PaisViewHolder miholder = holder as PaisViewHolder;
            LinearLayout view = miholder.row;
            ImageView icon = (ImageView)view.FindViewById(Resource.Id.image_sucursal);
            Pais pais = list[position];
            string pinName = pais.name.ToLower();
            switch (pinName)
            {
                case ("alemania"):
                    icon.SetImageResource(Resource.Drawable.alemania_circle);
                    break;
                case ("arabia saudita"):
                    icon.SetImageResource(Resource.Drawable.arabiasaudi_circle);
                    break;
                case ("australia"):
                    icon.SetImageResource(Resource.Drawable.australia_circle);
                    break;
                case ("brasil"):
                    icon.SetImageResource(Resource.Drawable.brazil_circle);
                    break;
                case ("canadá"):
                    icon.SetImageResource(Resource.Drawable.canada_circle);
                    break;
                case ("chile"):
                    icon.SetImageResource(Resource.Drawable.chile_circle);
                    break;
                case ("china"):
                    icon.SetImageResource(Resource.Drawable.china_circle);
                    break;
                case ("colombia"):
                    icon.SetImageResource(Resource.Drawable.colombia_circle);
                    break;
                case ("cuba"):
                    icon.SetImageResource(Resource.Drawable.cuba_circle);
                    break;
                case ("egipto"):
                    icon.SetImageResource(Resource.Drawable.egipto_circle);
                    break;
                case ("estados unidos"):
                    icon.SetImageResource(Resource.Drawable.estadosunidos_circle);
                    break;
                case ("francia"):
                    icon.SetImageResource(Resource.Drawable.francia_circle);
                    break;
                case ("grecia"):
                    icon.SetImageResource(Resource.Drawable.grecia_circle);
                    break;
                case ("india"):
                    icon.SetImageResource(Resource.Drawable.india_circle);
                    break;
                case ("italia"):
                    icon.SetImageResource(Resource.Drawable.italia_circle);
                    break;
                case ("japon"):
                    icon.SetImageResource(Resource.Drawable.japon_circle);
                    break;
                case ("korea"):
                    icon.SetImageResource(Resource.Drawable.korea_circle);
                    break;
                case ("méxico"):
                    icon.SetImageResource(Resource.Drawable.mexico_circle);
                    break;
                case ("perú"):
                    icon.SetImageResource(Resource.Drawable.peru_circle);
                    break;
                case ("portugal"):
                    icon.SetImageResource(Resource.Drawable.portugal_circle);
                    break;
                case ("puerto rico"):
                    icon.SetImageResource(Resource.Drawable.puertorico_circle);
                    break;
                case ("república dominicana"):
                    icon.SetImageResource(Resource.Drawable.republicadominicana_circle);
                    break;
                case ("rusia"):
                    icon.SetImageResource(Resource.Drawable.russia_circle);
                    break;
                case ("españa"):
                    icon.SetImageResource(Resource.Drawable.spain_circle);
                    break;
                case ("sudáfrica"):
                    icon.SetImageResource(Resource.Drawable.sudafrica_circle);
                    break;
                case ("tailandia"):
                    icon.SetImageResource(Resource.Drawable.tailandia_circle);
                    break;
                case ("turquia"):
                    icon.SetImageResource(Resource.Drawable.turquia_circle);
                    break;
                case ("uruguay"):
                    icon.SetImageResource(Resource.Drawable.uruguay_circle);
                    break;
                case ("venezuela"):
                    icon.SetImageResource(Resource.Drawable.venezuela_circle);
                    break;
                default:
                    icon.SetImageResource(Resource.Drawable.chile_circle);
                    break;
            }

            miholder.mImage = icon;
            miholder.mNombre.Text = pais.name;
            miholder.mDistancia.Text = pais.format;//Math.Round(sucursal.UserDistance, 1) + " Km";
            miholder.mNombre.Typeface = fontBold;
            miholder.mDistancia.Typeface = fontRegular;
        }

        /// <summary>
        /// Ons the create view holder.
        /// </summary>
        /// <returns>The create view holder.</returns>
        /// <param name="parent">Parent.</param>
        /// <param name="viewType">View type.</param>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View card = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.row_pais, parent, false);
            PaisViewHolder viewholder = new PaisViewHolder(card, onClick);

            viewholder.row.Click += (sender, e) =>
            {
                Pais pais = list[viewholder.AdapterPosition];
                lNombreLatLong = new List<object>();
                lNombreLatLong.Add(pais.name);
                lNombreLatLong.Add(new LatLng(pais.Latitude, pais.Longitude));
                lNombreLatLong.Add(pais.location.address);
                this.fragment.Activity.FinishActivity(100);
                string id = pais.id + string.Empty;
                DataManager.paisSeleccionado = pais;
                itemClick(sender, lNombreLatLong);
            };
            return viewholder;
        }



        /// <summary>
        /// Ons the click.
        /// </summary>
        /// <param name="locationName">Location name.</param>
        void onClick(List<Object> locationName)
        {
            Intent intentAHomeAgregaProducto = new Intent(fragment.Activity, typeof(RecetasListActivity));
            fragment.Activity.StartActivity(intentAHomeAgregaProducto);
        }


        /// <summary>
        /// Refreshs the adapter.
        /// </summary>
        /// <param name="l">L.</param>
        /// <param name="map">Map.</param>
        public void refreshAdapter(List<Pais> l, GoogleMap map)
        {
            list = new List<Pais>();
            list = SortSucursales(l);
            originList = SortSucursales(l);
            LatLng location = new LatLng(originList[0].Latitude, originList[1].Longitude);
            map.MoveCamera(CameraUpdateFactory.NewLatLng(location));
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(1);
            builder.Bearing(0);
            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            map.MoveCamera(cameraUpdate);
            map.MyLocationEnabled = true;
            NotifyDataSetChanged();
        }

        /// <summary>
        /// Sorts the sucursales.
        /// </summary>
        /// <returns>The sucursales.</returns>
        /// <param name="pPais">S sucursal.</param>
        private List<Pais> SortSucursales(List<Pais> pPais)
        {
            pPais.Sort((x, y) =>
            {
                if (x.HasUserDistance && !y.HasUserDistance)
                {
                    return -1;
                }
                else if (!x.HasUserDistance && y.HasUserDistance)
                {
                    return 1;
                }
                else if (!x.HasUserDistance && !y.HasUserDistance)
                {
                    return 0;
                }

                if (x.UserDistance < y.UserDistance)
                    return -1;
                else if (Math.Abs(x.UserDistance - y.UserDistance) <= Double.Epsilon)
                    return 0;
                else
                    return 1;
            });
            return pPais;
        }
    }
}