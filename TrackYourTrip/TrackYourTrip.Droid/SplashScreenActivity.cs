

using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross;
using MvvmCross.Forms.Platforms.Android.Views;
using MvvmCross.Platforms.Android;
using SmartHome.Droid;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TrackYourTrip.Droid
{
    [Activity(
        Label = "TrackYourTrip"
        , MainLauncher = true
        //, Icon = "@mipmap/icon"
        , Theme = "@style/Theme.Splash"
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreenActivity : MvxFormsSplashScreenActivity<Setup, Core.MvxApp, Core.FormsApp>
    {
        public SplashScreenActivity()
            : base(Resource.Layout.SplashScreen)
        {
        }

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            UserDialogs.Init(() => Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity);

            // Leverage controls' StyleId attrib. to Xamarin.UITest
            Forms.ViewInitialized += (object sender, ViewInitializedEventArgs e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.View.StyleId))
                {
                    e.NativeView.ContentDescription = e.View.StyleId;
                }
            };

            base.OnCreate(bundle);
        }

        protected override Task RunAppStartAsync(Bundle bundle)
        {
            //AppCenter.Start(
            //    "",
            //    typeof(Analytics),
            //    typeof(Crashes));

            StartActivity(typeof(FormsApplicationActivity));
            return Task.CompletedTask;
        }
    }
}