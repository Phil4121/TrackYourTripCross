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
 
[assembly: MvxNavigation(typeof(NewFishedSpotCatchViewModel), @"NewFishedSpotCatchPage")]
namespace TrackYourTrip.Core.ViewModels.NewTrip
{
    public class NewFishedSpotCatchViewModel : BaseViewModel<FishedSpotCatchModel, OperationResult<IModel>>
    {
        public NewFishedSpotCatchViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider, IUserDialogs userDialog, ILocalizeService localizeService)
            : base(Resources.AppResources.NewFishedSpotCatchPageTitle, mvxLogProvider, navigationService, userDialog, localizeService)
        {

        }

        #region Properties

        private FishedSpotCatchModel _fishedCatchSpot = new FishedSpotCatchModel();
        public FishedSpotCatchModel FishedSpotCatch
        {
            get => _fishedCatchSpot;
            set => SetProperty(ref _fishedCatchSpot, value);
        }

        private IDataServiceFactory<FishedSpotCatchModel> _dataStore;
        public override IDataServiceFactory<FishedSpotCatchModel> DataStore
        {
            get
            {
                if (_dataStore == null)
                {
                    //_dataStore = DataServiceFactory.GetFishedSpotFactory();
                }

                return _dataStore;
            }
            set => _dataStore = value;
        }

        public override bool IsNew => throw new NotImplementedException();

        #endregion

        #region Tasks

        public MvxNotifyTask NavigationTask { get; private set; }


        #endregion

        #region Methodes

        public override void Prepare(FishedSpotCatchModel parameter)
        {
            base.Prepare(parameter);
            this.FishedSpotCatch = parameter;
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
