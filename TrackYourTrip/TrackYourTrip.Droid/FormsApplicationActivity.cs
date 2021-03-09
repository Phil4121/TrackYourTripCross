using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using MvvmCross.Forms.Platforms.Android.Views;
using System;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.Services.BackgroundQueue;
using TrackYourTrip.Core.Services.BackgroundQueue.Messages;
using TrackYourTrip.Droid.Services.BackgroundService;
using Xamarin.Forms;
using static TrackYourTrip.Core.Helpers.EnumHelper;

namespace TrackYourTrip.Droid
{
    [Activity(Label = "FormsApplicationActivity",
              ScreenOrientation = ScreenOrientation.Portrait,
              LaunchMode = LaunchMode.SingleTask)]
    public class FormsApplicationActivity : MvxFormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            ToolbarResource = Resource.Layout.Toolbar;
            TabLayoutResource = Resource.Layout.Tabbar;

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            MessagingCenter.Subscribe<StartBackgroundWorkingServiceMessage>(this, MessageHelper.START_BACKGROUND_WORKING_SERVICE_MESSAGE, msg => {
                var intent = new Intent(this, typeof(BackgroundTaskService));
                StartService(intent);
            });

            MessagingCenter.Subscribe<StopBackgroundWorkingServiceMessage>(this, MessageHelper.STOP_BACKGROUND_WORKING_SERVICE_MESSAGE, msg => {
                var intent = new Intent(this, typeof(BackgroundTaskService));
                StopService(intent);
            });

            MessagingCenter.Subscribe<ElementFinishedMessage>(this, MessageHelper.ELEMENT_FINISHED_MESSAGE, msg => {                 
                ActivityHelper.AddActivityToList(new ActivityModel()
                    {
                        TaskType = EnumHelper.ParseEnum<TaskTypeEnum>(msg.BackgroundTask.ID_TaskType.ToString()),
                        AdditionalText = msg.BackgroundTask.TaskResponse,
                        Description = msg.BackgroundTask.TaskData,
                        Id = Guid.NewGuid(),
                        ExecutionDateTime = DateTime.Now
                    });
            });

            var message = new StartBackgroundWorkingServiceMessage();
            MessagingCenter.Send(message, MessageHelper.START_BACKGROUND_WORKING_SERVICE_MESSAGE);

            XF.Material.Droid.Material.Init(this, bundle);

            base.OnCreate(bundle);

            Xamarin.Essentials.Platform.Init(this, bundle);

            Xamarin.FormsGoogleMaps.Init(this, bundle);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}