using Android.UnitTest.Database;
using SQLite;
using System.IO;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(FakeAndroidDbManager))]

namespace Android.UnitTest.Database
{
    public class FakeAndroidDbManager : IDatabase
    {
        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(GetFullDbPath());
        }

        public string GetDbPath()
        {
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        }

        public string GetFullDbPath()
        {
            DatabaseHelper dbHelper = new DatabaseHelper();

            return Path.Combine(GetDbPath(), dbHelper.GetDbFilename);
        }

        public string GetMigrationScriptBaseFolderPath()
        {
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        }
    }
}