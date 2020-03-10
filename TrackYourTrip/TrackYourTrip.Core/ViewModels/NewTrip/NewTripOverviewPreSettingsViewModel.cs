using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
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

        private PreDefinedSpotSettings _preSettings = new PreDefinedSpotSettings();

        public PreDefinedSpotSettings PreSettings
        {
            get => _preSettings;
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

            PreSettings = _globalSettings.PreDefinedSpotSettings;
        }
    }
}
