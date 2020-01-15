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
using System.Threading.Tasks;
using TrackYourTrip.Core.ViewModels.Root;
using Xamarin.Essentials;

namespace TrackYourTrip.Core.ViewModels
{
    public class MainViewModel : MvxNavigationViewModel
    {
        public IMvxAsyncCommand ShowInitialViewModelsCommand { get; private set; }
        public IMvxAsyncCommand ShowTabsRootBCommand { get; private set; }

        private int _itemIndex;
        public int ItemIndex
        {
            get { return _itemIndex; }
            set
            {
                if (_itemIndex == value) return;
                _itemIndex = value;
                RaisePropertyChanged(() => ItemIndex);
            }
        }

        public MainViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) 
            : base(logProvider, navigationService)
        {
            ShowInitialViewModelsCommand = new MvxAsyncCommand(ShowInitialViewModels);
        }

        private async Task ShowInitialViewModels()
        {
            var tasks = new List<Task>();
            tasks.Add(NavigationService.Navigate<SettingsViewModel>());
            tasks.Add(NavigationService.Navigate<DemoViewModel>());
            await Task.WhenAll(tasks);
        }
    }
}