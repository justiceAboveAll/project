using Microsoft.Practices.Unity;
using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using SQLInfoCollectionService.Contracts;
using SQLInfoCollectionService;
using System.Data.SqlClient;
using DALLib.Impersonation;

namespace SQLInfoCollectionService.InstanceInfoUpdating
{
    using Entities;
    using System.Resources;

    public class InstanceInfoUpdater
    {
        private IUnityContainer _unityContainer;
        ILogger _logger;

        public InstanceInfoUpdater(IUnityContainer container, ILogger logger)
        {
            _unityContainer = container;
            this._logger = logger;
        }

        public InstanceInfo UpdateStatusOnly(Instance instance)
        {
            using (SqlConnection connection = new SqlConnection(BuildConnectionString(instance)))
            {
                InstanceInfo result = new InstanceInfo(instance.Id);
                try
                {
                    connection.Open();
                    IInstanceDataCollector collector = (IInstanceDataCollector)_unityContainer.Resolve(
                        typeof(IInstanceDataCollector),
                        new ParameterOverride("conn", connection));
                    result = ParseInstanceInfo(new InstanceInfo(instance.Id), collector.GetInstanceInfo().Rows[0]);
                }
                catch
                {
                    result.Status = InstanceStatus.Offline;
                }
                return result;
            }
        }

        public InstanceInfo Update(Instance instance)
        {
            using (SqlConnection connection = new SqlConnection(BuildConnectionString(instance)))
            {
                InstanceInfo result = new InstanceInfo(instance.Id);

                //impersonation
                Impersonation impersonation = null;
                ImpersonationResult impResult = null;

                if (instance.Authentication == AuthenticationType.Windows)
                {
                    impersonation = new Impersonation();
                    impResult = impersonation.Impersonate(instance.Login, null, instance.Password);

                    if (impResult.HasError) _logger.Error("Instance id =" + instance.Id + " Imperosnation error:" + impResult.ErrorString);
                    else _logger.Debug("Impersonation oK instance id ="+ instance.Id);

                  
                }


                 try
                {
                    connection.Open();
                }
                catch
                {
                    result.Status = InstanceStatus.Offline;
                    return result;
                }
                finally
                {
                    if (impResult !=null && impersonation!=null)
                        if (!impResult.HasError) impersonation.UndoImpersonation(impResult.User);
                }


                //!tmp changed beg
         

                IResourceManager resourceManger = (IResourceManager)_unityContainer.Resolve(typeof(IResourceManager),
                                                                                              new ParameterOverride("version", connection.ServerVersion));
                IInstanceDataCollector collector = (IInstanceDataCollector)_unityContainer.Resolve(typeof(IInstanceDataCollector),
                    new ParameterOverride("conn", connection), new ParameterOverride("resourceManager", resourceManger), new ParameterOverride("logger", _logger)); 
                //!tmp changed end

                 // Join parsed instance and databases.
                 var joinInstanceInfo = new JoinBlock<InstanceInfo, List<DatabaseInfo>>();
                // Get information about databases and their principals (inluding permissions).
                // Stars first because it has more data and more time to process it.
                DataTable databases = collector.GetDatabases(); // List of all databases on the instance with some basic information (size, date of creation).
                DataTable databaseRoles = collector.GetDatabaseRoles(); // List of all roles on all databases.
                DataTable databaseUsers = collector.GetDatabaseUsers(); // List of all users on all databases.
                DataTable databasePermissions = collector.GetDatabasePermissions(); // List of all permissions on all databases.
                // Start parsing databases and their principals.
                Task.Run(() => joinInstanceInfo.Target2.Post(ParseDatabasesAndTheirPrincipals(databases, databaseRoles, databaseUsers, databasePermissions)));
                // Get information about instance and it's principals.
                DataRow instanceDetails = collector.GetInstanceInfo().Rows[0];
                DataTable instanceRoles = collector.GetInstanceRoles();
                DataTable instanceLogins = collector.GetInstanceLogins();
                DataTable instancePermissions = collector.GetInstancePermissions();
                // Start parsing of instance principals
                Task.Run(() => joinInstanceInfo.Target1.Post(ParseInstancePrincipals(instance.Id, instanceDetails, instanceRoles, instanceLogins, instancePermissions)));
                var readyData = joinInstanceInfo.Receive();
                readyData.Item1.Databases.AddRange(readyData.Item2);
                return readyData.Item1;


            }
        }

