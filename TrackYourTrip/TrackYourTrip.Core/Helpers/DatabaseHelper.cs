using SQLite;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Services;
using Xamarin.Forms;

namespace TrackYourTrip.Core.Helpers
{
    public class DatabaseHelper : IDatabase
    {
        private readonly IDependencyService _dependencyService;

        const string _DbFileName = "TrackYourTrip.db";

        public string GetDbFilename
        {
            get
            {
                return _DbFileName;
            }
        }

        private SQLiteConnection _Connection;

        public DatabaseHelper() : this(new DependencyServiceWrapper()){

        }

        public DatabaseHelper(IDependencyService dependencyService)
        {
            _dependencyService = dependencyService;
        }

        public string GetDbPath()
        {
            return _dependencyService.Get<IDatabase>().GetDbPath();
        }

        public string GetFullDbPath()
        {
            return _dependencyService.Get<IDatabase>().GetFullDbPath();
        }

        public string GetMigrationScriptBaseFolderPath()
        {
            return _dependencyService.Get<IDatabase>().GetMigrationScriptBaseFolderPath();
        }

        public SQLiteConnection GetConnection()
        {
            if(_Connection == null)
            {
                _Connection = _dependencyService.Get<IDatabase>().GetConnection();
            }

            return _Connection;
        }

    }
}
