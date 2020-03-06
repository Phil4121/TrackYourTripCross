using Acr.UserDialogs;
using FFImageLoading.Forms;
using FluentValidation.Results;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackYourTrip.Core.CustomValidators;
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

            PreSetData();
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

        private string _fishingAreaErrorText;
        public string FishingAreaErrorText
        {
            get => _fishingAreaErrorText;
            set => SetProperty(ref _fishingAreaErrorText, value);
        }

        public new string SaveCommandTitle => Resources.AppResources.NewTripStart;

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

        public MvxNotifyTask ActiveTripTask { get; private set; }

        public MvxNotifyTask WheaterDataTask { get; private set; }

        #endregion

        #region Methodes

        public async override void ViewAppearing()
        {
            base.ViewAppearing();

            if (ActiveTripExists())
            {
                bool resumeActiveTrip = await UserDialog.ConfirmAsync(Resources.AppResources.ActiveTripPromptText, 
                                        Resources.AppResources.ActiveTripTitle, 
                                        Resources.AppResources.ActiveTripContinue, 
                                        Resources.AppResources.ActiveTripClose);

                ResumeWhenTripIsActive(resumeActiveTrip);
            }
        }

        public override void Validate()
        {
            StartNewTripValidator validator = new StartNewTripValidator();
            FluentValidation.Results.ValidationResult result = validator.Validate(Trip);
            Trip.IsValid = result.IsValid;
            ValidationResult = result;

            if (!result.IsValid)
                SetValidationFailures(result.Errors);
        }

        public override async Task SaveAsync()
        {
            var settings = Mvx.IoCProvider.Resolve<IAppSettings>();

            try
            {
                IsBusy = true;

                await base.SaveAsync();

                if (IsValid)
                {
                    Trip.Id = Guid.NewGuid();
                    Trip = await DataStore.SaveItemAsync(Trip);

                    settings.TripIdInProcess = Trip.Id.ToString();

                    await NavigationService.Navigate<NewTripOverviewViewModel, TripModel, OperationResult<IModel>>(Trip);
                }
            }
            catch (Exception ex)
            {
                settings.TripIdInProcess = Guid.Empty.ToString();
                throw ex;
            }
            finally
            {
                IsBusy = false;
            }
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

        void SetWheaterStatusPicture()
        {
            try
            {
                var settings = Mvx.IoCProvider.Resolve<IAppSettings>();

                WheaterStatusPicture = StatusHelper.StatusPicEnum.STATUS_WAITING.ToString();

                WheaterDataTask = MvxNotifyTask.Create(async () => {
                    var result = await WheaterServiceFactory.GetWheaterServiceFactory().ServiceIsReachable(settings.DefaultThreadWaitTime);

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

        void SetValidationFailures(IList<ValidationFailure> vf)
        {
            foreach (ValidationFailure f in vf)
            {
                switch (f.PropertyName.ToLower())
                {
                    case "fishingarea":
                        FishingAreaErrorText = f.ErrorMessage;
                        break;
                }
            }
        }

        void PreSetData()
        {
            Trip.TripDateTime = DateTime.Now;
        }

        void ResumeWhenTripIsActive(bool resumeWithActiveTrip)
        {
            if (resumeWithActiveTrip)
                ActiveTripTask = MvxNotifyTask.Create(NavigateToActiveTrip(), ex => LogException(ex));
            else
                ActiveTripTask = MvxNotifyTask.Create(DeleteActiveTrip(), ex => LogException(ex));
        }

        async Task NavigateToActiveTrip()
        {
            try
            {
                IsBusy = true;

                var activeTrip = await DataStore.GetItemAsync(Guid.Parse(TripHelper.GetTripIdInProcess()));

                Trip = activeTrip;

                await NavigationService.Navigate<NewTripOverviewViewModel, TripModel, OperationResult<IModel>>(activeTrip);
            }
            catch (Exception ex)
            {
                TripHelper.ResetTripInProcess();

                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task DeleteActiveTrip()
        {
            try
            {
                IsBusy = true;

                var activeTrip = await DataStore.GetItemAsync(Guid.Parse(TripHelper.GetTripIdInProcess()));
                await DataStore.DeleteItemAsync(activeTrip);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                TripHelper.ResetTripInProcess();
                IsBusy = false;
            }
        }

        bool ActiveTripExists()
        {
            return (TripHelper.TripInProcess() && 
                Trip.Id != Guid.Parse(TripHelper.GetTripIdInProcess()));
        }

        #endregion
    }
}
