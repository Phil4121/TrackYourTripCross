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
 
[assembly: MvxNavigation(typeof(NewFishedSpotCatchViewModel), @"NewFishedSpotCatchPage")]
namespace TrackYourTrip.Core.ViewModels.NewTrip
{
    public class NewFishedSpotCatchViewModel : BaseViewModel<FishedSpotCatchModel, OperationResult<IModel>>
    {
        public NewFishedSpotCatchViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider, IUserDialogs userDialog, ILocalizeService localizeService)
            : base(Resources.AppResources.NewFishedSpotCatchPageTitle, mvxLogProvider, navigationService, userDialog, localizeService)
        {

        }

        #region Properties

        private FishedSpotCatchModel _fishedCatchSpot = new FishedSpotCatchModel();
        public FishedSpotCatchModel FishedSpotCatch
        {
            get => _fishedCatchSpot;
            set => SetProperty(ref _fishedCatchSpot, value);
        }

        private IDataServiceFactory<FishedSpotCatchModel> _dataStore;
        public override IDataServiceFactory<FishedSpotCatchModel> DataStore
        {
            get
            {
                if (_dataStore == null)
                {
                    _dataStore = DataServiceFactory.GetFishedSpotCatchFactory();
                }

                return _dataStore;
            }
            set => _dataStore = value;
        }

        public override bool IsNew => throw new NotImplementedException();

        #endregion

        #region Tasks

        public MvxNotifyTask NavigationTask { get; private set; }

        public MvxNotifyTask PreFillFieldsTask { get; private set; }

        #endregion

        public Guid SelectedBiteDistanceId
        {
            get
            {
                if (FishedSpotCatch != null)
                    return FishedSpotCatch.ID_BiteDistance;

                return Guid.Empty;
            }
            set
            {
                if (FishedSpotCatch != null)
                    FishedSpotCatch.ID_BiteDistance = value;

                RaisePropertyChanged(nameof(SelectedBiteDistanceId));
            }
        }

        private MvxObservableCollection<BiteDistanceModel> _biteDistances;
        public MvxObservableCollection<BiteDistanceModel> BiteDistances
        {
            get => _biteDistances;
            set => SetProperty(ref _biteDistances, value);
        }

        public Guid SelectedBaitTypeId
        {
            get
            {
                if (FishedSpotCatch != null)
                    return FishedSpotCatch.ID_BaitType;

                return Guid.Empty;
            }
            set
            {
                if (FishedSpotCatch != null)
                    FishedSpotCatch.ID_BaitType = value;

                RaisePropertyChanged(nameof(SelectedBaitTypeId));
            }
        }

        private MvxObservableCollection<BaitTypeModel> _baitTypes;
        public MvxObservableCollection<BaitTypeModel> BaitTypes
        {
            get => _baitTypes;
            set => SetProperty(ref _baitTypes, value);
        }

        public Guid SelectedColorId
        {
            get
            {
                if (FishedSpotCatch != null)
                    return FishedSpotCatch.ID_BaitColor;

                return Guid.Empty;
            }
            set
            {
                if (FishedSpotCatch != null)
                    FishedSpotCatch.ID_BaitColor = value;

                RaisePropertyChanged(nameof(SelectedColorId));
            }
        }

        private MvxObservableCollection<BaitColorModel> _baitColors;
        public MvxObservableCollection<BaitColorModel> BaitColors
        {
            get => _baitColors;
            set => SetProperty(ref _baitColors, value);
        }

        public Guid SelectedFishId
        {
            get
            {
                if (FishedSpotCatch != null)
                    return FishedSpotCatch.ID_Fish;

                return Guid.Empty;
            }
            set
            {
                if (FishedSpotCatch != null)
                    FishedSpotCatch.ID_Fish = value;

                RaisePropertyChanged(nameof(SelectedFishId));
            }
        }

        private MvxObservableCollection<FishModel> _fishes;
        public MvxObservableCollection<FishModel> Fishes
        {
            get => _fishes;
            set => SetProperty(ref _fishes, value);
        }

        #region Methodes

        public override Task Initialize()
        {
            PreFillFieldsTask = MvxNotifyTask.Create(() => PreFillFieldsAsync(), onException: ex => LogException(ex));

            return base.Initialize();
        }

        public override void Prepare(FishedSpotCatchModel parameter)
        {
            base.Prepare(parameter);
            this.FishedSpotCatch = parameter;
        }

        public override void Validate()
        {
            FishedSpotCatch.IsValid = true;
        }

        async Task PreFillFieldsAsync()
        {
            BiteDistances = new MvxObservableCollection<BiteDistanceModel>(
                await DataServiceFactory.GetBiteDistanceFactory().GetItemsAsync()
                );

            await RaisePropertyChanged(() => BiteDistances);

            BaitTypes = new MvxObservableCollection<BaitTypeModel>(
                await DataServiceFactory.GetBaitTypeFactory().GetItemsAsync()
                );

            await RaisePropertyChanged(() => BaitTypes);

            BaitColors = new MvxObservableCollection<BaitColorModel>(
                await DataServiceFactory.GetBaitColorFactory().GetItemsAsync()
                );

            await RaisePropertyChanged(() => BaitColors);

            Fishes = new MvxObservableCollection<FishModel>(
                await DataServiceFactory.GetFishFactory().GetItemsAsync()
                );

            await RaisePropertyChanged(() => Fishes);
        }

        public override async Task SaveAsync()
        {
            try
            {
                IsBusy = true;

                await base.SaveAsync();

                if (IsValid)
                {
                    await DataStore.SaveItemAsync(FishedSpotCatch);

                    var fishedSpot = await DataServiceFactory.GetFishedSpotFactory().GetItemAsync(FishedSpotCatch.ID_FishedSpot);

                    await NavigationService.Navigate<NewFishedSpotOverviewViewModel, FishedSpotModel>(fishedSpot);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}
