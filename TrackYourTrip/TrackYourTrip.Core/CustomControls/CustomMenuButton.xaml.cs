using System;
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
            get => ButtonIcon.Source;
            set => ButtonIcon.Source = value;
        }

        public static readonly BindableProperty ButtonTextProperty = BindableProperty.Create(
                                                         propertyName: "ButtonText",
                                                         returnType: typeof(string),
                                                         declaringType: typeof(CustomMenuButton),
                                                         defaultValue: "",
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         propertyChanged: ButtonTextPropertyChanged);

        private static void ButtonTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMenuButton control = (CustomMenuButton)bindable;
            control.ButtonLabel.Text = newValue.ToString();
        }

        public string ButtonText { get; set; }

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