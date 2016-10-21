using DALLib.EF;
using DALLib.Models;
using SQLInfoCollectionService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Threading;
using SQLInfoCollectionService.Contracts;

namespace MsSqlMonitorUpdater
{
    class LocalDb : ILocalStorage
    {
        public void SaveInstances(InstanceInfo[] newInfo)
        {
            Console.WriteLine("Data saving started at: {0}", DateTime.Now.ToString("HH:mm:ss:fff"));
            using (MsSqlMonitorEntities context = new MsSqlMonitorEntities())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                // Link InstanceInfo to instance, information for which it contains.
                var linkedInstanceInfo = LinkInstancesByAssociatedId(newInfo, context);
                // Update instance information
                SetNewInstanceInfo(linkedInstanceInfo);
                // Clear instance before adding new information.
                ClearInstances(linkedInstanceInfo, context);
                // Add to instances logins and roles.
                AddPrincipalsToInstance(linkedInstanceInfo);
                // Add to instances databases with their users and roles.
                AddDatabasesAndTheirPrincipals(linkedInstanceInfo);
                // Set permissions for instance principals.
                SetInstancePrincipalsPermissions(linkedInstanceInfo, context);
                // Set permissions for database principals.
                SetDatabasePrinciaplsPermissions(linkedInstanceInfo, context);
                // Map principals (users to roles)
                MapPrincipals(linkedInstanceInfo);

                context.ChangeTracker.DetectChanges();
                context.SaveChanges();
                Console.WriteLine("Data saved at: {0}", DateTime.Now.ToString("HH:mm:ss:fff"));
            }
        }

        #region Configuring permissions for database principals

        private void SetDatabasePrinciaplsPermissions(List<Tuple<Instance, InstanceInfo>> linkedInfo, MsSqlMonitorEntities context)
        {
            linkedInfo.ForEach(li => // In every instance
            {
                foreach (DatabaseInfo dbInfo in li.Item2.Databases) // in every database in the instance
                {
                    foreach (PermissionInfo permInfo in li.Item2.Permissions)
                    {
                        // Permission should be unique, so we take one from database if it exists otherwise create new.
                        DbPermission permission = GetDbPermission(permInfo.Name, permInfo.State, context);
                        int toFind = permInfo.AssociatedIds.Count; // count of associated with permission principals
                        foreach (DbRoleInfo role in dbInfo.Roles)
                        {
                            if (permInfo.AssociatedIds.Contains(role.NativeId))
                            {
                                role.Entity.Permissions.Add(permission);
                                toFind--;
                            }
                            if (toFind == 0)
                                break;
                        }
                        if (toFind == 0)
                            continue;
                        foreach (DbUserInfo user in dbInfo.Users)
                        {
                            if (permInfo.AssociatedIds.Contains(user.NativeId))
                            {
                                user.Entity.Permissions.Add(permission);
                                toFind--;
                            }
                            if (toFind == 0)
                                break;
                        }
                    }
                }
            });
        }

        private DbPermission GetDbPermission(string name, string state, MsSqlMonitorEntities context)
        {
            DbPermission permission = context.DbPermissions.FirstOrDefault(p => p.Name == name && p.State == state);
            if(permission == null)
                permission = context.DbPermissions.Local.FirstOrDefault(p => p.Name == name && p.State == state);
            if (permission == null)
            {
                permission = new DbPermission() { Name = name, State = state };
                context.DbPermissions.Add(permission);
            }
            return permission;
        }

        #endregion

        #region Configuring permissions for instance principals

        private void SetInstancePrincipalsPermissions(List<Tuple<Instance, InstanceInfo>> linkedInfo, MsSqlMonitorEntities context)
        {
            linkedInfo.ForEach(li => // In every instance
            {
                foreach(PermissionInfo permInfo in li.Item2.Permissions) // find every permission
                {
                    // Permission should be unique, so we take one from database if it exists otherwise create new.
                    InstPermission permission = GetInstPermission(permInfo.Name, permInfo.State, context);
                    int toFind = permInfo.AssociatedIds.Count;
                    foreach(InstRoleInfo role in li.Item2.Roles) // Assign permission to role
                    {
                        if(permInfo.AssociatedIds.Contains(role.NativeId))
                        {
                            role.Entity.Permissions.Add(permission);
                            toFind--;
                        }
                        if (toFind == 0)
                            break;
                    }
                    if (toFind == 0)
                        continue;
                    foreach (InstLoginInfo login in li.Item2.Logins) // or to login
                    {
                        if (permInfo.AssociatedIds.Contains(login.NativeId))
                        {
                            login.Entity.Permissions.Add(permission);
                            toFind--;
                        }
                        if (toFind == 0)
                            break;
                    }
                }
            });
        }

