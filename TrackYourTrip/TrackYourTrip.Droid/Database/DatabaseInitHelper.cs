using System.IO;
using Android.App;
using Android.Content.Res;
using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Droid.Database
{
    public static class DatabaseInitHelper
    {
        public static void CopyDatabase(string dbFilePath)
        {
            var fileHelper = new FileHelper();
            var dbHelper = new DatabaseHelper();

            fileHelper.CopyFile(Application.Context.Assets.Open(dbHelper.GetDbFilename), dbFilePath, true);
        }

        public static void CopyDatabaseMigrationFiles(string migrationFilesFolderPath)
        {
            var fileHelper = new FileHelper();

            AssetManager mng = Application.Context.Assets;
            var i = mng.List("");

            foreach (string s in i)
            {
                if (s.Contains(".sql"))
                {
                    fileHelper.CopyFile(Application.Context.Assets.Open(s), Path.Combine(migrationFilesFolderPath, s), true);
                }
            }
        }

        public static void CopyDatabaseToSDCard()
        {
            var fileHelper = new FileHelper();
            var dbHelper = new DatabaseHelper();

            var sdcardPath = Android.OS.Environment.ExternalStorageDirectory.Path;

            fileHelper.CopyFile(dbHelper.GetDbPath(), Path.Combine(sdcardPath, dbHelper.GetDbFilename), true);
        }
    }
}