using SQLite;
using System;

namespace TrackYourTrip.Core.Interfaces
{
    public interface IDatabase
    {

        string GetDbPath();
        string GetFullDbPath();
        string GetMigrationScriptBaseFolderPath();
        SQLiteConnection GetConnection();
    }
}
