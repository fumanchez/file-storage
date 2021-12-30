using Microsoft.Data.Sqlite;

namespace FileStorage.Api.Services;

public partial class SqliteStorage
{
    private class CreateFilesTableQuery
    {
        private readonly SqliteConnection _connection;

        public CreateFilesTableQuery(SqliteConnection connection)
        {
            _connection = connection;
        }

        public void Execute()
        {
            const string commandText =
                @"create table if not exists files (" +
                    "id integer primary key autoincrement" +
                    ", name text not null" +
                    ", content blob not null" +
                    ", json blob" +
                    ")";

            using var command = _connection.CreateCommand();
            command.CommandText = commandText;
            
            command.ExecuteNonQuery();
        }
    }
}