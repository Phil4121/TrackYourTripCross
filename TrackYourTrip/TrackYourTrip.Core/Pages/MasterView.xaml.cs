// ---------------------------------------------------------------
// <author>Paul Datsyuk</author>
// <url>https://www.linkedin.com/in/pauldatsyuk/</url>
// ---------------------------------------------------------------

using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;

namespace TrackYourTrip.Core.Pages
{
    [MvxMasterDetailPagePresentation(MasterDetailPosition.Master)]
    public partial class MasterView : MvxContentPage
    {
        public MasterView()
        {
            InitializeComponent();
        }
    }
}