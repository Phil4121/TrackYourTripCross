using System;
using System.Collections.Generic;
using System.Text;
using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Threading.Tasks;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.Services;
using TrackYourTrip.Core.ViewModelResults;
using TrackYourTrip.Core.ViewModels.NewTrip;
using Xamarin.Forms.GoogleMaps;
using TrackYourTrip.Core.Services.BackgroundQueue;
using Xamarin.Forms;
using TrackYourTrip.Core.Services.BackgroundQueue.Messages;
using System.Threading;
using MvvmCross;
using Prism.Navigation;

[assembly: MvxNavigation(typeof(NewFishedSpotWeatherViewModel), @"NewFishedSpotWeatherPage")]
namespace TrackYourTrip.Core.ViewModels.NewTrip
{
    public class NewFishedSpotWeatherViewModel : BaseViewModel<FishedSpotModel, OperationResult<IModel>>, ISharedFishedSpotViewModel
    {
        public NewFishedSpotWeatherViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider, IUserDialogs userDialog, ILocalizeService localizeService)
            : base(Resources.AppResources.NewFishedSpotWeatherPageTitle, mvxLogProvider, navigationService, userDialog, localizeService)
        {
            OverwriteChangedCommand = new MvxCommand(
                () => NavigationTask = MvxNotifyTask.Create(ConfirmRemoveWeatherFromBackgroundQueue(), onException: ex => LogException(ex))
            );

            MessagingCenter.Subscribe<ElementFinishedMessage>(this, MessageHelper.ELEMENT_FINISHED_MESSAGE, message =>
            {
                if (message.BackgroundTask.ID_TaskType != (int)EnumHelper.TaskTypeEnum.WheaterTask ||
                    message.BackgroundTask.ID_ElementReference != FishedSpot.Id)
                    return;

                SetWheaterDataFields(new JSONHelper<WeatherTaskResponseModel>().Deserialize(message.BackgroundTask.TaskResponse));
            });
        }

        #region Properties

        Guid _backgroundQueueId = Guid.Empty;
        Guid BackgroundQueueId
        {
            get => _backgroundQueueId;
            set => SetProperty(ref _backgroundQueueId, value);
        }


        private FishedSpotModel _fishedSpot = new FishedSpotModel();
        public FishedSpotModel FishedSpot
        {
            get => _fishedSpot;
            set => SetProperty(ref _fishedSpot, value);
        }


        private string _wheaterStatusPicture = StatusHelper.GetPicForStatus(StatusHelper.StatusPicEnum.STATUS_UNDEFINED);
        public string WheaterStatusPicture
        {
            get => _wheaterStatusPicture;
            set => SetProperty(ref _wheaterStatusPicture, StatusHelper.GetPicForStatus(
                        (StatusHelper.StatusPicEnum)
                            Enum.Parse(typeof(StatusHelper.StatusPicEnum), value)));
        }


        bool _showWeatherStatusPicture = false;
        public bool ShowWeatherStatusPicture
        {
            get => _showWeatherStatusPicture;
            set => SetProperty(ref _showWeatherStatusPicture, value);
        }


        int _temperatureUnit = -1;
        public int TemperatureUnit
        {
            get => _temperatureUnit;
            set => SetProperty(ref _temperatureUnit, value);
        }


        IEnumerable<KeyValueModel> _weatherConditions = null;
        public IEnumerable<KeyValueModel> WeatherConditions
        {
            get
            {
                if (_weatherConditions == null)
                {
                    var conditions = new WeatherConditions(LocalizeService);
                    _weatherConditions = conditions.GetWeatherConditions();
                }

                return _weatherConditions;
            }
        }


        private IDataServiceFactory<FishedSpotModel> _dataStore;
        public override IDataServiceFactory<FishedSpotModel> DataStore
        {
            get
            {
                if (_dataStore == null)
                {
                    _dataStore = DataServiceFactory.GetFishedSpotFactory();
                }

                return _dataStore;
            }
            set => _dataStore = value;
        }


        public override bool IsNew
        {
            get => FishedSpot.IsNew;
        }

        public bool IsOverwritten
        {
            get => FishedSpot.Weather.IsOverwritten;
            set
            {
                FishedSpot.Weather.IsOverwritten = value;
                ShowWeatherStatusPicture = !value;

                RaisePropertyChanged(() => IsOverwritten);
                RaisePropertyChanged(() => ShowWeatherStatusPicture);
            }
        }

        #endregion

        #region Tasks

