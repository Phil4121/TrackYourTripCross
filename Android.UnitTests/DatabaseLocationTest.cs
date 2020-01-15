﻿using Android.App;
using Android.Content;
using Android.UnitTest.Database;
using Database;
using System.IO;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Services;
using Xunit;

namespace Android.UnitTests
{
    public class DatabaseLocationTest
    {
        DependencyServiceStub _dependencyService;
        
        public DatabaseLocationTest()
        {
            _dependencyService = new DependencyServiceStub();
            _dependencyService.Register<IDatabase>(new FakeAndroidDbManager());
        }

        [Fact]
        public void TestCopyDatabase()
        {
            var dbHelper = new DatabaseHelper(_dependencyService);
            var fileHelper = new FileHelper();

            fileHelper.CopyFile(Application.Context.Assets.Open(dbHelper.GetDbFilename), dbHelper.GetFullDbPath(), true);

            Assert.True(File.Exists(dbHelper.GetFullDbPath()));
        }
    }
}
