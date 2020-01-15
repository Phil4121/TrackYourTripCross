using SQLite;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace Database
{
    public static class DatabaseMigrator
    {
        public static bool Migrate(SQLiteConnection connection, string MigrationScriptBaseFolderPath)
        {
            int version = connection.ExecuteScalar<int>("PRAGMA user_version");

            var files = Directory.GetFiles(Path.Combine(MigrationScriptBaseFolderPath), "*.sql");

            if (files.Length == 0)
                return false;

            foreach (var migrationFile in files)
            {
                try
                {
                    if (!int.TryParse(Path.GetFileName(migrationFile).Split('.').First(), out int sqlVersion))
                    {
                        continue;
                    }

                    if (sqlVersion <= version)

                    {
                        continue;
                    }

                    var migrationScript = File.ReadAllText(migrationFile);

                    if (string.IsNullOrEmpty(migrationScript))
                        return false;

                    try
                    {

                        connection.BeginTransaction();

                        foreach (var statement in migrationScript.Split(';'))
                        {
                            if (string.IsNullOrEmpty(statement))
                                continue;

                            if (statement.StartsWith("--"))
                                continue;

                            connection.Execute(statement);
                        }

                        connection.Execute(string.Format("PRAGMA user_version = {0}", sqlVersion));

                        connection.Commit();
                    }
                    catch (Exception e)
                    {
                        connection.Rollback();

                        Console.WriteLine(e.Message);

                        return false;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    return false;
                }
            }

            return true;
        }
    }
}
