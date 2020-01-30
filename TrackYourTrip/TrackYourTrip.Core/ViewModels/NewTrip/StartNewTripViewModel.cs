using Acr.UserDialogs;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using System;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.ViewModels.NewTrip;

[assembly: MvxNavigation(typeof(StartNewTripViewModel), @"StartNewTripPage")]
namespace TrackYourTrip.Core.ViewModels.NewTrip
{
    public class StartNewTripViewModel : BaseViewModel<TripModel>
    {
        public StartNewTripViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider, IUserDialogs userDialog, ILocalizeService localizeService)
            : base(Resources.AppResources.NewTripPageTitle, mvxLogProvider, navigationService, userDialog, localizeService)
        {


        }

        public override IDataServiceFactory<TripModel> DataStore
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override bool IsNew => throw new NotImplementedException();

        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
