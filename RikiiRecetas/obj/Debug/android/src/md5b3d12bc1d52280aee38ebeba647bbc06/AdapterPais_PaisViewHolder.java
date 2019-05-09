package md5b3d12bc1d52280aee38ebeba647bbc06;


public class AdapterPais_PaisViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("RikiiRecetas.Adapter.AdapterPais+PaisViewHolder, RikiiRecetas", AdapterPais_PaisViewHolder.class, __md_methods);
	}


	public AdapterPais_PaisViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == AdapterPais_PaisViewHolder.class)
			mono.android.TypeManager.Activate ("RikiiRecetas.Adapter.AdapterPais+PaisViewHolder, RikiiRecetas", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
