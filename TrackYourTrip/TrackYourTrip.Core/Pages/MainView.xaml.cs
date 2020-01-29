// ---------------------------------------------------------------
// <author>Paul Datsyuk</author>
// <url>https://www.linkedin.com/in/pauldatsyuk/</url>
// ---------------------------------------------------------------

using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using TrackYourTrip.Core.ViewModels;

namespace TrackYourTrip.Core.Pages
{
    public partial class MainView : MvxNavigationPage<MainViewModel>
    {
        private bool _firstTime = true;

        public MainView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (_firstTime)
            {
                ViewModel.ShowMainMenuViewModelCommand.Execute(null);

                _firstTime = false;
            }

            base.OnAppearing();
        }
    }
}