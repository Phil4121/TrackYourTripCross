// ---------------------------------------------------------------
// <author>Paul Datsyuk</author>
// <url>https://www.linkedin.com/in/pauldatsyuk/</url>
// ---------------------------------------------------------------

using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using System;

namespace TrackYourTrip.Core.Pages
{
    [MvxMasterDetailPagePresentation(MasterDetailPosition.Root, WrapInNavigationPage = false)]
    public partial class RootView : MvxMasterDetailPage
    {
        public RootView()
        {
            InitializeComponent();

            this.IsPresentedChanged += RootPage_IsPresentedChanged;
        }

        private void RootPage_IsPresentedChanged(object sender, EventArgs e)
        {
            int i = 0;
            //To Do Something
        }
    }
}