        #region Parsing Instance and it's Principals

        private InstanceInfo ParseInstancePrincipals(int nativeInstanceId, DataRow instanceDetails, DataTable instanceRoles, 
            DataTable instanceLogins, DataTable instancePermissions)
        {
            // Parse instance itself.
            InstanceInfo result = ParseInstanceInfo(new InstanceInfo(nativeInstanceId), instanceDetails);
            result.Roles = ParseInstanceRoles(instanceRoles);
            result.Logins = ParseLogins(instanceLogins);
            result.Permissions = ParseInstancePermissions(instancePermissions);
            return result;
        }

        private InstanceInfo ParseInstanceInfo(InstanceInfo instanceInfo, DataRow instanceDetails)
        {
            instanceInfo.Memory = Convert.ToInt32(instanceDetails[0]);
            instanceInfo.CpuCount = Convert.ToByte(instanceDetails[1]);
            instanceInfo.OsVersion = instanceDetails[2].ToString();
            int[] instanceVersion = instanceDetails[3].ToString().Split('.').Select(num => Convert.ToInt32(num)).ToArray();
            instanceInfo.Major = instanceVersion.Length > 0 ? instanceVersion[0] : 0;
            instanceInfo.Major = instanceVersion.Length >= 1 ? instanceVersion[1] : 0;
            instanceInfo.Major = instanceVersion.Length >= 2 ? instanceVersion[2] : 0;
            instanceInfo.Major = instanceVersion.Length >= 3 ? instanceVersion[3] : 0;
            return instanceInfo;
        }

        private List<InstRoleInfo> ParseInstanceRoles(DataTable roles)
        {
            // Create list of instance roles with list of IDs of associated with them logins.
            return (from row in roles.AsEnumerable()
                    group row by new { Id = Convert.ToInt32(row[0]), Name = row[1].ToString(), Type = row[2].ToString() } into role
                    select new InstRoleInfo()
                    {
                        NativeId = role.Key.Id,
                        Entity = new InstRole() { Name = role.Key.Name, Type = role.Key.Type },
                        AssociatedIds = role.Where(r => !r.IsNull(3)).Select(r => Convert.ToInt32(r[3])).ToList()
                    }).ToList();
        }

        private List<InstLoginInfo> ParseLogins(DataTable logins)
        {
            // Create list of instance logins with their NativeId (as it saved in database).
            return (from row in logins.AsEnumerable()
                    select new InstLoginInfo()
                    {
                        NativeId = Convert.ToInt32(row[0]),
                        Entity = new InstLogin() { Name = row[1].ToString(), Type = row[2].ToString(), IsDisabled = row[3].ToString() == "1" ? true : false }
                    }).ToList();
        }

        private List<PermissionInfo> ParseInstancePermissions(DataTable permissions)
        {
            // Create new permission objects with list of IDs of associated with them principals.
            return (from row in permissions.AsEnumerable()
                    group row by new { Name = row[0].ToString(), State = row[1].ToString() }
                    into perm
                    select new PermissionInfo()
                    {
                        State = perm.Key.State,
                        Name = perm.Key.Name,
                        AssociatedIds = perm.Select(p => Convert.ToInt32(p[2])).ToList()
                    }).ToList();
        }

        #endregion

        #region Parsing Databases and their Principals

