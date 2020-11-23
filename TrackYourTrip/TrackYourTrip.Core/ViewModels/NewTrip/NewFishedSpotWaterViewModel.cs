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
using MvvmCross;

[assembly: MvxNavigation(typeof(NewFishedSpotWaterViewModel), @"NewFishedSpotWaterPage")]
namespace TrackYourTrip.Core.ViewModels.NewTrip
{
    public class NewFishedSpotWaterViewModel : BaseViewModel<FishedSpotModel, OperationResult<IModel>>, ISharedFishedSpotViewModel
    {
        public NewFishedSpotWaterViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider, IUserDialogs userDialog, ILocalizeService localizeService)
            : base(Resources.AppResources.NewFishedSpotWaterPageTitle, mvxLogProvider, navigationService, userDialog, localizeService)
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

        private PreDefinedSpotSettings _preSettings;
        public PreDefinedSpotSettings PreSettings
        {
            get
            {
                if (_preSettings == null)
                {
                    _preSettings = GetPreSettingsAsync();
                }

                return _preSettings;
            }
            set => SetProperty(ref _preSettings, value);
        }

        private MvxObservableCollection<WaterColorModel> _waterColors;
        public MvxObservableCollection<WaterColorModel> WaterColors
        {
            get => _waterColors;
            set => SetProperty(ref _waterColors, value);
        }

        private MvxObservableCollection<TurbidityModel> _turbidities;
        public MvxObservableCollection<TurbidityModel> Turbidities
        {
            get => _turbidities;
            set => SetProperty(ref _turbidities, value);
        }

        private MvxObservableCollection<CurrentModel> _currents;
        public MvxObservableCollection<CurrentModel> Currents
        {
            get => _currents;
            set => SetProperty(ref _currents, value);
        }

        public override bool IsNew
        {
            get => FishedSpot.IsNew;
        }

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

        #region Tasks

        public MvxNotifyTask PreFillFieldsTask { get; private set; }

        #endregion

        #region Methodes

        public override Task Initialize()
        {
            PreFillFieldsTask = MvxNotifyTask.Create(() => PreFillFieldsAsync(), onException: ex => LogException(ex));

            return base.Initialize();
        }

        public override void Prepare(FishedSpotModel parameter)
        {
            base.Prepare(parameter);

            FishedSpot = parameter;
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        private PreDefinedSpotSettings GetPreSettingsAsync()
        {
            IAppSettings settings = Mvx.IoCProvider.Resolve<IAppSettings>();
            return settings.PreDefinedSpotSettings;
        }

        async Task PreFillFieldsAsync()
        {
            Turbidities = new MvxObservableCollection<TurbidityModel>(
                await DataServiceFactory.GetTurbidityFactory().GetItemsAsync()
                );

            await RaisePropertyChanged(() => Turbidities);

            WaterColors = new MvxObservableCollection<WaterColorModel>(
                await DataServiceFactory.GetWaterColorFactory().GetItemsAsync()
                );

            await RaisePropertyChanged(() => WaterColors);

            Currents = new MvxObservableCollection<CurrentModel>(
                await DataServiceFactory.GetCurrentFactory().GetItemsAsync()
                );

            await RaisePropertyChanged(() => Currents);
        }

        #endregion
    }
}