        public MvxNotifyTask NavigationTask { get; private set; }

        public MvxNotifyTask PushToBackgroundQueueTask { get; private set; }

        public MvxNotifyTask RemoveWeatherRequestFromBackgroundQueueTask { get; private set; }

        #endregion

        #region Commands

        public IMvxCommand OverwriteChangedCommand { get; private set; }

        #endregion

        #region Methodes

        public override void Prepare(FishedSpotModel parameter)
        {
            base.Prepare(parameter);

            FishedSpot = parameter;

            if (!IsOverwritten)
            {
                ShowWeatherStatusPicture = true;
                WheaterStatusPicture = StatusHelper.StatusPicEnum.STATUS_WAITING.ToString();
                PushToBackgroundQueueTask = MvxNotifyTask.Create(PushWheaterRequestToBackgroundQueue(), ex => LogException(ex));
            }
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        void SetWeatherStatusPicture(bool receivedWeatherData)
        {
            if (receivedWeatherData)
                WheaterStatusPicture = StatusHelper.StatusPicEnum.STATUS_OK.ToString();
            else
                WheaterStatusPicture = StatusHelper.StatusPicEnum.STATUS_ERROR.ToString();
        }

        async Task PushWheaterRequestToBackgroundQueue()
        {
            BackgroundQueueId = await BackgroundQueueService.PushWheaterRequestToBackgroundQueue(FishedSpot.Id, FishedSpot.Spot.SpotMarker[0].Lat, FishedSpot.Spot.SpotMarker[0].Lng);

            var message = new StartBackgroundWorkingServiceMessage();
            MessagingCenter.Send(message, MessageHelper.START_BACKGROUND_WORKING_SERVICE_MESSAGE);
        }

        void SetWheaterDataFields(WeatherTaskResponseModel model)
        {
            SetWeatherStatusPicture(model.Success);

            if (model.Success)
            {
                FishedSpot.Weather.CurrentTemperature = model.CurrentTemperature;

                FishedSpot.Weather.WeatherSituation = model.WeatherSituation;

                TemperatureUnit = model.TemperatureUnit;
                FishedSpot.Weather.TemperatureUnit = TemperatureUnit;

                FishedSpot.Weather.DailyTemperatureHigh = model.DailyTemperatureHigh;
                FishedSpot.Weather.DailyTemperatureHighTime = model.DailyTemperatureHighTime;
                FishedSpot.Weather.DailyTemperatureLow = model.DailyTemperatureLow;
                FishedSpot.Weather.DailyTemperatureLowTime = model.DailyTemperatureLowTime;
                FishedSpot.Weather.MoonPhase = model.MoonPhase;
                FishedSpot.Weather.Humidity = model.Humidity;
                FishedSpot.Weather.AirPressureInHPA = model.AirPressureInHPA;
                FishedSpot.Weather.SunRiseTime = model.SunRiseTime;
                FishedSpot.Weather.SunSetTime = model.SunSetTime;
                FishedSpot.Weather.UVIndex = model.UVIndex;
                FishedSpot.Weather.VisibilityInKM = model.VisibilityInKM;
                FishedSpot.Weather.WindBearing = model.WindBearing;
                FishedSpot.Weather.WindSpeedInMS = model.WindSpeedInMS;
                FishedSpot.Weather.CloudCover = model.CloudCover;

                RaisePropertyChanged(() => FishedSpot);
                RaisePropertyChanged(() => TemperatureUnit);
            }
        }

        async Task ConfirmRemoveWeatherFromBackgroundQueue()
        {
            if (IsOverwritten)
            {
                bool overwrite = await UserDialog.ConfirmAsync(Resources.AppResources.RemoveWeatherTaskFromBackgroundQueuePromptText,
                    Resources.AppResources.RemoveWeatherTaskFromBackgroundQueueTitle,
                    Resources.AppResources.RemoveWeatherTaskFromBackgroundQueuePromptYes,
                    Resources.AppResources.RemoveWeatherTaskFromBackgroundQueuePromptNo);

                if (overwrite)
                    RemoveWeatherRequestFromBackgroundQueueTask = MvxNotifyTask.Create(RemoveWeatherRequestFromQueue(), ex => LogException(ex));
                else
                    IsOverwritten = false;
            }
        }

        async Task RemoveWeatherRequestFromQueue()
        {
            if (BackgroundQueueId != Guid.Empty)
                await BackgroundQueueService.RemoveElementFromQueue(BackgroundQueueId);
        }

        #endregion
    }
}
