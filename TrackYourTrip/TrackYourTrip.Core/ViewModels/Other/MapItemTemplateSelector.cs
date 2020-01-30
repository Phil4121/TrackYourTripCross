using Xamarin.Forms;

namespace TrackYourTrip.Core.ViewModels.Other
{
    public class MapItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return DataTemplate;
        }
    }
}
