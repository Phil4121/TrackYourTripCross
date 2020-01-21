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

[assembly: MvxNavigation(typeof(SpotViewModel), @"Spot")]
namespace TrackYourTrip.Core.ViewModels.Settings
{
    public class SpotViewModel : BaseViewModel<SpotModel, OperationResult<SpotModel>>
    {
        public SpotViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider, IUserDialogs userDialog, ILocalizeService localizeService)
            : base(Resources.AppResources.SpotPageTitle, mvxLogProvider, navigationService, userDialog, localizeService)
        {
            MapClickedCommand = new MvxCommand<Position?>(ExecuteMapClicked);

            SpotTypeChangedCommand = new MvxCommand(
                () => RefreshSpotTypeTask = MvxNotifyTask.Create(RefreshSpotTypeAsync, onException: ex => LogException(ex))
            );
        }

        #region Properties

        private IDataServiceFactory<SpotModel> _dataStore;
        public override IDataServiceFactory<SpotModel> DataStore
        {
            get
            {
                if (_dataStore == null)
                    _dataStore = DataServiceFactory.GetSpotFactory();

                return _dataStore;
            }
            set { _dataStore = value; }
        }

        public override bool IsNew => Spot.IsNew;

        private MvxObservableCollection<SpotTypeModel> _spotTypes;
        public MvxObservableCollection<SpotTypeModel> SpotTypes
        {
            get => _spotTypes;
            private set => SetProperty(ref _spotTypes, value);
        }

        public SpotModel Spot { get; set; }

        public CameraUpdate MapCenter
        {
            get
            {
                if (Spot != null &&
                    Spot.Lat != 0 &&
                    Spot.Lng != 0)
                    return CameraUpdateFactory.NewPositionZoom(new Position(Spot.Lat, Spot.Lng), 15d);

                var loc = LocationHelper.GetCurrentLocation();
                return CameraUpdateFactory.NewPositionZoom(new Position(loc.Latitude, loc.Longitude), 15d);
            }
        }

        public MvxObservableCollection<Pin> SpotMarker
        {
            get
            {
                if (Spot.Lat == 0 ||
                    Spot.Lng == 0)
                    return new MvxObservableCollection<Pin>();

                return new MvxObservableCollection<Pin>()
                {
                    new Pin
                    {
                        Position = new Position(Spot.Lat, Spot.Lng),
                        Label = !string.IsNullOrEmpty(Spot.Spot) ? Spot.Spot : Spot.Id.ToString()
                    }
                };
            }
        }

        public bool HasSelectedSpotType
        {
            get
            {
                if (Spot == null)
                    return false;

                Guid.TryParse(Spot.ID_SpotType.ToString(), out Guid result);

                RaisePropertyChanged(() => InstructionText);

                return result != Guid.Empty;
            }
        }

        public string InstructionText
        {
            get
            {
                if(Spot == null ||
                    Spot.ID_SpotType == Guid.Empty)
                    return Resources.AppResources.SpotTypeEmptyText;

                return Resources.AppResources.SpotTypeSetText;
            }
        }


        #endregion

        #region Commands

        public IMvxCommand<Position?> MapClickedCommand { get; private set; }

        public IMvxCommand SpotTypeChangedCommand { get; private set; }

        #endregion

        #region Tasks

        public MvxNotifyTask RefreshSpotTypeTask { get; private set; }

        public MvxNotifyTask InitSpotTypeTask { get; private set; }

        #endregion

        #region Methodes

        public override void Prepare(SpotModel parameter)
        {
            Spot = parameter;
        }

        public override Task Initialize()
        {
            InitSpotTypeTask = MvxNotifyTask.Create(InitSpotTypes, onException: ex => LogException(ex));

            return base.Initialize();
        }

        public override void Validate()
        {
            SpotModelValidator validator = new SpotModelValidator();
            var result = validator.Validate(Spot);
            Spot.IsValid = result.IsValid;
            ValidationResult = result;
        }

        public async override Task SaveAsync()
        {
            await base.SaveAsync();

            if (IsValid)
            {
                Spot.IsNew = false;
                await NavigationService.Close(this, new OperationResult<SpotModel>(Spot, isSaved: true));
            }
        }

        public async override Task DeleteCancelAsync()
        {
            await base.DeleteCancelAsync();

            if (!IsNew)
            {
                var confirmation = await UserDialog.ConfirmAsync(LocalizeService.Translate("DeleteItemText"),
                    title: LocalizeService.Translate("DeleteCommandTitle"));

                if (!confirmation)
                    return;

                await NavigationService.Close(this, new OperationResult<SpotModel>(Spot, isDeleted: true));

                return;
            }

            await NavigationService.Close(this, new OperationResult<SpotModel>(Spot, isCanceld: true));
        }

        void ExecuteMapClicked(Position? position)
        {
            RefreshPin(position);
        }

        void RefreshPin(Position? position)
        {
            if (!position.HasValue)
                return;

            try
            {
                Spot.Lat = position.Value.Latitude;
                Spot.Lng = position.Value.Longitude;

                RaisePropertyChanged(() => SpotMarker);
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
                return;

            Spot.SpotType = SpotTypes.Where(s => s.Id == Spot.ID_SpotType).FirstOrDefault();

            await RaisePropertyChanged(() => Spot);
            await RaisePropertyChanged(() => HasSelectedSpotType);
        }

        async Task InitSpotTypes()
        {
            SpotTypes = new MvxObservableCollection<SpotTypeModel>(
                await DataServiceFactory.GetSpotTypeFactory().GetItemsAsync()
            );

            await RaisePropertyChanged(() => InitSpotTypeTask);
        }

        #endregion
    }
}
