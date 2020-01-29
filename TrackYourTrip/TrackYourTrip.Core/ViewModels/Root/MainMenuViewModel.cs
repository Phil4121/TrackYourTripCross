using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.ViewModels.Root;

[assembly: MvxNavigation(typeof(MainMenuViewModel), @"MainMenuPage")]
namespace TrackYourTrip.Core.ViewModels.Root
{
    public class MainMenuViewModel : BaseViewModel<MenuItemModel>
    {
        public MainMenuViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider)
            : base(Resources.AppResources.MainMenuPageTitle, mvxLogProvider, navigationService)
        {
            SettingTappedCommand = new MvxCommand<string>(
                (param) => NavigationTask = MvxNotifyTask.Create(NavigateAsync(param), onException: ex => LogException(ex))
            );
        }

        public MvxNotifyTask NavigationTask { get; private set; }

        public override IDataServiceFactory<MenuItemModel> DataStore { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override bool IsNew => throw new NotImplementedException();

        public IMvxCommand<string> SettingTappedCommand { get; private set; }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        async Task NavigateAsync(string selectedMenu)
        {
            try
            {
                IsBusy = true;

                var result = await NavigationService.Navigate(MapStringToViewModel(selectedMenu));

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

        private string MapStringToViewModel(string selectedMenu)
        {
            switch (selectedMenu.ToLower())
            {
                case "setting":
                    return "SettingsPage";

                default:
                    throw new Exception("SelectedMenu string not found!");
            }
        }
    }
}
