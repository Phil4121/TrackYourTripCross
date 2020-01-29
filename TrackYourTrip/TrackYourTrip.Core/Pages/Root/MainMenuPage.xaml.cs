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
    public partial class MainMenuPage : MvxContentPage<MainMenuViewModel>
    {
        public MainMenuPage()
        {
            InitializeComponent();
        }
    }
}