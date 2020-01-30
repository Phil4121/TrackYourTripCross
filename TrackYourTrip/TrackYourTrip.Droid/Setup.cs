

using MvvmCross;
using MvvmCross.Forms.Platforms.Android.Core;
using MvvmCross.Logging;
using Serilog;

namespace SmartHome.Droid
{
    public class Setup : MvxFormsAndroidSetup<TrackYourTrip.Core.MvxApp, TrackYourTrip.Core.FormsApp>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.IoCProvider.RegisterSingleton<TrackYourTrip.Core.Interfaces.ILocalizeService>(() => new TrackYourTrip.Droid.Services.LocalizeService());
        }

        public override MvxLogProviderType GetDefaultLogProviderType()
        {
            return MvxLogProviderType.Serilog;
        }

        protected override IMvxLogProvider CreateLogProvider()
        {
            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.AndroidLog()
                        .CreateLogger();

            return base.CreateLogProvider();
        }
    }
}