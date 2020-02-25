using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace TrackYourTrip.Core.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomMapWithErrorLabel : ContentView
    {

        #region Properties

        public static readonly BindableProperty IsValidProperty =
            BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(CustomMapWithErrorLabel), true, BindingMode.OneWayToSource);

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            private set
            {
                SetValue(IsValidProperty, value);
            }
        }

        public static readonly BindableProperty ErrorTextProperty =
            BindableProperty.Create(nameof(ErrorText), typeof(string), typeof(CustomMapWithErrorLabel), string.Empty, BindingMode.TwoWay, propertyChanged: (b, o, n) =>
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


        public static readonly BindableProperty CameraCenterViewProperty =
            BindableProperty.Create(nameof(CameraCenterView), typeof(CameraUpdate), typeof(CustomMapWithErrorLabel), null, BindingMode.TwoWay, propertyChanged: (b,o,n) => {
                if (Equals(n, null) && Equals(o, null))
                    return;

                CustomMapWithErrorLabel control = (CustomMapWithErrorLabel)b;
                control.map.InitialCameraUpdate = (CameraUpdate)n;

                if (control.map.InitialCameraUpdate != null) {
                    control.map.MoveCamera((CameraUpdate)n);
                }
            });


        public CameraUpdate CameraCenterView
        {
            get => (CameraUpdate)GetValue(CameraCenterViewProperty);
            set => SetValue(CameraCenterViewProperty, value);
        }


        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(ObservableCollection<Pin>), typeof(CustomMapWithErrorLabel), new ObservableCollection<Pin>(), BindingMode.TwoWay, propertyChanged: (b, o, n) => {
                if (Equals(n, null) && Equals(o, null))
                    return;

                CustomMapWithErrorLabel control = (CustomMapWithErrorLabel)b;
                control.map.ItemsSource = (ObservableCollection<Pin>)n;

                ResetErrorStatus(b);
            });

        public ObservableCollection<Pin> ItemsSource
        {
            get => (ObservableCollection<Pin>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        #endregion

        #region Events

        public delegate void MapClickedEventHandler(object sender, MapEventArgs args);

        public event MapClickedEventHandler OnMapClicked;

        #endregion

        public CustomMapWithErrorLabel()
        {
            InitializeComponent();

            map.BindingContext = this;
            ErrorLabel.BindingContext = this;
        }

        #region Methodes

        private void map_MapClicked(object sender, MapClickedEventArgs e)
        {
            OnMapClicked?.Invoke(this, new MapEventArgs { ClickedPosition = new Position(e.Point.Latitude,e.Point.Longitude) });
        }

        static void ResetErrorStatus(BindableObject bindable)
        {
            bindable.SetValue(ErrorTextProperty, string.Empty);
        }

        #endregion
    }

    public class MapEventArgs : EventArgs
    {
        public Position ClickedPosition { get; set; }
    }
}