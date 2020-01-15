using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackYourTrip.Core.ViewModels.Root;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrackYourTrip.Core.Pages.Root
{
    [MvxTabbedPagePresentation(TabbedPosition.Tab, WrapInNavigationPage = true, NoHistory = false)]
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
                SettingsListView.SelectedItem = null;
        }
    }
}