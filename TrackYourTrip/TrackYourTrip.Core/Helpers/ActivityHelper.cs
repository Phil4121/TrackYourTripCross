using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.Services.BackgroundQueue.Messages;
using Xamarin.Forms;

namespace TrackYourTrip.Core.Helpers
{
    public static class ActivityHelper
    {
        static List<ActivityModel> _activityList = new List<ActivityModel>();

        public static List<ActivityModel> ActivityList
        {
            get => _activityList;
            private set => _activityList = value;   
        }

        public static void AddActivityToList(ActivityModel activity)
        {
            ActivityList.Add(activity);

            Device.BeginInvokeOnMainThread(() =>
            {
                MessagingCenter.Send(Application.Current, MessageHelper.ACTIVITY_LIST_CHANGED_MESSAGE);
            });
        }

        public static void ClearActivityList()
        {
            ActivityList.Clear();

            Device.BeginInvokeOnMainThread(() =>
            {
                MessagingCenter.Send(Application.Current, MessageHelper.ACTIVITY_LIST_CHANGED_MESSAGE);
            });
        }
    }
}
