﻿using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.IoC;
using MvvmCross.Plugin.Json;
using MvvmCross.ViewModels;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Services;
using TrackYourTrip.Core.Services.BackgroundQueue.Messages;
using Xamarin.Forms;

namespace TrackYourTrip.Core
{
    public class MvxApp : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes().
                EndingWith("Repository")
                .AsTypes()
                .RegisterAsLazySingleton();

            Device.SetFlags(new string[] { "SwipeView_Experimental" });

            Mvx.IoCProvider.RegisterType<IAppSettings, GlobalSettings>();
            Mvx.IoCProvider.RegisterType<IMvxJsonConverter, MvxJsonConverter>();
            Mvx.IoCProvider.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);

            Resources.AppResources.Culture = Mvx.IoCProvider.Resolve<ILocalizeService>().GetCurrentCultureInfo();

            RegisterAppStart<ViewModels.MainViewModel>();
        }
    }
}