using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackYourTrip.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrackYourTrip.Core.Pages
{
    [MvxTabbedPagePresentation(TabbedPosition.Tab, WrapInNavigationPage = true, NoHistory = false)]
    public partial class DemoView : MvxContentPage<DemoViewModel>
    {
        public DemoView()
        {
            InitializeComponent();
        }
    }
}