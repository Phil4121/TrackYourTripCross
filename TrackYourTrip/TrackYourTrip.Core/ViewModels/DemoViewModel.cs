// ---------------------------------------------------------------
// <author>Paul Datsyuk</author>
// <url>https://www.linkedin.com/in/pauldatsyuk/</url>
// ---------------------------------------------------------------

using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using TrackYourTrip.Core.Interfaces;

namespace TrackYourTrip.Core.ViewModels
{
    public class DemoViewModel : MvxNavigationViewModel
    {
        private readonly IMvxNavigationService navigationService;
        private readonly IMvxLogProvider mvxLogProvider;
        private readonly IAppSettings settings;
        private readonly IUserDialogs userDialogs;

        private readonly IMvxLog log;

        public DemoViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider, IAppSettings settings, IUserDialogs userDialogs)
            : base(mvxLogProvider, navigationService)
        {
            this.navigationService = navigationService;
            this.mvxLogProvider = mvxLogProvider;
            this.settings = settings;
            this.userDialogs = userDialogs;

            this.log = mvxLogProvider.GetLogFor(GetType());

            ButtonText = Resources.AppResources.MainPageButton;
        }

        public IMvxCommand PressMeCommand =>
            new MvxCommand(() =>
            {
                ButtonText = Resources.AppResources.MainPageButtonPressed;
            });

        public IMvxAsyncCommand GoToSecondPageCommand =>
            new MvxAsyncCommand(async () =>
            {
                var param = new Dictionary<string, string> { { "ButtonText", ButtonText } };

                await navigationService.Navigate<SecondViewModel, Dictionary<string, string>>(param);
            });

        public IMvxCommand OpenUrlCommand =>
            new MvxAsyncCommand<string>(async (url) =>
            {
                await Browser.OpenAsync(url, BrowserLaunchMode.External);
            });

        public IMvxCommand WriteLogCommand =>
            new MvxCommand(() =>
            {
                log.Log(MvxLogLevel.Debug, () => "Something in the Log", new Exception("Unknown exception occurred"));
            });

        public IMvxAsyncCommand MasterDetailModeCommand =>
            new MvxAsyncCommand(async () =>
            {
                await userDialogs.AlertAsync("Uncomment \n//[MvxMasterDetailPagePresentation] \n//RegisterAppStart<ViewModels.RootViewModel>(); \nand relaunch again");
            });

        public IMvxAsyncCommand ToolbarTestClickCommand =>
            new MvxAsyncCommand(async () =>
            {
                await userDialogs.AlertAsync("This is Toolbar Item Click");
            });

        public string ButtonText { get; set; }

        public int SuperNumber
        {
            get { return settings.SuperNumber; }
            set { settings.SuperNumber = value; }
        }
    }
}