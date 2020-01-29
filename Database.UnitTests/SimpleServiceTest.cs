using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.Services;
using TrackYourTrip.Models;
using Xunit;

namespace Database.UnitTests
{
    public class SimpleServiceTest
    {
        const string _dbFileName = "TrackYourTrip.db";
        const string _migrationScriptsFolderName = "MigrationScripts";

        SQLiteConnection _connection = new SQLiteConnection(_dbFileName);

        string _migrationSkriptFolderPath = Path.Combine(Environment.CurrentDirectory, _migrationScriptsFolderName);

        public SimpleServiceTest()
        {
            DatabaseMigrator.Migrate(_connection, _migrationSkriptFolderPath);
        }

        [Fact]
        public void TestSimpleDataServiceGetItemsFromWaterModels()
        {
            var simpleService = new SimpleDataService<TestWaterModel>(_connection, "WaterModels");

            var result = simpleService.GetItemsAsync().Result;

            Assert.NotNull(result);

            var waterModels = (List<TestWaterModel>) result;

            Assert.True(waterModels.Count > 0);
        }

        [Fact]
        public void TestSimpleDataServiceGetSingleWaterModelWithID()
        {
            var idNotExists = Guid.NewGuid();
            var idExists = Guid.Parse("2a3eeecf-472c-4b0f-9df0-73386cb3b3f7");

            var simpleService = new SimpleDataService<TestWaterModel>(_connection, "WaterModels");

            var result = simpleService.GetItemAsync(idNotExists).Result;

            Assert.Null(result);

            result = simpleService.GetItemAsync(idExists).Result;

            Assert.NotNull(result);

            Assert.True(result.ID == 10);
        }

        class TestWaterModel
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
    }

    public class SettingsTest
    {
        const string _dbFileName = "TrackYourTrip.db";
        const string _migrationScriptsFolderName = "MigrationScripts";

        SQLiteConnection _connection = new SQLiteConnection(_dbFileName);

        string _migrationSkriptFolderPath = Path.Combine(Environment.CurrentDirectory, _migrationScriptsFolderName);

        public SettingsTest()
        {
            DatabaseMigrator.Migrate(_connection, _migrationSkriptFolderPath);
        }

        [Fact]
        public async void TestGetSettings()
        {
            DataServiceFactory.Connection = _connection;

            var factory = DataServiceFactory.GetSettingFactory();

            Assert.NotNull(factory);

            var settings = await factory.GetItemsAsync();

            Assert.NotNull(settings);

            var settingsList = (List<SettingModel>)settings;

            Assert.True(settingsList.Count > 0);

        }
    }

    public class TestSaveFishingArea
    {
        const string _dbFileName = "TrackYourTrip.db";
        const string _migrationScriptsFolderName = "MigrationScripts";

        SQLiteConnection _connection = new SQLiteConnection(_dbFileName);

        string _migrationSkriptFolderPath = Path.Combine(Environment.CurrentDirectory, _migrationScriptsFolderName);

        public TestSaveFishingArea()
        {
            DatabaseMigrator.Migrate(_connection, _migrationSkriptFolderPath);
        }

        [Fact]
        public void SaveFishingArea()
        {
            DataServiceFactory.Connection = _connection;

            var factory = DataServiceFactory.GetFishingAreaFactory();

            Assert.NotNull(factory);

            var areaId = Guid.NewGuid();
            
            var tstFishingArea = new FishingAreaModel();
            tstFishingArea.Id = areaId;
            tstFishingArea.ID_WaterModel = Guid.Parse("2a3eeecf-472c-4b0f-9df0-73386cb3b3f7");
            tstFishingArea.Lat = 48.46;
            tstFishingArea.Lng = 13.9267;
            tstFishingArea.FishingArea = "Donau Obermühl";
            tstFishingArea.IsNew = true;

            var success = factory.SaveItemAsync(tstFishingArea).Result;

            Assert.True(success.Id != Guid.Empty);

            var storedFishingArea = factory.GetItemAsync(areaId).Result;

            Assert.True(storedFishingArea.Id == areaId);


            var spotId = Guid.NewGuid();
            var newSpot = new SpotModel();
            newSpot.FishingArea = storedFishingArea;
            newSpot.Id = spotId;
            newSpot.Spot = "Zanderfelsen";
            newSpot.ID_FishingArea = storedFishingArea.Id;
            newSpot.ID_SpotType = Guid.Parse("1fb8243b-a672-496b-955a-5930cb706250");
            newSpot.IsNew = true;

            storedFishingArea.Spots.Add(newSpot);

            success = factory.SaveItemAsync(storedFishingArea).Result;

            Assert.True(success.Id != Guid.Empty);
        }


        [Fact]
        public void SaveExistingFishingArea()
        {
            DataServiceFactory.Connection = _connection;

            var factory = DataServiceFactory.GetFishingAreaFactory();

            Assert.NotNull(factory);

            var areaId = Guid.Parse("0c46a2fd-9a56-4663-9ead-0d92ff39db7c");

            var existingArea = factory.GetItemAsync(areaId).Result;

            Assert.NotNull(existingArea);

            var success = factory.SaveItemAsync(existingArea).Result;

            Assert.True(success.Id != Guid.Empty);
        }
    }

    public class TestGetSpot
    {
        const string _dbFileName = "TrackYourTrip.db";
        const string _migrationScriptsFolderName = "MigrationScripts";

        SQLiteConnection _connection = new SQLiteConnection(_dbFileName);

        string _migrationSkriptFolderPath = Path.Combine(Environment.CurrentDirectory, _migrationScriptsFolderName);

        public TestGetSpot()
        {
            DatabaseMigrator.Migrate(_connection, _migrationSkriptFolderPath);
        }

        [Fact]
        public void GetSpot()
        {
            DataServiceFactory.Connection = _connection;

            var factory = DataServiceFactory.GetSpotFactory();

            Assert.NotNull(factory);

            var spotId = Guid.Parse("610b3ec0-0d70-40a6-9d9f-5167a8135410");

            var existingSpot = factory.GetItemAsync(spotId).Result;

            Assert.True(existingSpot.SpotMarker.Count > 0);
        }
    }
}
