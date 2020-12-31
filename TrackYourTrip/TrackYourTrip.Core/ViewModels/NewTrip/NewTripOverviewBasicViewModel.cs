﻿using Acr.UserDialogs;
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

        private CameraUpdate _mapCenter = null;

        public CameraUpdate MapCenter
        {
            get
            {
                if (_mapCenter != null)
                    return _mapCenter;


                if (Trip != null &&
                    Trip.FishingArea != null &&
                    Trip.FishingArea.Lat != 0 &&
                    Trip.FishingArea.Lng != 0)
                {
                    _mapCenter = CameraUpdateFactory.NewPositionZoom(new Position(Trip.FishingArea.Lat, Trip.FishingArea.Lng), 15d);
                    return _mapCenter;
                }
                else
                {
                    if (_mapCenter != null)
                        return _mapCenter;
                }

                Xamarin.Essentials.Location loc = LocationHelper.GetCurrentLocation();
                _mapCenter = CameraUpdateFactory.NewPositionZoom(new Position(loc.Latitude, loc.Longitude), 15d);

                return _mapCenter;
            }

            set => SetProperty(ref _mapCenter, value);
        }

        private MvxObservableCollection<Pin> _pins = null;

        public MvxObservableCollection<Pin> Pins
        {
            get
            {
                if (_pins != null && 
                    _pins.Count > 0)
                    return _pins;

                return new MvxObservableCollection<Pin>();
            }

            set => SetProperty(ref _pins, value);
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

                var fishingArea = await DataServiceFactory.GetFishingAreaFactory().GetItemAsync(Trip.FishingArea.Id);

                var fishedSpot = new FishedSpotModel(true)
                {
                    IsNew = true,
                    StartDateTime = DateTime.Now,
                    ID_Trip = Trip.Id,
                    ID_FishingArea = fishingArea.Id,
                    Trip = Trip,
                    FishingArea = fishingArea
                };

                fishedSpot.Water = CopyPreSettingsToWaterModel(fishedSpot);

                await DataServiceFactory.GetFishedSpotFactory().SaveItemAsync(fishedSpot);

                await NavigationService.Navigate<SpotsViewModel, OverviewArgs, SpotsViewModel>(new OverviewArgs(false, fishingArea, fishedSpot, PageHelper.NEWFISHEDSPOTOVERVIEW_PAGE));
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
            }

            FishedSpotWaterModel CopyPreSettingsToWaterModel(FishedSpotModel spot)
            {
                var water = new FishedSpotWaterModel();
                var gs = new GlobalSettings();

                water.ID_FishedSpot = spot.Id;
                water.IsNew = true;

                if (gs.PreDefinedSpotSettings != null) { 
                    water.WaterLevel = gs.PreDefinedSpotSettings.WaterLevel;
                    water.WaterTemperature = gs.PreDefinedSpotSettings.WaterTemperature;
                    water.ID_Current = gs.PreDefinedSpotSettings.ID_Current;
                    water.ID_Turbidity = gs.PreDefinedSpotSettings.ID_Turbidity;
                    water.ID_WaterColor = gs.PreDefinedSpotSettings.ID_WaterColor;
                }

                water.WaterLevelUnit = GenerallSettingsHelper.GetDefaultLengthUnit();
                water.WaterTemperatureUnit = GenerallSettingsHelper.GetDefaultTemperatureUnit();

                return water;
            }
        }

        #endregion
    }
}
