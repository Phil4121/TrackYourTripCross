using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.Services;
using TrackYourTrip.Core.ViewModelResults;
using TrackYourTrip.Core.ViewModels.NewTrip;
using Xamarin.Forms.GoogleMaps;

[assembly: MvxNavigation(typeof(NewFishedSpotOverviewViewModel), @"NewFishedSpotOverviewPage")]
namespace TrackYourTrip.Core.ViewModels.NewTrip
{
    public class NewFishedSpotOverviewViewModel : BaseViewModel<FishedSpotModel, OperationResult<IModel>>
    {
        public NewFishedSpotOverviewViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider, IUserDialogs userDialog, ILocalizeService localizeService)
            : base(Resources.AppResources.NewTripOverviewPageTitle, mvxLogProvider, navigationService, userDialog, localizeService)
        {

        }

        #region Properties

        private bool _firstTime = true;

        private FishedSpotModel _fishedSpot = new FishedSpotModel();
        public FishedSpotModel FishedSpotModel
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

        public override bool IsNew
        {
            get
            {
                return FishedSpotModel.IsNew;
            }
        }

        #endregion

        #region Methodes

        public override void Prepare(FishedSpotModel parameter)
        {
            FishedSpotModel = parameter;
            base.Prepare(parameter);
        }

        public override void Validate()
        {
            FishedSpotModel.IsValid = true;
        }

        public override void ViewAppearing()
        {
            base.ViewAppearing();

            if (_firstTime)
            {
                ShowInitialViewModels();
                _firstTime = false;
            }
        }

        public override async Task SaveAsync()
        {

            try
            {
                IsBusy = true;

                await base.SaveAsync();

                if (IsValid)
                {
                    await DataStore.SaveItemAsync(FishedSpotModel);

                    await NavigationService.Navigate<NewTripOverviewViewModel, TripModel, OperationResult<IModel>>(FishedSpotModel.Trip);
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

        private Task ShowInitialViewModels()
        {
            var tasks = new List<Task>
            {
                NavigationService.Navigate<NewFishedSpotBasicViewModel, FishedSpotModel, OperationResult<IModel>>(FishedSpotModel),
                NavigationService.Navigate<NewFishedSpotWaterViewModel, FishedSpotModel, OperationResult<IModel>>(FishedSpotModel),
                NavigationService.Navigate<NewFishedSpotWeatherViewModel, FishedSpotModel, OperationResult<IModel>>(FishedSpotModel)
            };


            return Task.WhenAll(tasks);
        }

        #endregion
    }
}
