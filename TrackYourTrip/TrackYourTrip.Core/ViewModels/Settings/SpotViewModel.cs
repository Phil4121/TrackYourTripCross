using Acr.UserDialogs;
using FluentValidation.Results;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackYourTrip.Core.CustomValidators;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.Services;
using TrackYourTrip.Core.ViewModelResults;
using TrackYourTrip.Core.ViewModels.Settings;
using Xamarin.Forms.GoogleMaps;

[assembly: MvxNavigation(typeof(SpotViewModel), @"Spot")]
namespace TrackYourTrip.Core.ViewModels.Settings
{
    public class SpotViewModel : BaseViewModel<SpotModel, OperationResult<SpotModel>>
    {
        public SpotViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider, IUserDialogs userDialog, ILocalizeService localizeService)
            : base(Resources.AppResources.SpotPageTitle, mvxLogProvider, navigationService, userDialog, localizeService)
        {
            MapClickedCommand = new MvxCommand<Position?>(ExecuteMapClicked);

            RefreshSpotCommand = new MvxCommand(
                () => RefreshSpotTask = MvxNotifyTask.Create(() => RefreshSpotAsync(), onException: ex => LogException(ex))
            );

            SpotTypeChangedCommand = new MvxCommand(
                () => RefreshSpotTypeTask = MvxNotifyTask.Create(() => RefreshSpotTypeAsync(), onException: ex => LogException(ex))
            );
        }

        #region Properties

        private IDataServiceFactory<SpotModel> _dataStore;
        public override IDataServiceFactory<SpotModel> DataStore
        {
            get
            {
                if (_dataStore == null)
                {
                    _dataStore = DataServiceFactory.GetSpotFactory();
                }

                return _dataStore;
            }
            set => _dataStore = value;
        }

        public override bool IsNew => Spot.IsNew;

        private MvxObservableCollection<SpotTypeModel> _spotTypes;
        public MvxObservableCollection<SpotTypeModel> SpotTypes
        {
            get => _spotTypes;
            private set => SetProperty(ref _spotTypes, value);
        }

        private SpotModel _spot;
        public SpotModel Spot
        {
            get => _spot;
            set => SetProperty(ref _spot, value);
        }

        public CameraUpdate MapCenter
        {
            get
            {
                if (Spot != null &&
                    SpotMarker != null &&
                    SpotMarker.Count != 0)
                {
                    return CameraUpdateFactory.NewPositionZoom(new Position(Spot.SpotMarker[0].Lat, Spot.SpotMarker[0].Lng), 15d);
                }

                Xamarin.Essentials.Location loc = LocationHelper.GetCurrentLocation();
                return CameraUpdateFactory.NewPositionZoom(new Position(loc.Latitude, loc.Longitude), 15d);
            }
        }

        public MvxObservableCollection<Pin> SpotMarker
        {
            get
            {
                MvxObservableCollection<Pin> marker = new MvxObservableCollection<Pin>();

                if (Spot.SpotMarker == null || Spot.SpotMarker.Count == 0)
                {
                    return marker;
                }

                foreach (SpotMarkerModel s in Spot.SpotMarker)
                {
                    marker.Add(new Pin
                    {
                        Position = new Position(s.Lat, s.Lng),
                        Label = !string.IsNullOrEmpty(s.SpotMarker) ? s.SpotMarker : s.Id.ToString()
                    });
                }

                return marker;
            }
        }

        public bool HasSelectedSpotType
        {
            get
            {
                if (Spot == null)
                {
                    return false;
                }

                Guid.TryParse(Spot.ID_SpotType.ToString(), out Guid result);

                RaisePropertyChanged(() => InstructionText);

                return result != Guid.Empty;
            }
        }

        public string InstructionText
        {
            get
            {
                if (Spot == null ||
                    Spot.ID_SpotType == Guid.Empty)
                {
                    return Resources.AppResources.SpotTypeEmptyText;
                }

                return Resources.AppResources.SpotTypeSetText;
            }
        }

        private string _spotNameErrorText;
        public string SpotNameErrorText
        {
            get => _spotNameErrorText;
            set => SetProperty(ref _spotNameErrorText, value);
        }

        private string _spotTypeErrorText;
        public string SpotTypeErrorText
        {
            get => _spotTypeErrorText;
            set => SetProperty(ref _spotTypeErrorText, value);
        }

        private string _spotLocationErrorText;
        public string SpotLocationErrorText
        {
            get => _spotLocationErrorText;
            set => SetProperty(ref _spotLocationErrorText, value);
        }

        #endregion

        #region Commands

        public IMvxCommand<Position?> MapClickedCommand { get; private set; }

        public IMvxCommand SpotTypeChangedCommand { get; private set; }

        public IMvxCommand RefreshSpotCommand { get; private set; }

        #endregion

        #region Tasks

