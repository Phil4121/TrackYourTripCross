using MvvmCross.Forms.Views;
using TrackYourTrip.Core.ViewModels.Root;

namespace TrackYourTrip.Core.Pages.Root
{
    public partial class SettingsPage : MvxContentPage<SettingsViewModel>
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (SettingsListView.SelectedItem != null)
            {
                SettingsListView.SelectedItem = null;
            }
        }
    }
}