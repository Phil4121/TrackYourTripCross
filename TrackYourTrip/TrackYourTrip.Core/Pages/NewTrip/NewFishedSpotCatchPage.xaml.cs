using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrackYourTrip.Core.Pages.NewTrip
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewFishedSpotCatchPage : MvxContentPage
    {
        public NewFishedSpotCatchPage()
        {
            InitializeComponent();
        }
    }
}