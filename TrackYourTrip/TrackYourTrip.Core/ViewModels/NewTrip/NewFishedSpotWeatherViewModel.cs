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

[assembly: MvxNavigation(typeof(NewFishedSpotWeatherViewModel), @"NewFishedSpotWeatherPage")]
namespace TrackYourTrip.Core.ViewModels.NewTrip
{
    public class NewFishedSpotWeatherViewModel : BaseViewModel<FishedSpotModel, OperationResult<IModel>>, ISharedFishedSpotViewModel
    {
        public NewFishedSpotWeatherViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider, IUserDialogs userDialog, ILocalizeService localizeService)
            : base(Resources.AppResources.NewFishedSpotWeatherPageTitle, mvxLogProvider, navigationService, userDialog, localizeService)
        {
            MessagingCenter.Subscribe<ElementFinishedMessage>(this, MessageHelper.ELEMENT_FINISHED_MESSAGE, message =>
            {
                if (message.BackgroundTask.ID_TaskType != (int)EnumHelper.TaskTypeEnum.WheaterTask ||
                    message.BackgroundTask.ID_ElementReference != FishedSpot.Id)
                    return;

                SetWheaterDataFields(new JSONHelper<WeatherTaskResponseModel>().Deserialize(message.BackgroundTask.TaskResponse));
            });
        }

        #region Properties

        private FishedSpotModel _fishedSpot = new FishedSpotModel();
        public FishedSpotModel FishedSpot
        {
            get => _fishedSpot;
            set => SetProperty(ref _fishedSpot, value);
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

        public override bool IsNew => throw new NotImplementedException();

        #endregion

        #region Tasks

        public MvxNotifyTask NavigationTask { get; private set; }

        public MvxNotifyTask PushToBackgroundQueue { get; private set; }

        #endregion

        #region Methodes

        public override void Prepare(FishedSpotModel parameter)
        {
            base.Prepare(parameter);

            FishedSpot = parameter;

            PushToBackgroundQueue = MvxNotifyTask.Create(PushWheaterRequestToBackgroundQueue(), ex => LogException(ex));
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        async Task PushWheaterRequestToBackgroundQueue()
        {
            await BackgroundQueueService.PushWheaterRequestToBackgroundQueue(FishedSpot.Id, FishedSpot.Spot.SpotMarker[0].Lat, FishedSpot.Spot.SpotMarker[0].Lng);

            var message = new StartBackgroundWorkingServiceMessage();
            MessagingCenter.Send(message, MessageHelper.START_BACKGROUND_WORKING_SERVICE_MESSAGE);
        }

        void SetWheaterDataFields(WeatherTaskResponseModel model)
        {
            FishedSpot.Weather.Temperature = model.Temperature;

            RaisePropertyChanged(() => FishedSpot);
        }

        #endregion
    }
}
