using System.Collections.Generic;
using SQLInfoCollectionService.Entities;
using SQLInfoCollectionService.Scheduler;
using System.Data;

namespace SQLInfoCollectionService.Contracts
{
    public interface IInstanceDataCollector
    {

        /// <summary>
        /// TODO: We need to decide what exactly it should return.
        /// Now it should have columns
        /// </summary>
        DataTable GetInstanceInfo();

        /// <summary>
        /// Returns roles from the instance.
        /// Columns sequence: principal_id, name, type_desc, princpal_id of assigned users
        /// </summary>
        DataTable GetInstanceRoles();

        /// <summary>
        /// Returns all logins on the instance
        /// Columns sequence: database_id, principal_id, name, type_desc, is_disabled (as numeric value 1 or 0)
        /// </summary>
        DataTable GetInstanceLogins();

        /// <summary>
        /// Returns all permissions from the instance.
        /// Columns sequence: permission_name, state_desc, grantee_principal_id
        /// </summary>
        DataTable GetInstancePermissions();

        /// <summary>
        /// Returns all databases from the instance.
        /// Columns sequence: database_id, name, Size in MB.
        /// </summary>
        DataTable GetDatabases();

        /// <summary>
        /// Returns all roles from all databases of the instance.
        /// Columns sequence: database_id, principal_id, name, type_desc, princpal_id of assigned users
        /// </summary>
        DataTable GetDatabaseRoles();

        /// <summary>
        /// Returns all users from all databases of the instance.
        /// Columns sequence: database_id, principal_id, name, type_desc
        /// </summary>
        DataTable GetDatabaseUsers();

        /// <summary>
        /// Returns all permissions from all databases of the instance.
        /// Columns sequence: database_id, permission_name, state_desc, grantee_principal_id
        /// </summary>
        DataTable GetDatabasePermissions();
    }
}