        public MvxNotifyTask RefreshSpotTypeTask { get; private set; }

        public MvxNotifyTask InitSpotTypeTask { get; private set; }

        public MvxNotifyTask LoadSpotMarkerTask { get; private set; }

        public MvxNotifyTask RefreshSpotTask { get; private set; }

        #endregion

        #region Methodes

        public override void Prepare(SpotModel parameter)
        {
            Spot = parameter;
        }

        public override void ViewAppearing()
        {
            if (!Spot.IsNew)
            {
                RefreshSpotTask = MvxNotifyTask.Create(() => RefreshSpotAsync(), onException: ex => LogException(ex));
            }

            base.ViewAppearing();
        }

        public override Task Initialize()
        {
            InitSpotTypeTask = MvxNotifyTask.Create(() => InitSpotTypesAsync(), onException: ex => LogException(ex));

            return base.Initialize();
        }

        public override void Validate()
        {
            SpotModelValidator validator = new SpotModelValidator();
            FluentValidation.Results.ValidationResult result = validator.Validate(Spot);
            Spot.IsValid = result.IsValid;
            ValidationResult = result;

            if (!result.IsValid)
                SetValidationFailures(result.Errors);
        }

        public override async Task SaveAsync()
        {
            try
            {
                IsBusy = true;

                await base.SaveAsync();

                if (IsValid)
                {
                    Spot = await DataStore.SaveItemAsync(Spot);
                    await NavigationService.Close(this, new OperationResult<SpotModel>(Spot, isSaved: true));

                }
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

        public override async Task DeleteAsync()
        {
            try
            {
                await base.DeleteAsync();

                if (!IsNew)
                {
                    bool confirmation = await UserDialog.ConfirmAsync(LocalizeService.Translate("DeleteItemText"),
                        title: LocalizeService.Translate("DeleteCommandTitle"));

                    if (!confirmation)
                    {
                        return;
                    }

                    IsBusy = true;

                    await NavigationService.Close(this, new OperationResult<SpotModel>(Spot, isDeleted: true));

                    return;
                }

                await NavigationService.Close(this, new OperationResult<SpotModel>(Spot, isCanceld: true));
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

        void ExecuteMapClicked(Position? position)
        {
            RefreshSpotMarkers(position);
        }

        void RefreshSpotMarkers(Position? position)
        {
            if (!position.HasValue || Spot.ID_SpotType == null)
            {
                return;
            }

            try
            {
                if (Spot.ID_SpotType == Guid.Parse(TableConsts.SPOTTYPE_SPOT_ID))
                {
                    Spot.SpotMarker.Clear();
                }

                if (Spot.ID_SpotType == Guid.Parse(TableConsts.SPOTTYPE_STRETCH_ID) &&
                    Spot.SpotMarker.Count == 3)
                {
                    Spot.SpotMarker.Clear();
                }

                Spot.SpotMarker.Add(new SpotMarkerModel(true)
                {
                    ID_Spot = Spot.Id,
                    Lat = position.Value.Latitude,
                    Lng = position.Value.Longitude,
                    SpotMarker = Spot.Id.ToString(),
                    Spot = Spot
                });

                RaisePropertyChanged(() => SpotMarker);
                RaisePropertyChanged(() => MapCenter);
            }
            catch (Exception ex)
            {
                throw;
                // handle!
            }
        }

        async Task RefreshSpotTypeAsync()
        {
            if (Spot == null)
            {
                return;
            }

            Spot.SpotType = SpotTypes.Where(s => s.Id == Spot.ID_SpotType).FirstOrDefault();
            Spot.SpotMarker.Clear();

            await RaisePropertyChanged(() => Spot);
            await RaisePropertyChanged(() => SpotMarker);
            await RaisePropertyChanged(() => MapCenter);
            await RaisePropertyChanged(() => HasSelectedSpotType);
        }

        async Task InitSpotTypesAsync()
        {
            SpotTypes = new MvxObservableCollection<SpotTypeModel>(
                await DataServiceFactory.GetSpotTypeFactory().GetItemsAsync()
            );

            await RaisePropertyChanged(() => SpotTypes);
        }

        async Task RefreshSpotAsync()
        {
            if (Spot == null || Spot.IsNew)
            {
                return;
            }

            Spot = await DataStore.GetItemAsync(Spot.Id);

            await RaisePropertyChanged(() => Spot);
        }

        private void SetValidationFailures(IList<ValidationFailure> vf)
        {
            foreach (ValidationFailure f in vf)
            {
                switch (f.PropertyName.ToLower())
                {
                    case "spot":
                        SpotNameErrorText = f.ErrorMessage;
                        break;

                    case "id_spottype":
                        SpotTypeErrorText = f.ErrorMessage;
                        break;

                    case "spotmarker":
                        SpotLocationErrorText = f.ErrorMessage;
                        break;
                }
            }
        }

        #endregion
    }
}
