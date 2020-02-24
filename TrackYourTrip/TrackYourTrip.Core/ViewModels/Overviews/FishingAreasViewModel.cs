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
using TrackYourTrip.Core.ViewModels;
using TrackYourTrip.Core.ViewModels.Overviews;
using TrackYourTrip.Core.ViewModels.Root;
using TrackYourTrip.Core.ViewModels.Settings;

[assembly: MvxNavigation(typeof(FishingAreasViewModel), @"FishingAreasPage")]
namespace TrackYourTrip.Core.ViewModels.Overviews
{
    public class FishingAreasViewModel : BaseOverviewModel<OverviewArgs, FishingAreaModel>
    {
        public FishingAreasViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider)
            : base(Resources.AppResources.FishingAreasPageTitle, mvxLogProvider, navigationService)
        {
            FishingAreaSelectedCommand = new MvxCommand<FishingAreaModel>(
                (param) => NavigationTask = MvxNotifyTask.Create(NavigateToFishingAreaAsync(param), onException: ex => LogException(ex))
                );

            SearchFishingAreasCommand = new MvxCommand<string>((param) => {
                    SearchString = param; 
                    RaisePropertyChanged(nameof(FishingAreas));
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

        private MvxObservableCollection<FishingAreaModel> _fishingAreas;
        public MvxObservableCollection<FishingAreaModel> FishingAreas
        {
            get
            {
                if (_fishingAreas == null)
                {
                    return new MvxObservableCollection<FishingAreaModel>();
                }

                if (!string.IsNullOrEmpty(SearchString))
                    return new MvxObservableCollection<FishingAreaModel>(_fishingAreas.Where(fa => fa.FishingArea.ToLower()
                                                                    .Contains(SearchString.ToLower()))
                                                                    .OrderBy(fa => fa.FishingArea));

                return new MvxObservableCollection<FishingAreaModel>(_fishingAreas.OrderBy(fa => fa.FishingArea));
            }

            private set => SetProperty(ref _fishingAreas, value);
        }

        private FishingAreaModel _selectedFishingArea;
        public FishingAreaModel SelectedFishingArea
        {
            get => _selectedFishingArea;
            private set => SetProperty(ref _selectedFishingArea, value);
        }

        private string SearchString { get; set; }

        #endregion

        #region Commands

        public IMvxCommand<FishingAreaModel> FishingAreaSelectedCommand { get; private set; }

        public IMvxCommand<string> SearchFishingAreasCommand { get; private set; }

        #endregion

        #region Tasks

        public MvxNotifyTask LoadAreasTask { get; private set; }

        public MvxNotifyTask NavigationTask { get; private set; }

        #endregion

        #region Methodes

        public override Task Initialize()
        {
            LoadAreasTask = MvxNotifyTask.Create(() => LoadAreasAsync(), onException: ex => LogException(ex));
            return base.Initialize();
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        public override async Task AddAsync()
        {
            await base.AddAsync();
            await NavigateToFishingAreaAsync(new FishingAreaModel(true));
        }

        async Task NavigateToFishingAreaAsync(FishingAreaModel fishingArea)
        {
            if (fishingArea == null)
            {
                return;
            }

            try
            {
                IsBusy = true;


                if (!NavigateBack)
                {
                    OperationResult<IModel> result = await NavigationService.Navigate<FishingAreaViewModel, FishingAreaModel, OperationResult<IModel>>(fishingArea);

                    if (result != null)
                    {
                        if (result.IsCanceld)
                        {
                            return;
                        }

                        LoadAreasTask = MvxNotifyTask.Create(LoadAreasAsync, onException: ex => LogException(ex));
                    }
                }
                else
                {
                    await NavigationService.Close(this, fishingArea);
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

        async Task LoadAreasAsync()
        {
            FishingAreas = new MvxObservableCollection<FishingAreaModel>(
                        await DataStore.GetItemsAsync()
                );
        }

        #endregion
    }
}
