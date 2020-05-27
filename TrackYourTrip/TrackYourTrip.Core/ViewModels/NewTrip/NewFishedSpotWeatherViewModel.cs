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

[assembly: MvxNavigation(typeof(NewFishedSpotWeatherViewModel), @"NewFishedSpotWeatherPage")]
namespace TrackYourTrip.Core.ViewModels.NewTrip
{
    public class NewFishedSpotWeatherViewModel : BaseViewModel<FishedSpotModel, OperationResult<IModel>>, ISharedFishedSpotViewModel
    {
        public NewFishedSpotWeatherViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider, IUserDialogs userDialog, ILocalizeService localizeService)
            : base(Resources.AppResources.NewFishedSpotWeatherPageTitle, mvxLogProvider, navigationService, userDialog, localizeService)
        {

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

        public int BiteCount
        {
            get
            {
                return 0;
            }
        }

        public int FishCount
        {
            get
            {
                return 0;
            }
        }

        #endregion

        #region Tasks

        public MvxNotifyTask NavigationTask { get; private set; }

        #endregion

        #region Methodes

        public override void Prepare(FishedSpotModel parameter)
        {
            base.Prepare(parameter);

            FishedSpot = parameter;
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
