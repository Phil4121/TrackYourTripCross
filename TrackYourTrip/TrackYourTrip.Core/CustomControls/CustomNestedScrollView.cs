using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TrackYourTrip.Core.CustomControls
{
    public class CustomNestedScrollView : ListView
    {
        public static readonly BindableProperty IsNestedScrollProperty = BindableProperty.Create(
            propertyName: nameof(IsNestedScroll),
            returnType: typeof(bool),
            declaringType: typeof(CustomNestedScrollView),
            defaultValue: false
            );

        public bool IsNestedScroll
        {
            get => (bool)GetValue(IsNestedScrollProperty);
            set => SetValue(IsNestedScrollProperty, value);
        }

    }
}
