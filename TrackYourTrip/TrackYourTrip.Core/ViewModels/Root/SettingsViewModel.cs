using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Services;
using TrackYourTrip.Core.ViewModels.Root;
using TrackYourTrip.Core.ViewModels.Settings;
using TrackYourTrip.Models;

[assembly: MvxNavigation(typeof(SettingsViewModel), @"SettingsPage")]
namespace TrackYourTrip.Core.ViewModels.Root
{
    public class SettingsViewModel : BaseViewModel<SettingModel>
    {
        public SettingsViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider)
            : base(Resources.AppResources.SettingsPageTitle, mvxLogProvider, navigationService)
        {
            SettingClickedCommand = new MvxCommand<SettingModel>(ExecuteSettingClicked);
        }

        #region Properties

        private IDataServiceFactory<SettingModel> _dataStore;
        public override IDataServiceFactory<SettingModel> DataStore
        {
            get
            {
                if (_dataStore == null)
                    _dataStore = DataServiceFactory.GetSettingFactory();

                return _dataStore;
            }
            set { _dataStore = value; }
        }

        public override bool IsNew => false;

        public MvxObservableCollection<SettingModel> Settings { get; private set; }

        public SettingModel SelectedSetting { get; set; }

        public MvxNotifyTask LoadSettingsTask { get; private set; }

        #endregion

        #region Commands

        public IMvxCommand<SettingModel> SettingClickedCommand { get; private set; }

        #endregion

        #region Methodes

        public override Task Initialize()
        {
            LoadSettingsTask = MvxNotifyTask.Create(LoadSettings);

            return base.Initialize();
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        async Task LoadSettings()
        {
            Settings = new MvxObservableCollection<SettingModel>(
                        await DataStore.GetItemsAsync()
                );
        }

        void ExecuteSettingClicked(SettingModel setting)
        {
            MvxNotifyTask.Create(NavigationService.Navigate(setting.LandingPage), onException: ex => LogException(ex));
        }

        #endregion
    }
}
