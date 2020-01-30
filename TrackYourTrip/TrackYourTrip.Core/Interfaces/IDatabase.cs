using SQLite;

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
