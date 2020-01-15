using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            var idExists = Guid.Parse("10");

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
}
