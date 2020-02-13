using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrackYourTrip.Core.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomPickerWithErrorLabel : ContentView
    {
        #region Properties

        public static readonly BindableProperty IsValidProperty =
            BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(CustomPickerWithErrorLabel), true, BindingMode.OneWayToSource);

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            private set
            {
                SetValue(IsValidProperty, value);
            }
        }


        public static readonly BindableProperty ErrorTextProperty =
            BindableProperty.Create(nameof(ErrorText), typeof(string), typeof(CustomPickerWithErrorLabel), string.Empty, BindingMode.TwoWay, propertyChanged: (b, o, n) =>
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


        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(CustomPickerWithErrorLabel), null, propertyChanged: (b, o, n) => {
                if (Equals(n, null) && Equals(o, null))
                    return;

                CustomPickerWithErrorLabel control = (CustomPickerWithErrorLabel)b;
                control.MainPicker.ItemsSource = (IEnumerable)n;
            });

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }


        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem), typeof(Object), typeof(CustomPickerWithErrorLabel), null, BindingMode.TwoWay, propertyChanged: (b, o, n) =>
            {
                if (Equals(n, null) && Equals(o, null))
                    return;

                CustomPickerWithErrorLabel control = (CustomPickerWithErrorLabel)b;
                control.MainPicker.SelectedItem = (Object)n;

            });

        public Object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }


        public static readonly BindableProperty SelectedValueProperty =
            BindableProperty.Create(nameof(SelectedValue), typeof(Object), typeof(CustomPickerWithErrorLabel), null, BindingMode.TwoWay, propertyChanged: (b, o, n) =>
            {
                if (Equals(n, null) && Equals(o, null))
                    return;


                CustomPickerWithErrorLabel control = (CustomPickerWithErrorLabel)b;
                control.MainPicker.SelectedValue = (Object)n;

                ResetErrorStatus(b);
            });

        public Object SelectedValue
        {
            get => GetValue(SelectedValueProperty);
            set => SetValue(SelectedValueProperty, value);
        }


        public static readonly BindableProperty SelectedValuePathProperty =
            BindableProperty.Create(nameof(SelectedValuePath), typeof(String), typeof(CustomPickerWithErrorLabel), string.Empty, BindingMode.TwoWay, propertyChanged: (b, o, n) =>
            {
                if (Equals(n, null) && Equals(o, null))
                    return;


                CustomPickerWithErrorLabel control = (CustomPickerWithErrorLabel)b;
                control.MainPicker.SelectedValuePath = (String)n;
            });

        public String SelectedValuePath
        {
            get => (string)GetValue(SelectedValuePathProperty);
            set => SetValue(SelectedValuePathProperty, value);
        }


        public static readonly BindableProperty DisplayMemberPathProperty =
            BindableProperty.Create(nameof(DisplayMemberPath), typeof(String), typeof(CustomPickerWithErrorLabel), string.Empty, BindingMode.TwoWay, propertyChanged: (b, o, n) =>
            {
                if (Equals(n, null) && Equals(o, null))
                    return;

                CustomPickerWithErrorLabel control = (CustomPickerWithErrorLabel)b;
                control.MainPicker.DisplayMemberPath = (String)n;
            });

        public String DisplayMemberPath
        {
            get => (string)GetValue(DisplayMemberPathProperty);
            set => SetValue(DisplayMemberPathProperty, value);
        }

        #endregion

        #region Events

        public delegate void PickerChangedHandler(object sender, PickerEventArgs args);

        public event PickerChangedHandler OnSelectedPickerValueChanged;

        #endregion

        public CustomPickerWithErrorLabel()
        {
            InitializeComponent();

            MainPicker.BindingContext = this;
            ErrorLabel.BindingContext = this;

            MainPicker.SelectedIndexChanged += MainPicker_SelectedIndexChanged;
        }

        #region EventHandler

        private void MainPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MainPicker.SelectedValue == null)
                return;
            
            OnSelectedPickerValueChanged?.Invoke(this, new PickerEventArgs { SelectedValue = MainPicker.SelectedValue });
        }

        #endregion

        #region Methodes

        static void ResetErrorStatus(BindableObject bindable)
        {
            bindable.SetValue(ErrorTextProperty, string.Empty);
        }

        #endregion
    }

    public class PickerEventArgs : EventArgs
    {
        public object SelectedValue { get; set; }
    }
}