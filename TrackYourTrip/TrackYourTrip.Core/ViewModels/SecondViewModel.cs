// ---------------------------------------------------------------
// <author>Paul Datsyuk</author>
// <url>https://www.linkedin.com/in/pauldatsyuk/</url>
// ---------------------------------------------------------------

using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Collections.Generic;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;

namespace TrackYourTrip.Core.ViewModels
{
    public class SecondViewModel : MvxViewModel<Dictionary<string, string>>
    {
        private readonly IMvxNavigationService navigationService;
        private readonly IAppSettings settings;
        private readonly IUserDialogs userDialogs;
        private readonly ILocalizeService localizeService;

        private Dictionary<string, string> _parameter;

        public SecondViewModel(IMvxNavigationService navigationService, IAppSettings settings, IUserDialogs userDialogs, ILocalizeService localizeService)
        {
            this.navigationService = navigationService;
            this.settings = settings;
            this.userDialogs = userDialogs;
            this.localizeService = localizeService;

            MainPageButtonText = "test";
        }

        public string MainPageButtonText { get; set; }

        public IMvxAsyncCommand BackCommand => new MvxAsyncCommand(async () =>
        {
            string localizedText = localizeService.Translate("SecondPage_ByeBye_Localization");

            await userDialogs.AlertAsync(localizedText);
            await navigationService.Close(this);
        });

        public override void Prepare(Dictionary<string, string> parameter)
        {
            _parameter = parameter;

            if (_parameter != null && _parameter.ContainsKey("ButtonText"))
            {
                MainPageButtonText = "ButtonText";
            }
        }

        public int SuperNumber
        {
            get => settings.SuperNumber;
            set => settings.SuperNumber = value;
        }
    }
}