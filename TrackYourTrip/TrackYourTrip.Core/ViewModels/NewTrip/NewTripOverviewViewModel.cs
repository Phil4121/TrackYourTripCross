using Acr.UserDialogs;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.ViewModelResults;
using TrackYourTrip.Core.ViewModels.NewTrip;

[assembly: MvxNavigation(typeof(NewTripOverviewViewModel), @"NewTripOverviewPage")]
namespace TrackYourTrip.Core.ViewModels.NewTrip
{
    public class NewTripOverviewViewModel : BaseViewModel<TripModel, OperationResult<IModel>>
    {
        public NewTripOverviewViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider, IUserDialogs userDialog, ILocalizeService localizeService)
            : base(Resources.AppResources.NewTripOverviewPageTitle, mvxLogProvider, navigationService, userDialog, localizeService)
        {

        }

        public override IDataServiceFactory<TripModel> DataStore { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override bool IsNew => throw new NotImplementedException();

        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
