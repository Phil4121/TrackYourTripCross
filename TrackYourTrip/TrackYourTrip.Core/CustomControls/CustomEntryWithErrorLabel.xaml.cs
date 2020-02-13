using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrackYourTrip.Core.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomEntryWithErrorLabel : ContentView
    {
        #region Properties

        public static readonly BindableProperty IsValidProperty =
            BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(CustomEntryWithErrorLabel), true, BindingMode.OneWayToSource);

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            private set
            {
                SetValue(IsValidProperty, value);
            }
        }

        public static readonly BindableProperty ErrorTextProperty =
            BindableProperty.Create(nameof(ErrorText), typeof(string), typeof(CustomEntryWithErrorLabel), string.Empty, BindingMode.TwoWay, propertyChanged:(b,o,n) =>
            {
                if (Equals(n, null) && Equals(o, null))
                    return;

                b.SetValue(IsValidProperty, string.IsNullOrEmpty((string)n));

            });

        public string ErrorText
        {
            get => (string)GetValue(ErrorTextProperty);
            set
            {
                SetValue(ErrorTextProperty, value);
            }
        }

        public static readonly BindableProperty UserInputProperty =
            BindableProperty.Create(nameof(UserInput), typeof(string), typeof(CustomEntryWithErrorLabel), string.Empty, BindingMode.TwoWay, propertyChanged:(b,o,n) =>
            {
                if (Equals(n, null) && Equals(o, null))
                    return;

                ResetErrorStatus(b);
            });

        public string UserInput
        {
            get => (string)GetValue(UserInputProperty);
            set => SetValue(UserInputProperty, value);
        }

        #endregion

        public CustomEntryWithErrorLabel()
        {
            InitializeComponent();

            MainEntry.BindingContext = this;
            ErrorLabel.BindingContext = this;
        }

        #region Methodes

        static void ResetErrorStatus(BindableObject bindable)
        {
            bindable.SetValue(ErrorTextProperty, string.Empty);
        }

        #endregion
    }
}