using Microsoft.Extensions.Logging;
using stuartalexanderltd.dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.Logging
{
    public class ProfiledDbCommand : DbCommand
    {
        private readonly ILogger<ProfiledDbCommand> _logger;
        private readonly DbCommand _dbCommand;

        public ProfiledDbCommand(ILogger<ProfiledDbCommand> logger, DbCommand dbCommand)
        {
            _logger = logger;
            _dbCommand = dbCommand;
        }

        public override string CommandText { get => _dbCommand.CommandText; set => _dbCommand.CommandText = value; }
        public override int CommandTimeout { get => _dbCommand.CommandTimeout; set => _dbCommand.CommandTimeout = value; }
        public override CommandType CommandType { get => _dbCommand.CommandType; set => _dbCommand.CommandType = value; }
        public override bool DesignTimeVisible { get => false; set => throw new NotImplementedException(); }
        public override UpdateRowSource UpdatedRowSource { get => _dbCommand.UpdatedRowSource; set => _dbCommand.UpdatedRowSource = value; }
        protected override DbConnection DbConnection { get => _dbCommand.Connection; set => _dbCommand.Connection = value; }

        protected override DbParameterCollection DbParameterCollection => _dbCommand.Parameters;

        protected override DbTransaction DbTransaction { get => _dbCommand.Transaction; set => _dbCommand.Transaction = value; }

        public override void Cancel()
        {
            ProfiledDbExtensions.Action(_logger, () => { _dbCommand.Cancel(); }, "DB operation cancelled");
        }

        public override int ExecuteNonQuery()
        {
            return ProfiledDbExtensions.Action(_logger, () => { return _dbCommand.ExecuteNonQuery(); }, "Execute non-query");
        }

        public override Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
        {
            return ProfiledDbExtensions.Action(_logger, () => { return _dbCommand.ExecuteNonQueryAsync(cancellationToken); }, "Execute non-query async");
        }

        public override object ExecuteScalar()
        {
            return ProfiledDbExtensions.Action(_logger, () => { return _dbCommand.ExecuteScalar(); }, "Execute scalar");
        }

        public override Task<object> ExecuteScalarAsync(CancellationToken cancellationToken)
        {
            return ProfiledDbExtensions.Action(_logger, () => { return _dbCommand.ExecuteScalarAsync(); }, "Execute scalar async");
        }

        public override void Prepare()
        {
            ProfiledDbExtensions.Action(_logger, () => { _dbCommand.Prepare(); }, "Prepare");
        }

        protected override DbParameter CreateDbParameter()
        {
            throw new NotImplementedException();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return ProfiledDbExtensions.Action(_logger, () => { return _dbCommand.ExecuteReader(behavior); }, "Execute Data Reader");
        }

        protected override Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
        {
            return ProfiledDbExtensions.Action(_logger, () => { return _dbCommand.ExecuteReaderAsync(behavior, cancellationToken); }, "Execute Data Reader async");
        }


    }
}
