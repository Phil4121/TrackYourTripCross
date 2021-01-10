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
    public partial class CustomTimeDisplayControl : ContentView
    {
        #region Properties

        public static readonly BindableProperty FromProperty =
            BindableProperty.Create(nameof(From), typeof(DateTime), typeof(CustomTimeDisplayControl), DateTime.MinValue, BindingMode.TwoWay);

        public DateTime From
        {
            get => (DateTime)GetValue(FromProperty);
            set => SetValue(FromProperty, value);
        }

        public static readonly BindableProperty ToProperty =
            BindableProperty.Create(nameof(To), typeof(DateTime), typeof(CustomTimeDisplayControl), DateTime.MinValue, BindingMode.TwoWay, propertyChanged: (b, o, n) =>
            {
                if (Equals(n, null) && Equals(o, null))
                    return;

                DateTime newTo = DateTime.MinValue;
                DateTime.TryParse(n.ToString(), out newTo);

                b.SetValue(IsMultilineProperty, newTo != DateTime.MinValue);
            });

        public DateTime To
        {
            get => (DateTime)GetValue(ToProperty);
            set => SetValue(ToProperty, value);
        }

        public static readonly BindableProperty IsMultilineProperty =
            BindableProperty.Create(nameof(IsMultiline), typeof(bool), typeof(CustomTimeDisplayControl), false, BindingMode.TwoWay);


        public bool IsMultiline
        {
            get => (bool)GetValue(IsMultilineProperty);
            private set => SetValue(IsMultilineProperty, value);
        }

        #endregion


        public CustomTimeDisplayControl()
        {
            InitializeComponent();

            FromLabel.BindingContext = this;
            ToLabel.BindingContext = this;
            Seperator.BindingContext = this;
        }
    }
}