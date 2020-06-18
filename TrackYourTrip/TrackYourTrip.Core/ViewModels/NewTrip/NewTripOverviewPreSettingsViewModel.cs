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
                        async () =>
                        {
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
                if (PreSettings == null)
                    return;

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

        public Guid SelectedTurbidityId
        {
            get
            {
                if (PreSettings == null)
                    return Guid.NewGuid();

                return PreSettings.ID_Turbidity;
            }
            set
            {
                if (PreSettings == null)
                    return;

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

        public Guid SelectedCurrentId
        {
            get
            {
                if (PreSettings == null)
                    return Guid.NewGuid();

                return PreSettings.ID_Current;
            }
            set
            {
                if (PreSettings == null)
                    return;

                PreSettings.ID_Current = value;

                RaisePropertyChanged(nameof(SelectedCurrentId));
            }
        }

        private MvxObservableCollection<CurrentModel> _currents;
        public MvxObservableCollection<CurrentModel> Currents
        {
            get => _currents;
            set => SetProperty(ref _currents, value);
        }

        public Guid SelectedBaitColorId
        {
            get
            {
                if (PreSettings == null)
                    return Guid.NewGuid();

                return PreSettings.ID_BaitColor;
            }
            set
            {
                if (PreSettings == null)
                    return;

                PreSettings.ID_BaitColor = value;

                RaisePropertyChanged(nameof(SelectedBaitColorId));
            }
        }

        private MvxObservableCollection<BaitColorModel> _baitColor;
        public MvxObservableCollection<BaitColorModel> BaitColor
        {
            get => _baitColor;
            set => SetProperty(ref _baitColor, value);
        }

        public Guid SelectedBaitTypeId
        {
            get
            {
                if (PreSettings == null)
                    return Guid.NewGuid();

                return PreSettings.ID_BaitType;
            }
            set
            {
                if (PreSettings == null)
                    return;

                PreSettings.ID_BaitType = value;

                RaisePropertyChanged(nameof(SelectedBaitTypeId));
            }
        }

        private MvxObservableCollection<BaitTypeModel> _baitType;
        public MvxObservableCollection<BaitTypeModel> BaitType
        {
            get => _baitType;
            set => SetProperty(ref _baitType, value);
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
            preSettings.ID_Current = SelectedCurrentId;

            return preSettings;
        }

        async Task<PreDefinedSpotSettings> InitPreSettingsAsync()
        {
            var preSettings = new PreDefinedSpotSettings();
            var settings = await DataServiceFactory.GetGenerallSettingFactory().GetItemsAsync();

            preSettings.WaterTemperatureUnit = GenerallSettingsHelper.GetDefaultTemperatureUnit();
            preSettings.WaterLevelUnit = GenerallSettingsHelper.GetDefaultLengthUnit();

            return preSettings;
        }

        async Task PreFillFieldsAsync()
        {
            Turbidities = new MvxObservableCollection<TurbidityModel>(
                await DataServiceFactory.GetTurbidityFactory().GetItemsAsync()
                );

            await RaisePropertyChanged(() => Turbidities);

            WaterColors = new MvxObservableCollection<WaterColorModel>(
                await DataServiceFactory.GetWaterColorFactory().GetItemsAsync()
                );

            await RaisePropertyChanged(() => WaterColors);

            Currents = new MvxObservableCollection<CurrentModel>(
                await DataServiceFactory.GetCurrentFactory().GetItemsAsync()
                );

            await RaisePropertyChanged(() => Currents);

            BaitColor = new MvxObservableCollection<BaitColorModel>(
                await DataServiceFactory.GetBaitColorFactory().GetItemsAsync()
                );

            await RaisePropertyChanged(() => BaitColor);

            BaitType = new MvxObservableCollection<BaitTypeModel>(
                await DataServiceFactory.GetBaitTypeFactory().GetItemsAsync()
                );

            await RaisePropertyChanged(() => BaitType);
        }

        void LogException(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
