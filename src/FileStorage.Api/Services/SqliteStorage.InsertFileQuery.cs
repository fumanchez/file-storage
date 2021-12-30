using Microsoft.Data.Sqlite;

namespace FileStorage.Api.Services;

public partial class SqliteStorage
{
    private class InsertFileQuery
    {
        private readonly SqliteConnection _connection;

        public InsertFileQuery(SqliteConnection connection)
        {
            _connection = connection;
        }

        public async Task<long> ExecuteWith(string name, byte[] content, byte[] json)
        {
            var id = InsertZeroFile(name, content.Length, json);
            using var blob = new SqliteBlob(_connection, "files", "content", id);
            await blob.WriteAsync(content);
            return id;
        }

        private long InsertZeroFile(string name, int length, byte[] json)
        {
            const string commandText =
                "insert into files (name, content, json) " +
                "values (@name, zeroblob(@length), @json);" +
                "select last_insert_rowid();";

            using var command = _connection.CreateCommand();
            command.CommandText = commandText;
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@length", length);
            command.Parameters.AddWithValue("@json", json);

            var rowId = command.ExecuteScalar();
            return (long)rowId;
        }

        private object? SelectLastInsertRowId()
        {
            const string commandText = @"select last_insert_rowid()";

            using var command = _connection.CreateCommand();
            command.CommandText = commandText;

            var rowId = command.ExecuteScalar();
            return rowId;
        }
    }
}