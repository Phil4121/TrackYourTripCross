using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using SQLite;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Droid.Database;


[assembly: Xamarin.Forms.Dependency(typeof(AndroidDbManager))]

namespace TrackYourTrip.Droid.Database
{
    public class AndroidDbManager : IDatabase
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
            var dbHelper = new DatabaseHelper();

            return Path.Combine(GetDbPath(), dbHelper.GetDbFilename);
        }

        public string GetMigrationScriptBaseFolderPath()
        {
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        }
    }
}