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

[assembly: MvxNavigation(typeof(NewFishedSpotBasicViewModel), @"NewFishedSpotBasicPage")]
namespace TrackYourTrip.Core.ViewModels.NewTrip
{
    public class NewFishedSpotBasicViewModel : BaseViewModel<FishedSpotModel, OperationResult<IModel>>, ISharedFishedSpotViewModel
    {
        public NewFishedSpotBasicViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider, IUserDialogs userDialog, ILocalizeService localizeService)
            : base(Resources.AppResources.NewFishedSpotBasicPageTitle, mvxLogProvider, navigationService, userDialog, localizeService)
        {

            BiteCommand = new MvxCommand(
                () => NavigationTask = MvxNotifyTask.Create(NavigateToBite(), onException: ex => LogException(ex)));

            CatchCommand = new MvxCommand(
                () => NavigationTask = MvxNotifyTask.Create(NavigateToCatch(), onException: ex => LogException(ex)));

        }

        #region Properties

        private FishedSpotModel _fishedSpot = new FishedSpotModel();
        public FishedSpotModel FishedSpot
        {
            get => _fishedSpot;
            set => SetProperty(ref _fishedSpot, value);
        }

        private IDataServiceFactory<FishedSpotModel> _dataStore;
        public override IDataServiceFactory<FishedSpotModel> DataStore
        {
            get
            {
                if (_dataStore == null)
                {
                    _dataStore = DataServiceFactory.GetFishedSpotFactory();
                }

                return _dataStore;
            }
            set => _dataStore = value;
        }

        public override bool IsNew => throw new NotImplementedException();

        #endregion

        #region Commands

        public IMvxCommand BiteCommand { get; private set; }

        public IMvxCommand CatchCommand { get; private set; }

        #endregion

        #region Tasks

        public MvxNotifyTask NavigationTask { get; private set; }


        #endregion

        #region Methodes

        public override void Prepare(FishedSpotModel parameter)
        {
            base.Prepare(parameter);
            this.FishedSpot = parameter;
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        async Task NavigateToBite()
        {
          try
            {
                IsBusy = true;

                GlobalSettings _globalSettings = new GlobalSettings();

                var bite = new FishedSpotBiteModel()
                {
                    Id = Guid.NewGuid(),
                    ID_FishedSpot = FishedSpot.Id,
                    ID_FishingArea = FishedSpot.ID_FishingArea,
                    ID_Trip = FishedSpot.ID_Trip,
                    BiteDateTime = DateTime.Now,
                    ID_BaitType = _globalSettings.PreDefinedSpotSettings != null ? _globalSettings.PreDefinedSpotSettings.ID_BaitType : Guid.Empty,
                    ID_BaitColor = _globalSettings.PreDefinedSpotSettings != null ? _globalSettings.PreDefinedSpotSettings.ID_BaitColor : Guid.Empty
                };

                await NavigationService.Navigate<NewFishedSpotBiteViewModel, FishedSpotBiteModel, OperationResult<IModel>>(bite);

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

        async Task NavigateToCatch()
        {
            /*  try
              {
                  IsBusy = true;

                  var activeTrip = await DataStore.GetItemAsync(Guid.Parse(TripHelper.GetTripIdInProcess()));

                  Trip = activeTrip;

                  await NavigationService.Navigate<NewTripOverviewViewModel, TripModel, OperationResult<IModel>>(activeTrip);
              }
              catch (Exception ex)
              {
                  TripHelper.ResetTripInProcess();
                  TripHelper.ResetPreSettings();

                  throw;
              }
              finally
              {
                  IsBusy = false;
              } */
        }

        #endregion
    }
}