        private InstPermission GetInstPermission(string name, string state, MsSqlMonitorEntities context)
        {
            InstPermission permission = context.InstPermissions.FirstOrDefault(perm => perm.Name == name && perm.State == state);
            if(permission == null)
                permission = context.InstPermissions.Local.FirstOrDefault(perm => perm.Name == name && perm.State == state);
            if (permission == null)
            {
                permission = new InstPermission() { Name = name, State = state };
                context.InstPermissions.Add(permission);
            }
            return permission;
        }

        #endregion

        private void ClearInstances(List<Tuple<Instance, InstanceInfo>> linkedInfo, MsSqlMonitorEntities context)
        {
            // Delete all roles before update.
            linkedInfo.ForEach(li => li.Item1.Roles.ToList().ForEach(r => { r.RefuseAllLogins(); context.Entry(r).State = EntityState.Deleted; }));
            // Delete all users before update.
            linkedInfo.ForEach(li => li.Item1.Logins.ToList().ForEach(l => context.Entry(l).State = EntityState.Deleted));
            // Delete all roles from databases
            linkedInfo.ForEach(li => li.Item1.Databases.ToList().ForEach(db => db.Roles.ToList().ForEach(role => { role.RefuseAllUsers(); context.Entry(role).State = EntityState.Deleted; })));
            linkedInfo.ForEach(li => li.Item1.Databases.ToList().ForEach(db => db.Users.ToList().ForEach(user => { user.RefuseAllRoles(); context.Entry(user).State = EntityState.Deleted; })));
            // Delete all databases
            linkedInfo.ForEach(li => li.Item1.Databases.ToList().ForEach(db => context.Entry(db).State = EntityState.Deleted));
        }

        private void AddPrincipalsToInstance(List<Tuple<Instance, InstanceInfo>> linkedInfo)
        {
            linkedInfo.ForEach(li =>
            {
                li.Item2.Roles.ForEach(r => li.Item1.Roles.Add(r.Entity));
                li.Item2.Logins.ForEach(l => li.Item1.Logins.Add(l.Entity));
            });
        }

        private void AddDatabasesAndTheirPrincipals(List<Tuple<Instance, InstanceInfo>> linkedInfo)
        {
            Parallel.ForEach(linkedInfo, li =>
            {
                li.Item2.Databases.ForEach(d => li.Item1.Databases.Add(d.Entity));
            });
        }

        private void SetNewInstanceInfo(List<Tuple<Instance, InstanceInfo>> linkedInfo)
        {
            foreach(var linkedInstance in linkedInfo)
            {
                Instance instance = linkedInstance.Item1;
                InstanceInfo instInfo = linkedInstance.Item2;
                instance.CpuCount = instInfo.CpuCount;
                instInfo.Memory = instInfo.Memory;
                instInfo.OsVersion = instInfo.OsVersion;
                if (instance.Version == null)
                    instance.Version = new InstanceVersion();
                instance.Version.Major = instInfo.Major;
                instance.Version.Minor = instInfo.Minor;
                instance.Version.Build = instInfo.Build;
                instance.Version.Revision = instInfo.Revision;
            }
        }

        #region Mapping

        private void MapPrincipals(List<Tuple<Instance, InstanceInfo>> linkedInfo)
        {
            linkedInfo.ForEach(li => MapInstancePrincipals(li.Item2));
            linkedInfo.ForEach(li => MapDatabasesPrincipals(li.Item2));
        }

        private void MapDatabasesPrincipals(InstanceInfo instInfo)
        {
            foreach(DatabaseInfo dbInfo in instInfo.Databases)
            {
                foreach(DbRoleInfo  role in dbInfo.Roles)
                {
                    int toFind = role.AssociatedIds.Count;
                    foreach(DbUserInfo user in dbInfo.Users)
                    {
                        if (toFind == 0)
                            break;
                        if(role.AssociatedIds.Contains(user.NativeId))
                        {
                            user.Entity.Roles.Add(role.Entity);
                            toFind--;
                        }
                    }
                }
            }
        }

        private void MapInstancePrincipals(InstanceInfo instInfo)
        {
            foreach(InstRoleInfo role in instInfo.Roles)
            {
                int toFind = role.AssociatedIds.Count;
                foreach (InstLoginInfo login in instInfo.Logins)
                {
                    if (toFind == 0)
                        break;
                    if(role.AssociatedIds.Contains(login.NativeId))
                    {
                        login.Entity.Roles.Add(role.Entity);
                        toFind--;
                    }
                }
            }
        }

        #endregion

        private List<Tuple<Instance, InstanceInfo>> LinkInstancesByAssociatedId(InstanceInfo[] infos, MsSqlMonitorEntities context)
        {
            var result = new List<Tuple<Instance, InstanceInfo>>(infos.Length);
            int toFind = infos.Length;
            foreach(InstanceInfo info in infos)
            {
                foreach(Instance inst in context.Instances)
                {
                    if(inst.Id == info.NativeId)
                    {
                        result.Add(new Tuple<Instance, InstanceInfo>(inst, info));
                        toFind--;
                        break;
                    }
                }
                if (toFind == 0)
                    break;
            }
            return result;
        }
    }
}
