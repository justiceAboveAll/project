using DALLib.Models;
using Model = DALLib.Models;

namespace DALLib.EF
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Configuration;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    public class MsSqlMonitorEntities : DbContext
    {
        public MsSqlMonitorEntities()
            : base(GetConnectionString())
        {
            
        }


        //returns connection string for local computer
        //to add new connection string see class  ConnectionsStringsSetting
        public static String GetConnectionString()
        {
            log4net.Config.XmlConfigurator.Configure();

            ConnectionsStringsSetting.Default.DataHashtable = new Hashtable();
            ConnectionsStringsSetting.Default.Save();


            if (ConnectionsStringsSetting.Default.DataHashtable == null)
            {
                ConnectionsStringsSetting.Default.DataHashtable = new Hashtable();                
                ConnectionsStringsSetting.Default.Save();
            }

            String connString = (String)ConnectionsStringsSetting.Default.DataHashtable[Environment.MachineName];

            if (connString == null)
            {

                //Temporary
                ConnectionsStringsSetting.Default.DataHashtable.Add("YURA-PC", @"Data Source=YURA-PC\SQLEXPRESS;Initial Catalog=db1;Integrated Security=True;MultipleActiveResultSets=True;");
                ConnectionsStringsSetting.Default.Save();

                connString = (String)ConnectionsStringsSetting.Default.DataHashtable[Environment.MachineName];
            }

            return connString;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InstLogin>()
                .HasMany(il => il.Roles)
                .WithMany(ir => ir.Logins)
                .Map(lr => lr.ToTable("InstRoleMembers").MapLeftKey("LoginId").MapRightKey("RoleId"));

            modelBuilder.Entity<DbUser>()
                .HasMany(du => du.Roles)
                .WithMany(dr => dr.Users)
                .Map(ur => ur.ToTable("DbRoleMembers").MapLeftKey("UserId").MapRightKey("RoleId"));

            modelBuilder.Entity<Credential>()
                .HasRequired(c => c.User)
                .WithMany(u => u.Credentials)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<InstPrincipal>()
                .HasMany(ip => ip.Permissions)
                .WithMany(ip => ip.Principals)
                .Map(pp => pp.ToTable("InstPrincipalPermission").MapLeftKey("PrincipalId").MapRightKey("PermissionId"));

            modelBuilder.Entity<DbPrincipal>()
                .HasMany(dp => dp.Permissions)
                .WithMany(dp => dp.Principal)
                .Map(pp => pp.ToTable("DbPrincipalPermission").MapLeftKey("PrincipalId").MapRightKey("PermissionId"));
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Instance> Instances { get; set; }
        public virtual DbSet<InstanceVersion> InstanceVersions { get; set; }
        public virtual DbSet<Assign> Assigns { get; set; }
        public virtual DbSet<Model.Database> Databases { get; set; }
        public virtual DbSet<InstPrincipal> InstPrincipals { get; set; }
        public virtual DbSet<DbPrincipal> DbPrincipals { get; set; }
        public virtual DbSet<DbPermission> DbPermissions { get; set; }
        public virtual DbSet<InstPermission> InstPermissions { get; set; }

        public virtual DbSet<InstanceUpdateJob> InstanceUpdateJobs { get; set; }
        public virtual DbSet<JobType> JobTypes { get; set; }
    }
}