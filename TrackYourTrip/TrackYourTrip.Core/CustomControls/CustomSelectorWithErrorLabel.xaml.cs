using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrackYourTrip.Core.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomSelectorWithErrorLabel : ContentView
    {
        #region Properties

        public static readonly BindableProperty IsValidProperty =
            BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(CustomSelectorWithErrorLabel), true, BindingMode.OneWayToSource);

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            private set
            {
                SetValue(IsValidProperty, value);
            }
        }

        public static readonly BindableProperty DisplayTextProperty =
        BindableProperty.Create(nameof(DisplayText), typeof(string), typeof(CustomSelectorWithErrorLabel), string.Empty, BindingMode.TwoWay, propertyChanged: (b, o, n) =>
        {
            if (Equals(n, null) && Equals(o, null))
                return;

            b.SetValue(IsValidProperty, string.IsNullOrEmpty((string)n));
            ResetErrorStatus(b);
        });

        public string DisplayText
        {
            get => (string)GetValue(DisplayTextProperty);
            set => SetValue(DisplayTextProperty, value);
        }

        public static readonly BindableProperty ErrorTextProperty =
            BindableProperty.Create(nameof(ErrorText), typeof(string), typeof(CustomSelectorWithErrorLabel), string.Empty, BindingMode.TwoWay, propertyChanged: (b, o, n) =>
            {
                if (Equals(n, null) && Equals(o, null))
                    return;

                b.SetValue(IsValidProperty, string.IsNullOrEmpty((string)n));

            });

        public string ErrorText
        {
            get => (string)GetValue(ErrorTextProperty);
            set => SetValue(ErrorTextProperty, value);
        }

        #endregion

        #region Events

        public delegate void SelectorClickedEventHandler(object sender, EventArgs args);

        public event SelectorClickedEventHandler OnSelectorClicked;

        #endregion

        public CustomSelectorWithErrorLabel()
        {
            InitializeComponent();

            DisplayLabel.BindingContext = this;
            ErrorLabel.BindingContext = this;
        }

        private void SelectionButton_Clicked(object sender, EventArgs e)
        {
            OnSelectorClicked?.Invoke(this, new EventArgs());
        }

        static void ResetErrorStatus(BindableObject bindable)
        {
            bindable.SetValue(ErrorTextProperty, string.Empty);
        }
    }
}