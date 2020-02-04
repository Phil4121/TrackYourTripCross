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
        public static readonly BindableProperty IsValidProperty =
            BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(CustomEntryWithErrorLabel), true, BindingMode.TwoWay);

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            set
            {
                SetValue(IsValidProperty, value);

                if (IsValid)
                    ErrorText = string.Empty;

                OnPropertyChanged(nameof(IsValid));
            }
        }

        public static readonly BindableProperty ErrorTextProperty =
            BindableProperty.Create(nameof(ErrorText), typeof(string), typeof(CustomEntryWithErrorLabel), string.Empty, BindingMode.TwoWay);

        public string ErrorText
        {
            get => (string)GetValue(ErrorTextProperty);
            set => SetValue(ErrorTextProperty, value);
        }

        public static readonly BindableProperty UserInputProperty =
            BindableProperty.Create(nameof(UserInput), typeof(string), typeof(CustomEntryWithErrorLabel), string.Empty,
                BindingMode.TwoWay);

        public string UserInput
        {
            get => GetValue(UserInputProperty).ToString();
            set => SetValue(UserInputProperty, value);
        }

        public CustomEntryWithErrorLabel()
        {
            InitializeComponent();

            MainEntry.BindingContext = this;
            ErrorLabel.BindingContext = this;

            MainEntry.TextChanged += MainEntry_TextChanged;
        }

        private void MainEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!IsValid)
                IsValid = true;
        }
    }
}