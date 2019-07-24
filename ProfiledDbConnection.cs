
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace stuartalexanderltd.dapper
{
    public class ProfiledDbConnection : DbConnection
    {
        private readonly DbConnection _dbConnection;
        private readonly ILogger<ProfiledDbConnection> _logger;

        public ProfiledDbConnection(DbConnection dbConnection, ILogger<ProfiledDbConnection> logger)
        {
            _dbConnection = dbConnection;
            _logger = logger;
        }

        protected override bool CanRaiseEvents => base.CanRaiseEvents;
        public override string ConnectionString { get => _dbConnection.ConnectionString; set => _dbConnection.ConnectionString = value; }
        public override int ConnectionTimeout => _dbConnection.ConnectionTimeout;
        public override string Database => _dbConnection.Database;
        public override string DataSource => _dbConnection.DataSource;
        public override string ServerVersion => _dbConnection.ServerVersion;
        public override ISite Site { get => _dbConnection.Site; set => _dbConnection.Site = value; }
        public override ConnectionState State => _dbConnection.State;

        public override void ChangeDatabase(string databaseName)
        {
            Action(() => { _dbConnection.ChangeDatabase(databaseName); }, "Change Database Name");
        }

        public override void Open()
        {
            Action(() => { _dbConnection.Open(); }, "Connection Opened");
        }

        public override Task OpenAsync(CancellationToken cancellationToken)
        {
            return ActionAsync(() => { return _dbConnection.OpenAsync(cancellationToken); }, "Connection Opened Async");
        }

        protected override DbCommand CreateDbCommand()
        {
            return Action(() => { return _dbConnection.CreateCommand(); }, "Create DB command");
        }

        public override void EnlistTransaction(Transaction transaction)
        {
            Action(() => { _dbConnection.EnlistTransaction(transaction); }, "Enlisting in transaction");
        }

        public override void Close()
        {
            Action(() => { _dbConnection.Close(); }, "Connection Closed");
        }

        protected override DbTransaction BeginDbTransaction(System.Data.IsolationLevel isolationLevel)
        {
            return Action(() => { return _dbConnection.BeginTransaction(); }, "Begin DB Transaction");
        }

        private void Action(Action func, string message)
        {
            var sw = new Stopwatch();
            func();
            sw.Stop();
            _logger.LogInformation($"{message} - {sw.ElapsedMilliseconds}ms");
        }
        private T Action<T>(Func<T> func, string message)
        {
            var sw = new Stopwatch();
            var result = func();
            sw.Stop();
            _logger.LogInformation($"{message} - {sw.ElapsedMilliseconds}ms");
            return result;
        }
        private async Task ActionAsync(Func<Task> func, string message)
        {
            var sw = new Stopwatch();
            await func();
            sw.Stop();
            _logger.LogInformation($"{message} - {sw.ElapsedMilliseconds}ms");
        }
    }
}