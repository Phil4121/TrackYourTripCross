using SQLite;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Database.UnitTests
{
    public class DatabaseStructureTest
    {
        readonly ITestOutputHelper output;

        const string _dbFileName = "TrackYourTrip.db";
        const string _migrationScriptsFolderName = "MigrationScripts";

        SQLiteConnection _connection = new SQLiteConnection(_dbFileName);

        string _migrationSkriptFolderPath = Path.Combine(Environment.CurrentDirectory, _migrationScriptsFolderName);

        public DatabaseStructureTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void TestUpdateScriptCompleted()
        {
            Assert.True(DatabaseMigrator.Migrate(_connection, _migrationSkriptFolderPath));
        }

        [Fact]
        public void TestTablesExists()
        {
            bool allTablesExists = true;

            var tables = Helper.GetTableNameConstants();

            foreach(string table in tables)
            {
                var exists = _connection.ExecuteScalar<string>(string.Format("SELECT name FROM sqlite_master WHERE type = 'table' AND name = '{0}'", table));

                if (string.IsNullOrEmpty(exists))
                {
                    allTablesExists = false;
                    output.WriteLine(string.Format("Table does not exists: {0}", table));
                    continue;
                }
            }

            Assert.True(allTablesExists);
        }
    }
}
