using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Services;
using TrackYourTrip.Core.ViewModels.NewTrip;

[assembly: MvxNavigation(typeof(NewTripOverviewPreSettingsViewModel), @"NewTripOverviewPreSettingsPage")]
namespace TrackYourTrip.Core.ViewModels.NewTrip
{
    public class NewTripOverviewPreSettingsViewModel : MvxViewModel
    {
        public NewTripOverviewPreSettingsViewModel()
        {

        }

        GlobalSettings _globalSettings = new GlobalSettings();

        public string Title
        {
            get => Resources.AppResources.NewTripOverviewPreSettingsPageTitle;
        }

        private PreDefinedSpotSettings _preSettings;

        public PreDefinedSpotSettings PreSettings
        {
            get
            {
                if (_preSettings == null)
                {
                    var tsk = MvxNotifyTask.Create(
                        async () => {
                            _preSettings = await SetPreSettingsAsync();
                            await RaisePropertyChanged(nameof(PreSettings));
                            },
                        onException: ex => LogException(ex));
                }

                return _preSettings;
            }
            set => SetProperty(ref _preSettings, value);
        }

        public override void ViewDisappearing()
        {
            base.ViewDisappearing();

            _globalSettings.PreDefinedSpotSettings = PreSettings;
        }

        public override void ViewAppearing()
        {
            base.ViewAppearing();

            var storedPreDefinedSpotSettings = _globalSettings.PreDefinedSpotSettings;

            if (storedPreDefinedSpotSettings != null)
                PreSettings = storedPreDefinedSpotSettings;
        }

        async Task<PreDefinedSpotSettings> SetPreSettingsAsync()
        {
            var preSettings = new PreDefinedSpotSettings();
            var settings = await DataServiceFactory.GetGenerallSettingFactory().GetItemsAsync();

            preSettings.WaterTemperatureUnit = int.Parse(settings.Where(s => s.SettingKey == TableConsts.DEFAULT_TEMPERATURE_UNIT).First().SettingValue);

            return preSettings;
        }

        void LogException(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
