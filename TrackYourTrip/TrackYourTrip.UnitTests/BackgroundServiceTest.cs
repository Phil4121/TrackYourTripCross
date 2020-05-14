﻿using System;
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

            BackgroundQueueService service = new BackgroundQueueService();

            var testSpotGuid = Guid.NewGuid();
            var testLat = 48.45;
            var testLng = 13.9167;

            bool result = service.PushWheaterRequestToBackgroundQueue(_connection, testSpotGuid, testLat, testLng).Result;

            Assert.True(result);
        }

        [Fact]
        public void TestPushAndPopToBackgroundQueue()
        {
            var platformServicesFake = A.Fake<IPlatformServices>();
            Device.PlatformServices = platformServicesFake;

            DatabaseMigrator.Migrate(_connection, _migrationSkriptFolderPath);

            BackgroundQueueService service = new BackgroundQueueService();

            var testSpotGuid = Guid.NewGuid();
            var testLat = 48.45;
            var testLng = 13.9167;

            bool result = service.PushWheaterRequestToBackgroundQueue(_connection, testSpotGuid, testLat, testLng).Result;

            Assert.True(result);

            var testSpotGuid2 = Guid.NewGuid();
            var testLat2 = 48.45;
            var testLng2 = 13.9167;

            bool result2 = service.PushWheaterRequestToBackgroundQueue(_connection, testSpotGuid2, testLat2, testLng2).Result;

            Assert.True(result);


            var model = service.PopWheaterRequestFromBackgroundQueue(_connection).Result;

            Assert.Equal(model.ID_ElementReference.ToString(), testSpotGuid2.ToString());

            var model2 = service.PopWheaterRequestFromBackgroundQueue(_connection, false).Result;

            Assert.Equal(model2.ID_ElementReference.ToString(), testSpotGuid.ToString());
        }

        [Fact]
        public void TestRemoveFromBackgroundQueue()
        {
            var platformServicesFake = A.Fake<IPlatformServices>();
            Device.PlatformServices = platformServicesFake;

            DatabaseMigrator.Migrate(_connection, _migrationSkriptFolderPath);

            BackgroundQueueService service = new BackgroundQueueService();

            var deleteAll = service.EmptyQueue(_connection).Result;

            Assert.True(deleteAll);

            var testSpotGuid = Guid.NewGuid();
            var testLat = 48.45;
            var testLng = 13.9167;

            bool result = service.PushWheaterRequestToBackgroundQueue(_connection, testSpotGuid, testLat, testLng).Result;

            Assert.True(result);

            var model = service.PopWheaterRequestFromBackgroundQueue(_connection).Result;

            result = service.RemoveElementFromQueue(_connection, model).Result;

            Assert.True(result);

            model = service.PopWheaterRequestFromBackgroundQueue(_connection).Result;

            Assert.Null(model);
        }

        public void TestCountBackgroundQueue()
        {
            var platformServicesFake = A.Fake<IPlatformServices>();
            Device.PlatformServices = platformServicesFake;

            DatabaseMigrator.Migrate(_connection, _migrationSkriptFolderPath);

            BackgroundQueueService service = new BackgroundQueueService();

            var testSpotGuid = Guid.NewGuid();
            var testLat = 48.45;
            var testLng = 13.9167;

            bool result = service.PushWheaterRequestToBackgroundQueue(_connection, testSpotGuid, testLat, testLng).Result;

            Assert.True(result);

            int cnt = service.GetQueueElementCount(_connection).Result;

            Assert.Equal(1, cnt);

            var model = service.PopWheaterRequestFromBackgroundQueue(_connection).Result;

            result = service.RemoveElementFromQueue(_connection, model).Result;

            Assert.True(result);

            cnt = service.GetQueueElementCount(_connection).Result;

            Assert.Equal(0, cnt);
        }
    }
}