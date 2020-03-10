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

[assembly: MvxNavigation(typeof(NewTripOverviewViewModel), @"NewTripOverviewPage")]
namespace TrackYourTrip.Core.ViewModels.NewTrip
{
    public class NewTripOverviewViewModel : BaseViewModel<TripModel, OperationResult<IModel>>
    {
        public NewTripOverviewViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider, IUserDialogs userDialog, ILocalizeService localizeService)
            : base(Resources.AppResources.NewTripOverviewPageTitle, mvxLogProvider, navigationService, userDialog, localizeService)
        {

        }

        #region Properties

        private bool _firstTime = true;

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

        #region Methodes

        public override void Prepare(TripModel parameter)
        {
            Trip = parameter;

            base.Prepare(parameter);
        }

        public override void Validate()
        {
            throw new NotImplementedException();
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

        private Task ShowInitialViewModels()
        {
            var tasks = new List<Task>();

            try
            {
                IsBusy = true;

                tasks.Add(NavigationService.Navigate<NewTripOverviewBasicViewModel, TripModel, OperationResult<IModel>>(Trip));

            }catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsBusy = false;
            }

            return Task.WhenAll(tasks);
        }

        #endregion
    }
}
