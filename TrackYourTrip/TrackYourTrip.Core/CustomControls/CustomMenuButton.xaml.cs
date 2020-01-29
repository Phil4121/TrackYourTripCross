using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrackYourTrip.Core.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomMenuButton : ContentView
    {

        public event EventHandler ButtonClicked;

        public ImageSource Icon
        {
            get { return ButtonIcon.Source; }
            set { ButtonIcon.Source = value; }
        }

        public string Label
        {
            get { return ButtonLabel.Text; }
            set { ButtonLabel.Text = value; }
        }

        public CustomMenuButton()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            EventHandler handler = ButtonClicked;
            handler?.Invoke(this, e);
        }
    }
}