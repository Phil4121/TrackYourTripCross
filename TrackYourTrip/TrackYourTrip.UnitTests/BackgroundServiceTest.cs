using System;
using Xunit;
using TrackYourTrip.Core.Services.BackgroundQueue;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.Helpers;
using FakeItEasy;
using Xamarin.Forms.Internals;
using Xamarin.Forms;
using SQLite;
using System.IO;
using Database;

namespace TrackYourTrip.UnitTests
{
    public class BackgroundServiceTest
    {

        const string _dbFileName = "TrackYourTrip.db";
        const string _migrationScriptsFolderName = "MigrationScripts";

        SQLiteConnection _connection = new SQLiteConnection(_dbFileName);

        string _migrationSkriptFolderPath = Path.Combine(Environment.CurrentDirectory, _migrationScriptsFolderName);

        [Fact]
        public void TestPushIntoBackgroundQueue()
        {
            var platformServicesFake = A.Fake<IPlatformServices>();
            Device.PlatformServices = platformServicesFake;

            DatabaseMigrator.Migrate(_connection, _migrationSkriptFolderPath);

            var testSpotGuid = Guid.NewGuid();
            var testLat = 48.45;
            var testLng = 13.9167;

            var result = BackgroundQueueService.PushWheaterRequestToBackgroundQueue(_connection, testSpotGuid, testLat, testLng).Result;

            Assert.True(result != Guid.Empty);
        }

        [Fact]
        public void TestPushAndPopToBackgroundQueue()
        {
            var platformServicesFake = A.Fake<IPlatformServices>();
            Device.PlatformServices = platformServicesFake;

            DatabaseMigrator.Migrate(_connection, _migrationSkriptFolderPath);

            var testSpotGuid = Guid.NewGuid();
            var testLat = 48.45;
            var testLng = 13.9167;

            var result = BackgroundQueueService.PushWheaterRequestToBackgroundQueue(_connection, testSpotGuid, testLat, testLng).Result;

            Assert.True(result != Guid.Empty);

            var testSpotGuid2 = Guid.NewGuid();
            var testLat2 = 48.45;
            var testLng2 = 13.9167;

            var result2 = BackgroundQueueService.PushWheaterRequestToBackgroundQueue(_connection, testSpotGuid2, testLat2, testLng2).Result;

            Assert.True(result != Guid.Empty);


            var model = BackgroundQueueService.PopWheaterRequestFromBackgroundQueue(_connection).Result;

            Assert.Equal(model.ID_ElementReference.ToString(), testSpotGuid2.ToString());

            var model2 = BackgroundQueueService.PopWheaterRequestFromBackgroundQueue(_connection, false).Result;

            Assert.Equal(model2.ID_ElementReference.ToString(), testSpotGuid.ToString());
        }

        [Fact]
        public void TestRemoveFromBackgroundQueue()
        {
            var platformServicesFake = A.Fake<IPlatformServices>();
            Device.PlatformServices = platformServicesFake;

            DatabaseMigrator.Migrate(_connection, _migrationSkriptFolderPath);

            var deleteAll = BackgroundQueueService.EmptyQueue(_connection).Result;

            Assert.True(deleteAll);

            var testSpotGuid = Guid.NewGuid();
            var testLat = 48.45;
            var testLng = 13.9167;

            var result = BackgroundQueueService.PushWheaterRequestToBackgroundQueue(_connection, testSpotGuid, testLat, testLng).Result;

            Assert.True(result != Guid.Empty);

            var model = BackgroundQueueService.PopWheaterRequestFromBackgroundQueue(_connection).Result;

            bool res = BackgroundQueueService.RemoveElementFromQueue(_connection, model).Result;

            Assert.True(res);

            model = BackgroundQueueService.PopWheaterRequestFromBackgroundQueue(_connection).Result;

            Assert.Null(model);
        }

        public void TestCountBackgroundQueue()
        {
            var platformServicesFake = A.Fake<IPlatformServices>();
            Device.PlatformServices = platformServicesFake;

            DatabaseMigrator.Migrate(_connection, _migrationSkriptFolderPath);

            var testSpotGuid = Guid.NewGuid();
            var testLat = 48.45;
            var testLng = 13.9167;

            var result = BackgroundQueueService.PushWheaterRequestToBackgroundQueue(_connection, testSpotGuid, testLat, testLng).Result;

            Assert.True(result != Guid.Empty);

            int cnt = BackgroundQueueService.GetQueueElementCount(_connection).Result;

            Assert.Equal(1, cnt);

            var model = BackgroundQueueService.PopWheaterRequestFromBackgroundQueue(_connection).Result;

            bool res = BackgroundQueueService.RemoveElementFromQueue(_connection, model).Result;

            Assert.True(res);

            cnt = BackgroundQueueService.GetQueueElementCount(_connection).Result;

            Assert.Equal(0, cnt);
        }
    }
}