        private List<DatabaseInfo> ParseDatabasesAndTheirPrincipals(DataTable databases, DataTable databaseRoles, DataTable databaseUsers, DataTable databasePermissions)
        {
            // Create DatabaseInfo classes that hold information about Database and all it's entities.
            List<DatabaseInfo> parsedDbs = ParseDatabases(databases);
            // All entities from all databases are in one table. So group them by databaseId.
            var groupedRoles = from row in databaseRoles.AsEnumerable() group row by Convert.ToInt32(row[0]) into g select g;
            var groupedUsers = from row in databaseUsers.AsEnumerable() group row by Convert.ToInt32(row[0]) into g select g;
            var groupedPermissions = from row in databasePermissions.AsEnumerable() group row by Convert.ToInt32(row[0]) into g select g;
            parsedDbs.ForEach(db =>
            {
                // Parse permissions
                //db.Permissions.AddRange(ParseDatabasePermissions(groupedPermissions.First(perms => perms.Key == db.NativeId)));
                db.Permissions = ParseDatabasePermissions(groupedPermissions.First(perms => perms.Key == db.NativeId));
                // Parse users
                //ParseUsers(groupedUsers.First(users => users.Key == db.NativeId), db).ForEach(user => db.Entity.Users.Add(user.Entity));
                db.Users = ParseUsers(groupedUsers.First(users => users.Key == db.NativeId), db);
                db.Users.ForEach(user => db.Entity.Users.Add(user.Entity));
                // Parse roles
                //ParseDatabaseRoles(groupedRoles.First(roles => roles.Key == db.NativeId), db).ForEach(role => db.Entity.Roles.Add(role.Entity));
                db.Roles = ParseDatabaseRoles(groupedRoles.First(roles => roles.Key == db.NativeId), db);
                db.Roles.ForEach(role => db.Entity.Roles.Add(role.Entity));
            });
            return parsedDbs;
        }

        private List<DbRoleInfo> ParseDatabaseRoles(IGrouping<int, DataRow> roles, DatabaseInfo database)
        {
            return (from row in roles
                    group row by new { Id = Convert.ToInt32(row[1]), Name = row[2].ToString() } into g
                    select new DbRoleInfo()
                    {
                        NativeId = g.Key.Id,
                        AssociatedIds = g.Where(r => !r.IsNull(4)).Select(r => Convert.ToInt32(r[4])).ToList(),
                        Entity = new DbRole()
                        {
                            Database = database.Entity,
                            Name = g.Key.Name,
                        }
                    }).ToList();
        }

        private List<DbUserInfo> ParseUsers(IGrouping<int, DataRow> users, DatabaseInfo database)
        {
            return (from row in users
                    select new DbUserInfo()
                    {
                        NativeId = Convert.ToInt32(row[1]),
                        Entity = new DbUser()
                        {
                            Database = database.Entity,
                            Name = row[2].ToString(),
                            Type = row[3].ToString()
                        }
                    }).ToList();
        }

        private List<PermissionInfo> ParseDatabasePermissions(IGrouping<int, DataRow> permissions)
        {
            // Create new permission objects with list of IDs of associated with them principals.
            return (from row in permissions
                    group row by new { Name = row[1].ToString(), State = row[2].ToString() }
                    into perm
                    select new PermissionInfo()
                    {
                        State = perm.Key.State,
                        Name = perm.Key.Name,
                        AssociatedIds = perm.Select(p => Convert.ToInt32(p[3])).ToList()
                    }).ToList();
        }

        private List<DatabaseInfo> ParseDatabases(DataTable databases)
        {
            return (from row in databases.AsEnumerable()
                    select new DatabaseInfo()
                    {
                        NativeId = Convert.ToInt32(row[0]),
                        Entity = new Database()
                        {
                            Name = row[1].ToString(),
                            CreateDate = Convert.ToDateTime(row[2]),
                            Size = Convert.ToDecimal(row[3])
                        }
                    }).ToList();
        }

        #endregion

        private string BuildConnectionString(Instance instance)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = instance.DataSource;
            if (instance.Authentication == AuthenticationType.Sql) //!changed  if (instance.Authentication == AuthenticationType.Windows) to AuthenticationType.Sql
            {
                builder.UserID = instance.Login;
                builder.Password = instance.Password;
            }
            else
                builder.IntegratedSecurity = true;
            return builder.ConnectionString;
        }
    }
}
