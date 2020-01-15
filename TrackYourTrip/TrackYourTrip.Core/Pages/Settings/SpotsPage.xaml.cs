using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackYourTrip.Core.ViewModels.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
                SpotsListView.SelectedItem = null;
        }
    }
}