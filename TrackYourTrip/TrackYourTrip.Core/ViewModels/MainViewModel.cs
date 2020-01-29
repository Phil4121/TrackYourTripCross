﻿// ---------------------------------------------------------------
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
        private readonly IMvxNavigationService _navigationService;

        public IMvxAsyncCommand ShowMainMenuViewModelCommand { get; private set; }

        public MainViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ShowMainMenuViewModelCommand = new MvxAsyncCommand(ShowMainMenuViewModel);
        }

        private async Task ShowMainMenuViewModel()
        {
            await NavigationService.Navigate<MainMenuViewModel>();
        }
    }
}