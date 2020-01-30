using TrackYourTrip.Core.CustomControls;
using Xamarin.Forms;

namespace TrackYourTrip.Core.CustomBehavior
{
    public class EmptyAndOnlyDigitsEntryValidatorBehavior : Behavior<CustomEntry>
    {
        CustomEntry control;
        string _placeHolder;
        bool _internalChange;
        Color _placeHolderColor;

        protected override void OnAttachedTo(CustomEntry bindable)
        {
            bindable.TextChanged += Bindable_TextChanged;
            bindable.Unfocused += Bindable_Unfocused;
            control = bindable;
            _placeHolder = bindable.Placeholder;
            _placeHolderColor = bindable.PlaceholderColor;
        }

        private void Bindable_Unfocused(object sender, FocusEventArgs e)
        {
            if (!e.IsFocused)
            {
                FormatEntry();
            }
        }

        private void Bindable_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_internalChange)
            {
                FormatEntry();
            }
        }

        protected override void OnDetachingFrom(CustomEntry bindable)
        {
            bindable.Unfocused -= Bindable_Unfocused;
            bindable.TextChanged -= Bindable_TextChanged;
        }


        protected bool EntryIsValid()
        {

            _internalChange = true;

            if (ControlIsNullOrEmpty())
            {
                control.IsValid = false;
                return false;
            }

            if (ControlContainsInvalidInput())
            {
                control.IsValid = false;
                return false;
            }

            control.IsValid = true;

            _internalChange = false;

            return true;
        }

        private bool ControlIsNullOrEmpty()
        {
            if (string.IsNullOrEmpty(control.Text))
            {
                return true;
            }

            return false;
        }

        private bool ControlContainsInvalidInput()
        {
            if (!double.TryParse(control.Text, out double i))
            {
                return true;
            }

            return false;
        }

        protected void FormatEntry()
        {
            if (control != null)
            {
                if (!EntryIsValid())
                {
                    if (ControlIsNullOrEmpty())
                    {
                        control.Placeholder = string.Format("*");
                        control.PlaceholderColor = Color.Red;
                        control.Text = string.Empty;
                        control.HorizontalTextAlignment = TextAlignment.Start;
                    }

                    if (!string.IsNullOrEmpty(control.Text) &&
                        ControlContainsInvalidInput())
                    {
                        control.Text = string.Format("{0}", control.Text);
                        control.PlaceholderColor = Color.Red;
                        control.HorizontalTextAlignment = TextAlignment.Start;
                    }
                }

                else
                {
                    control.Placeholder = _placeHolder;
                    control.PlaceholderColor = _placeHolderColor;
                    control.HorizontalTextAlignment = TextAlignment.Start;
                }
            }
        }
    }
}