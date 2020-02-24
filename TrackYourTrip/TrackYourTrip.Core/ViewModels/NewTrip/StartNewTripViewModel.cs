using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Threading.Tasks;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.ViewModelResults;
using TrackYourTrip.Core.ViewModels.NewTrip;
using TrackYourTrip.Core.ViewModels.Overviews;

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
        }

        #region Properties

        private TripModel _trip = new TripModel();

        public TripModel Trip
        {
            get => _trip;
            set => SetProperty(ref _trip, value);
        }

        public override IDataServiceFactory<TripModel> DataStore
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override bool IsNew => throw new NotImplementedException();

        #endregion

        #region Commands

        public IMvxCommand FishingAreaSelectedCommand { get; private set; }

        #endregion

        #region Tasks

        public MvxNotifyTask NavigationTask { get; private set; }

        #endregion

        #region Methodes

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        async Task NavigateToFishingAreasSelectionAsync()
        {
            try
            {
                IsBusy = true;

                var result = await NavigationService.Navigate<OverviewArgs, FishingAreaModel>(PageHelper.FISHING_AREA_PAGE, new OverviewArgs(true));

                    if (result != null)
                    {
                        Trip.FishingArea = result;
                        Trip.ID_FishingArea = result.Id;

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

        #endregion
    }
}
