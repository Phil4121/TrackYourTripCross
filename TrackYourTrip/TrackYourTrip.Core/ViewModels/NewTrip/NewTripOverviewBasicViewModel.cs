using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.Services;
using TrackYourTrip.Core.ViewModelResults;
using TrackYourTrip.Core.ViewModels.NewTrip;
using TrackYourTrip.Core.ViewModels.Overviews;
using Xamarin.Forms.GoogleMaps;

[assembly: MvxNavigation(typeof(NewTripOverviewBasicViewModel), @"NewTripOverviewBasicPage")]
namespace TrackYourTrip.Core.ViewModels.NewTrip
{
    public class NewTripOverviewBasicViewModel : BaseViewModel<TripModel, OperationResult<IModel>>
    {
        public NewTripOverviewBasicViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider, IUserDialogs userDialog, ILocalizeService localizeService)
            : base(Resources.AppResources.NewTripOverviewPageTitle, mvxLogProvider, navigationService, userDialog, localizeService)
        {
            CheckInToSpotCommand = new MvxCommand(
                () => NavigationTask = MvxNotifyTask.Create(CheckInToNewSpotAsync(), onException: ex => LogException(ex))
            );
        }

        #region Properties

        private TripModel _trip = new TripModel();
        public TripModel Trip
        {
            get => _trip;
            set => SetProperty(ref _trip, value);
        }

        private IDataServiceFactory<TripModel> _dataStore;
        public override IDataServiceFactory<TripModel> DataStore
        {
            get
            {
                if (_dataStore == null)
                {
                    _dataStore = DataServiceFactory.GetTripFactory();
                }

                return _dataStore;
            }
            set => _dataStore = value;
        }

        public override bool IsNew => throw new NotImplementedException();

        public string CheckInButtonText => Resources.AppResources.SpotCheckinButtonText;

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
                return new MvxObservableCollection<Pin>();
            }
        }

        #endregion

        #region Commands

        public IMvxCommand CheckInToSpotCommand { get; private set; }

        #endregion

        #region Tasks

        public MvxNotifyTask NavigationTask { get; private set; }

        #endregion

        #region Methodes

        public override void Prepare(TripModel parameter)
        {
            base.Prepare(parameter);

            Trip = parameter;
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        async Task CheckInToNewSpotAsync()
        {
            try
            {
                IsBusy = true;

                var fishedSpot = new FishedSpotModel()
                {
                    ID_Trip = Trip.Id,
                    Trip = Trip
                };

                var fishingArea = await DataServiceFactory.GetFishingAreaFactory().GetItemAsync(Trip.FishingArea.Id);

                await NavigationService.Navigate<SpotsViewModel, OverviewArgs, SpotsViewModel>(new OverviewArgs(false, fishingArea, PageHelper.NEWFISHEDSPOTOVERVIEW_PAGE));
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}
