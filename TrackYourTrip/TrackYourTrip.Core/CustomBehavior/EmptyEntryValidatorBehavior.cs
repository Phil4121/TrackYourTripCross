using TrackYourTrip.Core.CustomControls;
using Xamarin.Forms;

namespace TrackYourTrip.Core.CustomBehavior
{
    public class EmptyEntryValidatorBehavior : Behavior<CustomEntry>
    {
        CustomEntry control;
        string _placeHolder;
        Xamarin.Forms.Color _placeHolderColor;

        protected override void OnAttachedTo(CustomEntry bindable)
        {
            bindable.Focused += Bindable_Focused;
            bindable.TextChanged += Bindable_TextChanged;
            control = bindable;
            _placeHolder = bindable.Placeholder;
            _placeHolderColor = bindable.PlaceholderColor;
        }

        private void Bindable_Focused(object sender, FocusEventArgs e)
        {
            if (!e.IsFocused)
            {
                FormatEntry();
            }
        }

        private void Bindable_TextChanged(object sender, TextChangedEventArgs e)
        {
            FormatEntry();
        }

        protected override void OnDetachingFrom(CustomEntry bindable)
        {
            bindable.Focused -= Bindable_Focused;
            bindable.TextChanged -= Bindable_TextChanged;
        }


        protected bool EntryIsValid()
        {
            if (string.IsNullOrEmpty(control.Text))
            {
                control.IsValid = false;
                return false;
            }

            control.IsValid = true;
            return true;
        }

        protected void FormatEntry()
        {
            if (control != null)
            {
                if (!EntryIsValid())
                {
                    control.Placeholder = string.Format("*");
                    control.PlaceholderColor = Color.Red;
                    control.Text = string.Empty;
                    control.HorizontalTextAlignment = TextAlignment.End;
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
