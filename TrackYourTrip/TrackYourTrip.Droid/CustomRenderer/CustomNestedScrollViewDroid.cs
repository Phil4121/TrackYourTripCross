using TrackYourTrip.Core.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomNestedScrollView), typeof(CustomNestedScrollViewDroid))]
public class CustomNestedScrollViewDroid : ListViewRenderer
{
    public CustomNestedScrollViewDroid(Android.Content.Context context) : base(context)
    {

    }

    protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
    {
        base.OnElementChanged(e);

        if(e.NewElement != null)
        {
            var listview = this.Control as Android.Widget.ListView;
            listview.NestedScrollingEnabled = true;
        }
    }
}