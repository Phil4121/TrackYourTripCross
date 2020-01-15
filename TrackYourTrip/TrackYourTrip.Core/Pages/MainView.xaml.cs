// ---------------------------------------------------------------
// <author>Paul Datsyuk</author>
// <url>https://www.linkedin.com/in/pauldatsyuk/</url>
// ---------------------------------------------------------------

using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using TrackYourTrip.Core.ViewModels;

namespace TrackYourTrip.Core.Pages
{
    [MvxTabbedPagePresentation(TabbedPosition.Root, WrapInNavigationPage = true, NoHistory = false)]
    public partial class MainView : MvxTabbedPage<MainViewModel>
    {
        private bool _firstTime = true;

        public MainView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_firstTime)
            {
                ViewModel.ShowInitialViewModelsCommand.ExecuteAsync(null);
                _firstTime = false;
            }
        }
    }
}