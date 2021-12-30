using Microsoft.Data.Sqlite;

namespace FileStorage.Api.Services;

public partial class SqliteStorage
{
    private class SelectFileQuery
    {
        private readonly SqliteConnection _connection;

        public SelectFileQuery(SqliteConnection connection)
        {
            _connection = connection;
        }

        public (string name, MemoryStream content, MemoryStream json) ExecuteWith(long id)
        {
            const string commandText =
                "select name, content, json from files " +
                "where id = @id";

            using var command = _connection.CreateCommand();
            command.CommandText = commandText;
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            reader.Read();

            var name = reader.GetString(0);
            var content = reader.GetStream(1) as MemoryStream;
            var json = reader.GetStream(2) as MemoryStream;
            return (name, content, json);
        }
    }
}