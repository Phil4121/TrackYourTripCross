

using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using System;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Droid.Database;

namespace SmartHome.Droid
{
    //You can specify additional application information in this attribute
#if DEBUG
    [Application(Debuggable = true)]
#else
    [Application(Debuggable = false)]
#endif

    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        private bool CopyCompleted { get; set; }

        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          : base(handle, transer)
        {
            CopyCompleted = false;
        }

        public override void OnCreate()
        {
            base.OnCreate();

            RegisterActivityLifecycleCallbacks(this);
            //A great place to initialize Xamarin.Insights and Dependency Services!
        }

        public override void OnTerminate()
        {
            base.OnTerminate();

            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {

            if (!CopyCompleted)
            {
                DatabaseHelper DbHelper = new DatabaseHelper();

                DatabaseInitHelper.CopyDatabase(DbHelper.GetFullDbPath());
                DatabaseInitHelper.CopyDatabaseMigrationFiles(DbHelper.GetMigrationScriptBaseFolderPath());

                Database.DatabaseMigrator.Migrate(DbHelper.GetConnection(), DbHelper.GetMigrationScriptBaseFolderPath());

#if DEBUG
                DatabaseInitHelper.CopyDatabaseToSDCard();
#endif

                CopyCompleted = true;

            }
        }

        public void OnActivityDestroyed(Activity activity)
        {

        }

        public void OnActivityPaused(Activity activity)
        {

        }

        public void OnActivityResumed(Activity activity)
        {

        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {

        }

        public void OnActivityStarted(Activity activity)
        {

        }

        public void OnActivityStopped(Activity activity)
        {

        }
    }
}