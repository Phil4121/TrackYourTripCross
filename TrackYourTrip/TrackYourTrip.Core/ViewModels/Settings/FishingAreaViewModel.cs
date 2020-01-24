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
using System.Linq;

[assembly: MvxNavigation(typeof(FishingAreaViewModel), @"FishingAreaPage")]
namespace TrackYourTrip.Core.ViewModels.Settings
{
    public class FishingAreaViewModel : BaseViewModel<FishingAreaModel, OperationResult<FishingAreaModel>>
    {
        public FishingAreaViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider, IUserDialogs userDialog, ILocalizeService localizeService)
            : base(Resources.AppResources.FishingAreaPageTitle, mvxLogProvider, navigationService, userDialog, localizeService)
        {

            MapClickedCommand = new MvxCommand<Position?>(
                (param) => RefreshPinsTask = MvxNotifyTask.Create(RefreshPinAsync(param), onException: ex => LogException(ex))
                );

            WaterChangedCommand = new MvxCommand(
                () => RefreshWaterTask = MvxNotifyTask.Create(RefreshWaterAsync, onException: ex => LogException(ex))
                );

            SpotsClickedCommand = new MvxCommand(
                () => NavigationTask = MvxNotifyTask.Create(NavigateToSpotsAsync, onException: ex => LogException(ex))
                );

            RefreshFishingAreaCommand = new MvxCommand(
                () => RefreshFishingAreaTask = MvxNotifyTask.Create(RefreshFishingAreaAsync, onException: ex => LogException(ex))
                );
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

        private FishingAreaModel _fishingArea;

        public FishingAreaModel FishingArea {
            get => _fishingArea;
            set => SetProperty(ref _fishingArea, value);
        }

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
                        Label = !string.IsNullOrEmpty(FishingArea.FishingArea) ? FishingArea.FishingArea : FishingArea.Id.ToString(),
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

        public IMvxCommand WaterChangedCommand { get; private set; }

        public MvxCommand SpotsClickedCommand { get; private set; }

        public MvxCommand RefreshFishingAreaCommand { get; private set; }

        #endregion

        #region Tasks

        public MvxNotifyTask InitWatersTask { get; private set; }

        public MvxNotifyTask RefreshPinsTask { get; private set; }

        public MvxNotifyTask NavigationTask { get; private set; }

        public MvxNotifyTask RefreshWaterTask { get; private set; }

        public MvxNotifyTask RefreshFishingAreaTask { get; private set; }

        #endregion

        #region Methodes

        public override void Prepare(FishingAreaModel parameter)
        {
            FishingArea = parameter;
        }

        public override void ViewAppearing()
        {
            if(!FishingArea.IsNew)
                RefreshFishingAreaTask = MvxNotifyTask.Create(RefreshFishingAreaAsync, onException: ex => LogException(ex));

            base.ViewAppearing();
        }

        public override Task Initialize()
        {
            InitWatersTask = MvxNotifyTask.Create(InitWatersAsync, onException: ex => LogException(ex));

            return base.Initialize();
        }

        public override void Validate()
        {
            FishingAreaModelValidator validator = new FishingAreaModelValidator();
            var result = validator.Validate(FishingArea);
            FishingArea.IsValid = result.IsValid;
            ValidationResult = result;
        }

        public async override Task SaveAsync()
        {
            await base.SaveAsync();

            try
            {
                IsBusy = true;

                if (IsValid)
                {
                    FishingArea = DataStore.SaveItem(FishingArea);

                    await NavigationService.Close(this, new OperationResult<FishingAreaModel>(FishingArea, isSaved: true));
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

        public async override Task DeleteAsync()
        {
            await base.DeleteAsync();

            if (!IsNew)
            {
                var confirmation = await UserDialog.ConfirmAsync(LocalizeService.Translate("DeleteItemText"), 
                    title: LocalizeService.Translate("DeleteCommandTitle"));

                if (!confirmation)
                    return;

                var result = DataStore.DeleteItem(FishingArea);

                if (result)
                    await NavigationService.Close(this, new OperationResult<FishingAreaModel>(FishingArea, isDeleted: result));
                
                return;
            }

            await NavigationService.Close(this, new OperationResult<FishingAreaModel>(FishingArea, isCanceld: true));
        }

        async Task NavigateToSpotsAsync()
        {
            await base.SaveAsync();

            try
            {
                if (IsValid)
                {
                    IsBusy = true;

                    FishingArea = DataStore.SaveItem(FishingArea);

                    await NavigationService.Navigate<SpotsViewModel, FishingAreaModel, OperationResult<FishingAreaModel>>(FishingArea);
                }
            }catch(Exception ex)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task RefreshPinAsync(Position? position)
        {
            if (!position.HasValue)
                return;

            try
            {
                FishingArea.Lat = position.Value.Latitude;
                FishingArea.Lng = position.Value.Longitude;

                await RaisePropertyChanged(() => Pins);
            }
            catch (Exception ex)
            {
                throw;
                // handle!
            }
        }

        async Task RefreshWaterAsync()
        {
            if (FishingArea == null)
                return;

            FishingArea.WaterModel = Waters.Where(w => w.Id == FishingArea.ID_WaterModel).FirstOrDefault();

            await RaisePropertyChanged(() => FishingArea);
        }

        async Task InitWatersAsync()
        {
            Waters = new MvxObservableCollection<WaterModel>(
                await DataServiceFactory.GetWaterFactory().GetItemsAsync()
            );

            await RaisePropertyChanged(() => InitWatersTask);
        }

        async Task RefreshFishingAreaAsync()
        {
            if (FishingArea == null || 
                FishingArea.IsNew)
                return;

            FishingArea = await DataStore.GetItemAsync(FishingArea.Id);

            await RaisePropertyChanged(() => RefreshFishingAreaTask);
        }

        #endregion
    }
}
