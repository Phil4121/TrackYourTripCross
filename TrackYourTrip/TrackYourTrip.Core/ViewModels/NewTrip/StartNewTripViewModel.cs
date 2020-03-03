using Acr.UserDialogs;
using FFImageLoading.Forms;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Threading.Tasks;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.Services;
using TrackYourTrip.Core.Services.Wheater;
using TrackYourTrip.Core.ViewModelResults;
using TrackYourTrip.Core.ViewModels.NewTrip;
using TrackYourTrip.Core.ViewModels.Overviews;
using Xamarin.Forms.GoogleMaps;

[assembly: MvxNavigation(typeof(StartNewTripViewModel), @"StartNewTripPage")]
namespace TrackYourTrip.Core.ViewModels.NewTrip
{
    public class StartNewTripViewModel : BaseViewModel<TripModel>
    {
        public StartNewTripViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider, IUserDialogs userDialog, ILocalizeService localizeService)
            : base(Resources.AppResources.NewTripPageTitle, mvxLogProvider, navigationService, userDialog, localizeService)
        {
            FishingAreaSelectedCommand = new MvxCommand(
                () => NavigationTask = MvxNotifyTask.Create(NavigateToFishingAreasSelectionAsync(), onException: ex => LogException(ex))
            );            
        }

        #region Properties

        private TripModel _trip = new TripModel();
        public TripModel Trip
        {
            get => _trip;
            set => SetProperty(ref _trip, value);
        }

        private string _wheaterStatusPicture = StatusHelper.GetPicForStatus(StatusHelper.StatusPicEnum.STATUS_UNDEFINED);

        public string WheaterStatusPicture {
            get => _wheaterStatusPicture;
            set => SetProperty(ref _wheaterStatusPicture, StatusHelper.GetPicForStatus(
                        (StatusHelper.StatusPicEnum)
                            Enum.Parse(typeof(StatusHelper.StatusPicEnum), value)));
        }

        public override IDataServiceFactory<TripModel> DataStore
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override bool IsNew => throw new NotImplementedException();

        public CameraUpdate MapCenter
        {
            get
            {
                if (Trip != null &&
                    Trip.FishingArea != null &&
                    Trip.FishingArea.Lat != 0 &&
                    Trip.FishingArea.Lng != 0)
                {
                    return CameraUpdateFactory.NewPositionZoom(new Position(Trip.FishingArea.Lat, Trip.FishingArea.Lng), 15d);
                }

                Xamarin.Essentials.Location loc = LocationHelper.GetCurrentLocation();
                return CameraUpdateFactory.NewPositionZoom(new Position(loc.Latitude, loc.Longitude), 15d);
            }
        }

        public MvxObservableCollection<Pin> Pins
        {
            get
            {
                if (Trip == null ||
                    Trip.FishingArea == null ||
                    Trip.FishingArea.Lat == 0 ||
                    Trip.FishingArea.Lng == 0)
                {
                    return new MvxObservableCollection<Pin>();
                }

                return new MvxObservableCollection<Pin>()
                {
                    new Pin
                    {
                        Label = !string.IsNullOrEmpty(Trip.FishingArea.FishingArea) ? Trip.FishingArea.FishingArea : Trip.FishingArea.Id.ToString(),
                        Position = new Position(Trip.FishingArea.Lat, Trip.FishingArea.Lng)
                    }
                };
            }
        }

        #endregion

        #region Commands

        public IMvxCommand FishingAreaSelectedCommand { get; private set; }

        #endregion

        #region Tasks

        public MvxNotifyTask NavigationTask { get; private set; }

        public MvxNotifyTask WheaterDataTask { get; private set; }

        #endregion

        #region Methodes

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        async Task NavigateToFishingAreasSelectionAsync()
        {
            try
            {
                IsBusy = true;

                var result = await NavigationService.Navigate<OverviewArgs, FishingAreaModel>(PageHelper.FISHING_AREAS_PAGE, new OverviewArgs(true));

                if (result != null)
                {
                    Trip.FishingArea = result;
                    Trip.ID_FishingArea = result.Id;

                    SetWheaterStatusPicture();

                    await RaisePropertyChanged(() => MapCenter);
                    await RaisePropertyChanged(() => Pins);
                    await RaisePropertyChanged(() => Trip);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void SetWheaterStatusPicture()
        {
            try
            {
                WheaterStatusPicture = StatusHelper.StatusPicEnum.STATUS_WAITING.ToString();

                WheaterDataTask = MvxNotifyTask.Create(async () => {
                    var result = await WheaterServiceFactory.GetWheaterServiceFactory().ServiceIsReachable(new GlobalSettings().DefaultThreadWaitTime);

                    if(result)
                        WheaterStatusPicture = StatusHelper.StatusPicEnum.STATUS_OK.ToString();
                    else
                        WheaterStatusPicture = StatusHelper.StatusPicEnum.STATUS_ERROR.ToString();

                }, onException: ex => LogException(ex));

            }catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
