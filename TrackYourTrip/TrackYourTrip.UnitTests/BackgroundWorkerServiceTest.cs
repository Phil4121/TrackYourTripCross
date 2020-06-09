using Database;
using FakeItEasy;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Services.BackgroundQueue;
using TrackYourTrip.Core.Services.BackgroundQueue.Messages;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TrackYourTrip.UnitTests
{
    public class BackgroundWorkerServiceTest
    {
        const string _dbFileName = "TrackYourTrip.db";
        const string _migrationScriptsFolderName = "MigrationScripts";

        SQLiteConnection _connection = new SQLiteConnection(_dbFileName);

        public BackgroundWorkerServiceTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        string _migrationSkriptFolderPath = Path.Combine(Environment.CurrentDirectory, _migrationScriptsFolderName);

        private readonly ITestOutputHelper output;

        [Fact]
        public async void TestRunBackgroundWorker()
        {
            var platformServicesFake = A.Fake<IPlatformServices>();
            Device.PlatformServices = platformServicesFake;

            DatabaseMigrator.Migrate(_connection, _migrationSkriptFolderPath);


            MessagingCenter.Subscribe<StartBackgroundWorkingServiceMessage>(this, MessageHelper.START_BACKGROUND_WORKING_SERVICE_MESSAGE, message =>
            {
                var msg = new DiagnosticMessage("Start Background Working Service");
                output.WriteLine("{0}", msg.Message);

            });

            MessagingCenter.Subscribe<StopBackgroundWorkingServiceMessage>(this, MessageHelper.STOP_BACKGROUND_WORKING_SERVICE_MESSAGE, message =>
            {
                var msg = new DiagnosticMessage("Stop Background Working Service");
                output.WriteLine("{0}", msg.Message);
            });

            MessagingCenter.Subscribe<ElementFinishedMessage>(this, MessageHelper.ELEMENT_FINISHED_MESSAGE, message =>
            {
                var msg = new DiagnosticMessage("Element processed");
                output.WriteLine("{0}", msg.Message);

            });

            var deleteAll = BackgroundQueueService.EmptyQueue(_connection).Result;

            Assert.True(deleteAll);

            var testSpotGuid = Guid.NewGuid();
            var testLat = 48.45;
            var testLng = 13.9167;

            bool result = BackgroundQueueService.PushWheaterRequestToBackgroundQueue(_connection, testSpotGuid, testLat, testLng).Result;

            Assert.True(result);

            await BackgroundWorkerService.RunBackgroundWorkerService(_connection, new CancellationToken(false));

            int cnt = BackgroundQueueService.GetQueueElementCount(_connection).Result;

            Assert.Equal(0, cnt);
        }
    }
}
