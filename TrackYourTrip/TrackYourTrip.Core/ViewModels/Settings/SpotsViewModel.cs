using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.Services;
using TrackYourTrip.Core.ViewModelResults;
using TrackYourTrip.Core.ViewModels.Settings;

[assembly: MvxNavigation(typeof(SpotsViewModel), @"SpotsPage")]
namespace TrackYourTrip.Core.ViewModels.Settings
{
    public class SpotsViewModel : BaseViewModel<FishingAreaModel, OperationResult<FishingAreaModel>>
    {
        public SpotsViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider)
            : base(Resources.AppResources.SpotsPageTitle, mvxLogProvider, navigationService)
        {
            SpotSelectedCommand = new MvxCommand<SpotModel>(
                (param) => NavigationTask = MvxNotifyTask.Create(NavigateToSpot(param), onException: ex => LogException(ex))
            );
        }

        #region Properties

        private IDataServiceFactory<FishingAreaModel> _dataStore;
        public override IDataServiceFactory<FishingAreaModel> DataStore
        {
            get
            {
                if (_dataStore == null)
                {
                    _dataStore = DataServiceFactory.GetFishingAreaFactory();
                }

                return _dataStore;
            }
            set => _dataStore = value;
        }

        public override bool IsNew => false;

        private FishingAreaModel _rootFishingArea;
        public FishingAreaModel RootFishingArea
        {
            get => _rootFishingArea;
            private set => SetProperty(ref _rootFishingArea, value);
        }

        public MvxObservableCollection<SpotModel> Spots => new MvxObservableCollection<SpotModel>(RootFishingArea.Spots.OrderBy(s => s.Spot));


        private SpotModel _selectedSpot;
        public SpotModel SelectedSpot
        {
            get => _selectedSpot;
            private set => SetProperty(ref _selectedSpot, value);
        }

        #endregion

        #region Tasks

        public MvxNotifyTask NavigationTask { get; private set; }

        public MvxNotifyTask RefreshSpotsTask { get; private set; }

        #endregion

        #region Commands

        public IMvxCommand<SpotModel> SpotSelectedCommand { get; private set; }

        #endregion

        #region Methodes

        public override void Prepare(FishingAreaModel parameter)
        {
            RootFishingArea = parameter;
        }

        public override void Validate()
        {
            // nothing to do
        }

        public override async Task AddAsync()
        {
            await base.AddAsync();

            SpotModel spot = new SpotModel(true);
            spot.ID_FishingArea = RootFishingArea.Id;

            await NavigateToSpot(spot);
        }

        async Task RefreshSpots()
        {
            RootFishingArea = await DataStore.GetItemAsync(RootFishingArea.Id);

            await RaisePropertyChanged(() => RootFishingArea);
            await RaisePropertyChanged(() => Spots);
        }

        async Task NavigateToSpot(SpotModel spot)
        {
            if (spot == null)
            {
                return;
            }

            try
            {
                IsBusy = true;

                OperationResult<SpotModel> result = await NavigationService.Navigate<SpotViewModel, SpotModel, OperationResult<SpotModel>>(spot);

                if (result != null)
                {
                    if (result.IsCanceld)
                    {
                        return;
                    }

                    RefreshSpotsTask = MvxNotifyTask.Create(RefreshSpots, onException: ex => LogException(ex));
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

        #endregion
    }
}
