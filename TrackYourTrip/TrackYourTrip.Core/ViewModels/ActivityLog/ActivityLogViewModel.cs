using Acr.UserDialogs;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.ViewModels.ActivityLog;
using Xamarin.Forms;

[assembly: MvxNavigation(typeof(ActivityLogViewModel), @"ActivityLogPage")]
namespace TrackYourTrip.Core.ViewModels.ActivityLog
{
    public class ActivityLogViewModel : BaseViewModel<ActivityModel>
    {
        public ActivityLogViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider, IUserDialogs userDialog, ILocalizeService localizeService)
            : base(Resources.AppResources.ActivityLogPageTitle, mvxLogProvider, navigationService, userDialog, localizeService)
        {
            ReLoadActivityLogs();

            MessagingCenter.Subscribe<Application>(Application.Current, MessageHelper.ACTIVITY_LIST_CHANGED_MESSAGE, msg => {
                ReLoadActivityLogs();
            });
        }

        public override IDataServiceFactory<ActivityModel> DataStore { 
            get => throw new NotImplementedException(); 
            set => throw new NotImplementedException(); 
        }

        public override bool IsNew => true;

        private List<ActivityModel> _activityLogs;
        public List<ActivityModel> ActivityLogs
        {
            get => _activityLogs;
            set => SetProperty(ref _activityLogs, value);
        }

        #region Methodes

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        public void ReLoadActivityLogs()
        {
            ActivityLogs = ActivityHelper.ActivityList;

            RaisePropertyChanged(nameof(ActivityLogs));
        }

        #endregion
    }
}
