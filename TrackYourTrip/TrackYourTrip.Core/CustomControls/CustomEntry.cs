using Xamarin.Forms;

namespace TrackYourTrip.Core.CustomControls
{
    public class CustomEntry : Entry
    {
        public static readonly BindableProperty IsValidProperty =
            BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(CustomEntry), true, BindingMode.TwoWay);

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            set => SetValue(IsValidProperty, value);
        }

        public static readonly BindableProperty BorderErrorColorProperty =
            BindableProperty.Create(nameof(BorderErrorColor), typeof(Xamarin.Forms.Color), typeof(CustomEntry), Color.Transparent, BindingMode.TwoWay);

        public Xamarin.Forms.Color BorderErrorColor
        {
            get => (Xamarin.Forms.Color)GetValue(BorderErrorColorProperty);
            set => SetValue(BorderErrorColorProperty, value);
        }

        public static readonly BindableProperty ErrorTextProperty =
        BindableProperty.Create(nameof(ErrorText), typeof(string), typeof(CustomEntry), string.Empty);

        public string ErrorText
        {
            get => (string)GetValue(ErrorTextProperty);
            set => SetValue(ErrorTextProperty, value);
        }
    }
}
