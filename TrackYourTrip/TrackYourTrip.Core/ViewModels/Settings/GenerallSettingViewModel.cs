using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.Services;
using TrackYourTrip.Core.ViewModels.Settings;
using static TrackYourTrip.Core.Helpers.EnumHelper;

[assembly: MvxNavigation(typeof(GenerallSettingViewModel), @"GenerallSettingPage")]
namespace TrackYourTrip.Core.ViewModels.Settings
{
    public class GenerallSettingViewModel : BaseViewModel<GenerallSettingModel>
    {
        public GenerallSettingViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider, IUserDialogs userDialog, ILocalizeService localizeService)
            : base(Resources.AppResources.GenerallSettingPageTitle, mvxLogProvider, navigationService, userDialog, localizeService)
        {
            InitSettingsTask = MvxNotifyTask.Create(() => LoadGenerallSettingsAsync(), onException: ex => LogException(ex));
        }

        #region Properties

        private IDataServiceFactory<GenerallSettingModel> _dataStore;
        public override IDataServiceFactory<GenerallSettingModel> DataStore
        {
            get
            {
                if (_dataStore == null)
                {
                    _dataStore = DataServiceFactory.GetGenerallSettingFactory();
                }

                return _dataStore;
            }
            set => _dataStore = value;
        }

        public override bool IsNew => true;

        private List<GenerallSettingModel> _generallSettings;
        public List<GenerallSettingModel> GenerallSettings {
            get => _generallSettings;
            set => SetProperty(ref _generallSettings, value);
        }

        private bool _isCelsius;
        public bool IsCelsius {
            get => _isCelsius;
            set
            {
                SetProperty(ref _isCelsius,  value);

                GenerallSettings.Where(set => set.SettingKey == TableConsts.DEFAULT_TEMPERATURE_UNIT)
                    .First()
                    .SettingValue = BooleanHelper.ConvertBooltoString(IsCelsius);
            }
        }

        private bool _isMetric;
        public bool IsMetric
        {
            get => _isMetric;
            set
            {
                SetProperty(ref _isMetric, value);

                GenerallSettings.Where(set => set.SettingKey == TableConsts.DEFAULT_LENGTH_UNIT)
                    .First()
                    .SettingValue = BooleanHelper.ConvertBooltoString(IsMetric);
            }
        }

        #endregion

        #region Commands

        public MvxCommand<bool> TemperatureUnitSwitchedCommand { get; private set; }

        #endregion

        #region Tasks

        public MvxNotifyTask InitSettingsTask { get; private set; }

        #endregion

        public override async Task SaveAsync()
        {


            base.SaveAsync();

        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        async Task LoadGenerallSettingsAsync()
        {
            GenerallSettings = new List<GenerallSettingModel>(
                await DataStore.GetItemsAsync()
                );

            InitSettings();
        }

        private void InitSettings()
        {
            if (GenerallSettings == null)
                return;

            var tempUnit = GenerallSettings.Where(gs => gs.SettingKey == TableConsts.DEFAULT_TEMPERATURE_UNIT).FirstOrDefault().SettingValue;
            IsCelsius = Int32.Parse(tempUnit) == (int)TemperatureUnitEnum.C;

            tempUnit = GenerallSettings.Where(gs => gs.SettingKey == TableConsts.DEFAULT_LENGTH_UNIT).FirstOrDefault().SettingValue;
            IsMetric = Int32.Parse(tempUnit) == (int)LengthUnitEnum.M;

            RaisePropertyChanged(nameof(IsCelsius));
        }
    }
}
