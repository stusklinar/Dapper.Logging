
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
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
            ProfiledDbExtensions.Action(_logger, () => { _dbConnection.ChangeDatabase(databaseName); }, "Change Database Name");
        }

        public override void Open()
        {
            ProfiledDbExtensions.Action(_logger, () => { _dbConnection.Open(); }, "Connection Opened");
        }

        public override Task OpenAsync(CancellationToken cancellationToken)
        {
            return ProfiledDbExtensions.Action(_logger, () => { return _dbConnection.OpenAsync(cancellationToken); }, "Connection Opened Async");
        }

        protected override DbCommand CreateDbCommand()
        {
            return ProfiledDbExtensions.Action(_logger, () => { return _dbConnection.CreateCommand(); }, "Create DB command");
        }

        public override void EnlistTransaction(Transaction transaction)
        {
            ProfiledDbExtensions.Action(_logger, () => { _dbConnection.EnlistTransaction(transaction); }, "Enlisting in transaction");
        }

        public override void Close()
        {
            ProfiledDbExtensions.Action(_logger, () => { _dbConnection.Close(); }, "Connection Closed");
        }

        protected override DbTransaction BeginDbTransaction(System.Data.IsolationLevel isolationLevel)
        {
            return ProfiledDbExtensions.Action(_logger, () => { return _dbConnection.BeginTransaction(); }, "Begin DB Transaction");
        }
    }
}