package md5b3d12bc1d52280aee38ebeba647bbc06;


public class AdapterListaRecetas_RecetaEnListaViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("RikiiRecetas.Adapter.AdapterListaRecetas+RecetaEnListaViewHolder, RikiiRecetas", AdapterListaRecetas_RecetaEnListaViewHolder.class, __md_methods);
	}


	public AdapterListaRecetas_RecetaEnListaViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == AdapterListaRecetas_RecetaEnListaViewHolder.class)
			mono.android.TypeManager.Activate ("RikiiRecetas.Adapter.AdapterListaRecetas+RecetaEnListaViewHolder, RikiiRecetas", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
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
