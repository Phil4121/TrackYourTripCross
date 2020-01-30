using System;
using TrackYourTrip.Core.CustomControls;
using Xamarin.Forms;

namespace TrackYourTrip.Core.CustomBehavior
{
    public class EmptyPickerValidatorBehavior : Behavior<CustomPicker>
    {
        CustomPicker control;
        //string _placeHolder;
        //Xamarin.Forms.Color _placeHolderColor;

        protected override void OnAttachedTo(CustomPicker bindable)
        {
            bindable.Focused += Bindable_Focused;
            bindable.SelectedIndexChanged += Bindable_SelectedIndexChanged;

            control = bindable;
        }

        private void Bindable_SelectedIndexChanged(object sender, EventArgs e)
        {
            EntryIsValid();
        }

        private void Bindable_Focused(object sender, FocusEventArgs e)
        {
            if (!e.IsFocused)
            {
                EntryIsValid();
            }
        }

        protected override void OnDetachingFrom(CustomPicker bindable)
        {
            bindable.Focused -= Bindable_Focused;
            bindable.SelectedIndexChanged -= Bindable_SelectedIndexChanged;
        }


        protected bool EntryIsValid()
        {
            if (string.IsNullOrEmpty(control.SelectedValue.ToString()))
            {
                control.IsValid = false;
                return false;
            }

            control.IsValid = true;
            return true;
        }
    }
}
