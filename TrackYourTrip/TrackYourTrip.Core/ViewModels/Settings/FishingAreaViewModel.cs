using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.Services;
using TrackYourTrip.Core.ViewModels.Settings;
using TrackYourTrip.Core.CustomValidators;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using TrackYourTrip.Core.Interfaces;
using System.Threading.Tasks;
using TrackYourTrip.Core.ViewModelResults;
using MvvmCross.Commands;
using Acr.UserDialogs;

[assembly: MvxNavigation(typeof(FishingAreaViewModel), @"FishingAreaPage")]
namespace TrackYourTrip.Core.ViewModels.Settings
{
    public class FishingAreaViewModel : BaseViewModel<FishingAreaModel, OperationResult<FishingAreaModel>>
    {
        public FishingAreaViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider, IUserDialogs userDialog, ILocalizeService localizeService)
            : base(Resources.AppResources.FishingAreaPageTitle, mvxLogProvider, navigationService, userDialog, localizeService)
        {


            MapClickedCommand = new MvxCommand<Position?>(ExecuteMapClicked);

            SpotsClickedCommand = new MvxAsyncCommand(NavigateToSpots);
        }

        #region Properties

        private IDataServiceFactory<FishingAreaModel> _dataStore;
        public override IDataServiceFactory<FishingAreaModel> DataStore
        {
            get
            {
                if (_dataStore == null)
                    _dataStore = DataServiceFactory.GetFishingAreaFactory();

                return _dataStore;
            }
            set { _dataStore = value; }
        }


        private MvxObservableCollection<WaterModel> _waters;
        public MvxObservableCollection<WaterModel> Waters
        {
            get => _waters;
            private set => SetProperty(ref _waters, value);
        }

        public override bool IsNew => FishingArea.IsNew;

        public FishingAreaModel FishingArea { get; set; }

        public CameraUpdate MapCenter
        {
            get
            {
                if (FishingArea != null &&
                    FishingArea.Lat != 0 &&
                    FishingArea.Lng != 0)
                    return CameraUpdateFactory.NewPositionZoom(new Position(FishingArea.Lat, FishingArea.Lng), 15d);

                var loc = LocationHelper.GetCurrentLocation();
                return CameraUpdateFactory.NewPositionZoom(new Position(loc.Latitude, loc.Longitude), 15d);
            }
        }

        public MvxObservableCollection<Pin> Pins
        {
            get
            {
                if (FishingArea.Lat == 0 ||
                    FishingArea.Lng == 0)
                    return new MvxObservableCollection<Pin>();

                return new MvxObservableCollection<Pin>()
                {
                    new Pin
                    {
                        Label = !string.IsNullOrEmpty(FishingArea.FishingArea) ? FishingArea.FishingArea : string.Empty,
                        Position = new Position(FishingArea.Lat, FishingArea.Lng)
                    }
                };
            }
        }

        public string SpotTitle
        {
            get
            {
                return Resources.AppResources.SpotsPageTitle;
            }
        }

        #endregion

        #region Commands

        public IMvxCommand<Position?> MapClickedCommand { get; private set; }

        public MvxAsyncCommand SpotsClickedCommand { get; private set; }

        #endregion

        #region Tasks

        public MvxNotifyTask InitWatersTask { get; private set; }

        #endregion

        #region Methodes

        public override void Prepare(FishingAreaModel parameter)
        {
            FishingArea = parameter;
        }

        public override Task Initialize()
        {
            InitWatersTask = MvxNotifyTask.Create(InitWaters, onException: ex => LogException(ex));

            return base.Initialize();
        }

        public override void Validate()
        {
            FishingAreaModelValidator validator = new FishingAreaModelValidator();
            var result = validator.Validate(FishingArea);
            FishingArea.IsValid = result.IsValid;
            ValidationResult = result;
        }

        public override void Save()
        {
            base.Save();

            try
            {
                IsBusy = true;

                if (IsValid)
                {
                    var result = MvxNotifyTask.Create(DataStore.SaveItemAsync(FishingArea), onException: ex => LogException(ex));

                    if (result.IsCompleted) {
                        IsBusy = false;
                        NavigationService.Close(this, new OperationResult<FishingAreaModel>(FishingArea, isSaved: result.Result));
                    }
                }
            }catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async override void DeleteCancel()
        {
            base.DeleteCancel();

            if (!IsNew)
            {
                var confirmation = await UserDialog.ConfirmAsync(LocalizeService.Translate("DeleteItemText"), 
                    title: LocalizeService.Translate("DeleteCommandTitle"));

                if (!confirmation)
                    return;

                var result = Task.Run<bool>(() => DataStore.DeleteItemAsync(FishingArea));

                if (result.Result)
                    await NavigationService.Close(this, new OperationResult<FishingAreaModel>(FishingArea, isDeleted: result.Result));
                
                return;
            }

            await NavigationService.Close(this, new OperationResult<FishingAreaModel>(FishingArea, isCanceld: true));
        }

        void ExecuteMapClicked(Position? position)
        {
            RefreshPin(position);
        }

        async Task NavigateToSpots()
        {
            IsBusy = true;

            var result = await NavigationService.Navigate<SpotsViewModel, FishingAreaModel, OperationResult<FishingAreaModel>>(FishingArea);

            IsBusy = false;

            if (result != null)
            {
                FishingArea.Spots = result.Entity.Spots;
            }
        }

        void RefreshPin(Position? position)
        {
            if (!position.HasValue)
                return;

            try
            {
                FishingArea.Lat = position.Value.Latitude;
                FishingArea.Lng = position.Value.Longitude;

                RaisePropertyChanged(nameof(Pins));
            }
            catch (Exception ex)
            {
                throw;
                // handle!
            }
        }

        async Task InitWaters()
        {
            Waters = new MvxObservableCollection<WaterModel>(
                await DataServiceFactory.GetWaterFactory().GetItemsAsync()
            );

            await RaisePropertyChanged(() => InitWatersTask);
        }

        #endregion
    }
}
