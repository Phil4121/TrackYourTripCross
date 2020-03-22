﻿using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Models;
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

        #region Properties

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

        public TurbidityModel SelectedTurbidity {
            get
            {
                if (PreSettings == null)
                    return new TurbidityModel();

                return PreSettings.Turbidity;
            }
            set {
                PreSettings.Turbidity = value;
                PreSettings.ID_Turbidity = value.Id;

                RaisePropertyChanged(nameof(SelectedTurbidity));
            }
        }

        private MvxObservableCollection<TurbidityModel> _turbidities;

        public MvxObservableCollection<TurbidityModel> Turbidities
        {
            get => _turbidities;
            set => SetProperty(ref _turbidities, value);
        }

        #endregion

        #region Tasks

        public MvxNotifyTask PreFillFieldsTask { get; private set; }

        #endregion

        public override Task Initialize()
        {
            PreFillFieldsTask = MvxNotifyTask.Create(() => PreFillFieldsAsync(), onException: ex => LogException(ex));

            return base.Initialize();
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
            {
                PreSettings = storedPreDefinedSpotSettings;
            }
            else
            {
                MvxNotifyTask.Create(
                        async () =>
                        {
                            PreSettings = await InitPreSettingsAsync();
                            await RaisePropertyChanged(nameof(PreSettings));
                        }, onException: ex => LogException(ex));
            }
        }

        async Task<PreDefinedSpotSettings> SetPreSettingsAsync()
        {
            var preSettings = await InitPreSettingsAsync();

            preSettings.Turbidity = SelectedTurbidity;
            preSettings.ID_Turbidity = SelectedTurbidity.Id;

            return preSettings;
        }

        async Task<PreDefinedSpotSettings> InitPreSettingsAsync()
        {
            var preSettings = new PreDefinedSpotSettings();
            var settings = await DataServiceFactory.GetGenerallSettingFactory().GetItemsAsync();

            preSettings.WaterTemperatureUnit = int.Parse(settings.Where(s => s.SettingKey == TableConsts.DEFAULT_TEMPERATURE_UNIT).First().SettingValue);
            preSettings.WaterLevelUnit = int.Parse(settings.Where(s => s.SettingKey == TableConsts.DEFAULT_LENGTH_UNIT).First().SettingValue);

            return preSettings;
        }

        async Task PreFillFieldsAsync()
        {
            Turbidities = new MvxObservableCollection<TurbidityModel>(
                await DataServiceFactory.GetTurbidityFactory().GetItemsAsync()
            );

            await RaisePropertyChanged(() => Turbidities);
        }

        void LogException(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
