using System;
using System.Collections.Generic;
using System.Linq;
using SQLInfoCollectionService.Entities;
using System.Data;
using SQLInfoCollectionService.Contracts;
using SQLInfoCollectionService.Scheduler;
using System.Data.SqlClient;

namespace SQLInfoCollectionService.Collectors
{
    public class InstanceDataCollector : IInstanceDataCollector, IDisposable
    {
        private SqlConnection connection;
        private SqlCommand command;
        private ILogger logger;
        private IResourceManager resourceManager;

        public InstanceDataCollector(string connectionString, IResourceManager resourceManager, ILogger logger)
        {
            connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            command = new SqlCommand(); // Маленький нюанс. Команді призначається не той конекшн.
            this.logger = logger;
            this.resourceManager = resourceManager;
            this.resourceManager.SetPoftfix(connection.ServerVersion);
        }

        // Андрій, вибач за втручання, але я думаю ти і сам планував так зробити.
        // Цей метод використовується в класі UpdateInstanceInfoJob. Там відкривається і закривається конекшн.
        public InstanceDataCollector(SqlConnection conn, IResourceManager resourceManager, ILogger logger)
        {
            this.connection = conn;
            command = new SqlCommand();
            command.Connection = connection;
            this.logger = logger;
            this.resourceManager = resourceManager;
            this.resourceManager.SetPoftfix(connection.ServerVersion);
        }

        public DataTable GetInstanceInfo()
        {
            logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetInstanceInfo");
            string script = resourceManager.GetInstanceDetailsScript();
            this.command.CommandText = script;

            return FillTable();
        }

        public DataTable GetInstanceRoles()
        {
            logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetInstanceRoles");
            string script = resourceManager.GetInstanceRolesScript();
            this.command.CommandText = script;

            return FillTable();
        }

        public DataTable GetInstanceLogins()
        {
            logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetInstanceLogins");
            string script = resourceManager.GetInstanceLoginsScript();
            this.command.CommandText = script;

            return FillTable();
        }

        public DataTable GetInstancePermissions()
        {
            logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetInstancePermissions");
            string script = resourceManager.GetInstancePermissionsScript();
            this.command.CommandText = script;

            return FillTable();
        }

        public DataTable GetDatabases()
        {
            logger.Debug("at SQLInfoCollectionService.Collectors.GetDatabases.CollectDatabases");
            string script = resourceManager.GetDatabasesScript();
            this.command.CommandText = script;

            return FillTable();
        }

        public DataTable GetDatabaseRoles()
        {
            logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetDatabaseRoles");
            string script = resourceManager.GetDbRolesScript();
            this.command.CommandText = script;

            return FillTable();
        }

        public DataTable GetDatabaseUsers()
        {
            logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetDatabaseUsers");
            string script = resourceManager.GetDbUsersScript();
            this.command.CommandText = script;

            return FillTable();
        }

        public DataTable GetDatabasePermissions()
        {
            logger.Debug("at SQLInfoCollectionService.Collectors.DatabaseCollector.GetDatabasePermissions");
            string script = resourceManager.GetDbPermissionsScript();
            this.command.CommandText = script;

            return FillTable();
        }

        public void OpenSqlConnection()
        {
            connection.Open();
        }

        private DataTable FillTable()
        {
            using (IDataReader reader = command.ExecuteReader())
            {
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        public void Dispose()
        {
            this.connection.Close();
            this.command.Dispose();
        }
    }
}
