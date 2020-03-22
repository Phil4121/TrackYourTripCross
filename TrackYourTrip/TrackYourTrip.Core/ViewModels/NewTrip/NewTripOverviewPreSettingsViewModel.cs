using MvvmCross.Navigation;
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
                            },
                        onException: ex => LogException(ex));
                }

                return _preSettings;
            }
            set => SetProperty(ref _preSettings, value);
        }

        public Guid SelectedWaterColorId
        {
            get
            {
                if (PreSettings == null)
                    return Guid.NewGuid();

                return PreSettings.ID_WaterColor;
            }
            set
            {
                PreSettings.ID_WaterColor = value;

                RaisePropertyChanged(nameof(SelectedWaterColorId));
            }
        }

        private MvxObservableCollection<WaterColorModel> _waterColors;

        public MvxObservableCollection<WaterColorModel> WaterColors
        {
            get => _waterColors;
            set => SetProperty(ref _waterColors, value);
        }

        public Guid SelectedTurbidityId {
            get
            {
                if (PreSettings == null)
                    return Guid.NewGuid();

                return PreSettings.ID_Turbidity;
            }
            set {
                PreSettings.ID_Turbidity = value;

                RaisePropertyChanged(nameof(SelectedTurbidityId));
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

            preSettings.ID_Turbidity = SelectedTurbidityId;
            preSettings.ID_WaterColor = SelectedWaterColorId;

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

            WaterColors = new MvxObservableCollection<WaterColorModel>(
                    await DataServiceFactory.GetWaterColorFactory().GetItemsAsync()
                );

            await RaisePropertyChanged(() => Turbidities);
            await RaisePropertyChanged(() => WaterColors);
        }

        void LogException(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
