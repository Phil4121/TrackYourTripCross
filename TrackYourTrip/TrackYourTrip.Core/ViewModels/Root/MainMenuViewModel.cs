using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Threading.Tasks;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.ViewModels.Root;

[assembly: MvxNavigation(typeof(MainMenuViewModel), @"MainMenuPage")]
namespace TrackYourTrip.Core.ViewModels.Root
{
    public class MainMenuViewModel : BaseViewModel<MenuItemModel>
    {
        public MainMenuViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider)
            : base(Resources.AppResources.ApplicationName, mvxLogProvider, navigationService)
        {
            SettingTappedCommand = new MvxCommand<string>(
                (param) => NavigationTask = MvxNotifyTask.Create(NavigateAsync(param), onException: ex => LogException(ex))
            );
        }

        #region Properties

        public override IDataServiceFactory<MenuItemModel> DataStore
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override bool IsNew => throw new NotImplementedException();

        public string SettingString => Resources.AppResources.SettingsPageTitle;

        public string NewTripString => Resources.AppResources.NewTripPageTitle;

        #endregion

        #region Tasks

        public MvxNotifyTask NavigationTask { get; private set; }

        #endregion

        #region Commands

        public IMvxCommand<string> SettingTappedCommand { get; private set; }

        #endregion

        #region Methods

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        async Task NavigateAsync(string selectedMenu)
        {
            try
            {
                IsBusy = true;

                bool result = await NavigationService.Navigate(MapStringToViewModel(selectedMenu));

                if (!result)
                {
                    throw new Exception("Navigation not possible!");
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

        string MapStringToViewModel(string selectedMenu)
        {
            switch (selectedMenu.ToLower())
            {
                case "setting":
                    return PageHelper.SETTINGS_PAGE;

                case "startnewtrip":
                    return PageHelper.START_NEW_TRIP_PAGE;

                default:
                    throw new Exception("SelectedMenu string not found!");
            }
        }

        #endregion
    }
}
