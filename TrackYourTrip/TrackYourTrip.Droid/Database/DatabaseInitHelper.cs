using Android.App;
using Android.Content.Res;
using System.IO;
using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Droid.Database
{
    public static class DatabaseInitHelper
    {
        public static void CopyDatabase(string dbFilePath)
        {
            FileHelper fileHelper = new FileHelper();
            DatabaseHelper dbHelper = new DatabaseHelper();

#if DEBUG
            fileHelper.CopyFile(Application.Context.Assets.Open(dbHelper.GetDbFilename), dbFilePath, false);
#else
            fileHelper.CopyFile(Application.Context.Assets.Open(dbHelper.GetDbFilename), dbFilePath, true);
#endif
        }

        public static void CopyDatabaseMigrationFiles(string migrationFilesFolderPath)
        {
            FileHelper fileHelper = new FileHelper();

            AssetManager mng = Application.Context.Assets;
            string[] i = mng.List("");

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
            FileHelper fileHelper = new FileHelper();
            DatabaseHelper dbHelper = new DatabaseHelper();

            string sdcardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;

            fileHelper.CopyFile(Path.Combine(dbHelper.GetDbPath(), dbHelper.GetDbFilename), Path.Combine(sdcardPath, dbHelper.GetDbFilename), true);
        }
    }
}