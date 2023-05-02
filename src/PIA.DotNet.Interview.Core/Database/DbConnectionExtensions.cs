
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace PIA.DotNet.Interview.Core.Database
{
    /// <summary>
    /// 
    /// </summary>
    public static class DbConnectionExtensions
    {
        public static async Task<int> ExecuteNonQuery(this DbConnection connection, string commandText, IList<SQLiteParameter> parameters = null, int timeout = 30)
        {
            var command = connection.CreateCommand();
            command.CommandTimeout = timeout;
            command.CommandText = commandText;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    command.Parameters.Add(p);
                }
            }
            return await command.ExecuteNonQueryAsync();
        }

        public static async Task<T>  ExecuteScalar<T>(this DbConnection connection, string commandText, IList<SQLiteParameter> parameters = null, int timeout = 30) =>
            (T)connection.ExecuteScalar(commandText, parameters, timeout);

        private static object ExecuteScalar(this DbConnection connection, string commandText, IList<SQLiteParameter> parameters, int timeout)
        {
            var command = connection.CreateCommand();
            command.CommandTimeout = timeout;
            command.CommandText = commandText;
            return command.ExecuteScalarAsync();
        }

        public static async Task<DbDataReader>  ExecuteReader(this DbConnection connection, string commandText, IList<SQLiteParameter> parameters = null)
        {
            var command = connection.CreateCommand();
            command.CommandText = commandText;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    command.Parameters.Add(p);
                }
            }
            return await command.ExecuteReaderAsync();
        }
    }
}
