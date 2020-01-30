using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using TrackYourTrip.Core.ViewModels.Settings;

namespace TrackYourTrip.Core.Pages.Settings
{
    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class SpotsPage : MvxContentPage<SpotsViewModel>
    {
        public SpotsPage()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (SpotsListView.SelectedItem != null)
            {
                SpotsListView.SelectedItem = null;
            }
        }
    }
}