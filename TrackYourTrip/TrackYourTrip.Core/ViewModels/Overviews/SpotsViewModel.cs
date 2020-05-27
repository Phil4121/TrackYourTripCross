using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.Services;
using TrackYourTrip.Core.ViewModelResults;
using TrackYourTrip.Core.ViewModels.NewTrip;
using TrackYourTrip.Core.ViewModels.Overviews;
using TrackYourTrip.Core.ViewModels.Settings;

[assembly: MvxNavigation(typeof(SpotsViewModel), @"SpotsPage")]
namespace TrackYourTrip.Core.ViewModels.Overviews
{
    public class SpotsViewModel : BaseOverviewModel<OverviewArgs, FishingAreaModel, SpotsViewModel>
    {
        public SpotsViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider)
            : base(Resources.AppResources.SpotsPageTitle, mvxLogProvider, navigationService)
        {
            SpotSelectedCommand = new MvxCommand<SpotModel>(
                (param) => NavigationTask = MvxNotifyTask.Create(NavigateToSpot(param), onException: ex => LogException(ex))
            );

            SearchSpotsCommand = new MvxCommand<string>((param) =>
                {
                    SearchString = param;
                    RaisePropertyChanged(nameof(Spots));
                }
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

        public MvxObservableCollection<SpotModel> Spots
        {
            get
            {
                if (!string.IsNullOrEmpty(SearchString))
                    return new MvxObservableCollection<SpotModel>(RootFishingArea.Spots.Where(s => s.Spot.ToLower()
                                                                    .Contains(SearchString.ToLower()))
                                                                    .OrderBy(s => s.Spot));

                return new MvxObservableCollection<SpotModel>(RootFishingArea.Spots.OrderBy(s => s.Spot));
            }
        }


        private SpotModel _selectedSpot;
        public SpotModel SelectedSpot
        {
            get => _selectedSpot;
            private set => SetProperty(ref _selectedSpot, value);
        }

        private string SearchString { get; set; }

        #endregion

        #region Tasks

        public MvxNotifyTask NavigationTask { get; private set; }

        public MvxNotifyTask RefreshSpotsTask { get; private set; }

        #endregion

        #region Commands

        public IMvxCommand<SpotModel> SpotSelectedCommand { get; private set; }

        public IMvxCommand<string> SearchSpotsCommand { get; private set; }

        #endregion

        #region Methodes

        public override void Prepare(OverviewArgs parameter)
        {
            this.NavigateBack = parameter.NavigateBack;
            this.NavigateTo = parameter.NavigateTo;

            RootFishingArea = (FishingAreaModel)parameter.Object;
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

                if (!NavigateBack)
                {
                    if (NavigateTo == string.Empty)
                    {
                        OperationResult<SpotModel> result = await NavigationService.Navigate<SpotViewModel, SpotModel, OperationResult<SpotModel>>(spot);

                        if (result != null)
                        {
                            if (result.IsCanceld)
                            {
                                return;
                            }

                            RefreshSpotsTask = MvxNotifyTask.Create(RefreshSpots, onException: ex => LogException(ex));
                            return;
                        }
                    }


                    switch(NavigateTo)
                    {
                        case PageHelper.NEWFISHEDSPOTOVERVIEW_PAGE:
                            await NavigationService.Navigate<NewFishedSpotOverviewViewModel, FishedSpotModel, OperationResult<IModel>>(
                                new FishedSpotModel()
                                    {
                                        Id = Guid.NewGuid(),
                                        ID_Spot = spot.Id,
                                        ID_Trip = Guid.Parse(TripHelper.GetTripIdInProcess()),
                                        StartDateTime = DateTime.Now,
                                        Spot = spot,
                                        IsNew = true
                                    }
                                );
                            return;

                        default:
                            throw new Exception("Navigation destination not known!");

                    }

                }
                else
                {
                    await NavigationService.Close(this);
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
