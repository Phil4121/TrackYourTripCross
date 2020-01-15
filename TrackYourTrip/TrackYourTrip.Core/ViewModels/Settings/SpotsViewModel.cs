using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
            SpotSelectedCommand = new MvxAsyncCommand<SpotModel>(NavigateToSpot);
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

        public override bool IsNew => false;

        private FishingAreaModel _rootFishingArea;
        public FishingAreaModel RootFishingArea
        {
            get => _rootFishingArea;
            private set => SetProperty(ref _rootFishingArea, value);
        }

        public MvxObservableCollection<SpotModel> Spots
        {
            get => new MvxObservableCollection<SpotModel>(RootFishingArea.Spots);
        }


        private SpotModel _selectedSpot;
        public SpotModel SelectedSpot
        {
            get => _selectedSpot;
            private set
            {
                SetProperty(ref _selectedSpot, value);
                SpotSelectedCommand.ExecuteAsync(value);
            }
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand<SpotModel> SpotSelectedCommand { get; private set; }

        #endregion

        #region Methodes

        public override void Prepare(FishingAreaModel parameter)
        {
            RootFishingArea = parameter;
        }

        public override void Validate()
        {
            // not needed
        }

        public override void Add()
        {
            base.Add();

            var spot = new SpotModel(true);
            spot.ID_FishingArea = RootFishingArea.Id;

            MvxNotifyTask.Create(NavigateToSpot(spot), onException: ex => LogException(ex));
        }

        async Task NavigateToSpot(SpotModel spot)
        {
            if (spot == null)
                return;

            IsBusy = true;

            var result = await NavigationService.Navigate<SpotViewModel, SpotModel, OperationResult<SpotModel>>(spot);

            IsBusy = false;

            if (result != null)
            {

                if (result.IsCanceld)
                    return;

                var s = RootFishingArea.Spots.FirstOrDefault(a => a.Id == result.Entity.Id);

                if (s != null)
                    RootFishingArea.Spots.Remove(s);

                if (!result.IsDeleted)
                    RootFishingArea.Spots.Add(result.Entity);

                if (!RootFishingArea.IsNew)
                    Save();

                await RaisePropertyChanged(() => RootFishingArea);
                await RaisePropertyChanged(() => Spots);

            }
        }

        public override void Save()
        {
            base.Save();

            if (RootFishingArea.IsValid)
            {
                Task.Run<bool>(() => DataStore.SaveItemAsync(RootFishingArea));
            }
        }

        #endregion
    }
}
