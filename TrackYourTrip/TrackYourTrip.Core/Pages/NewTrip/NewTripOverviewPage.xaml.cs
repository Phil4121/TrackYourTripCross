
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Xamarin.Forms.Xaml;

namespace TrackYourTrip.Core.Pages.NewTrip
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxTabbedPagePresentation(TabbedPosition.Root, NoHistory = false)]
    public partial class NewTripOverviewPage : MvxTabbedPage
    {
        public NewTripOverviewPage()
        {
            InitializeComponent();
        }
    }
}