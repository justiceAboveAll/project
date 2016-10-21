using DALLib.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALLib.Models;
using DALLib.Repos;
using DALLib.EF;

namespace DALLib
{
    public class UnitOfWork : IDisposable
    {
        private MsSqlMonitorEntities context;
        private AssignRepository _assignRepository;
        private DatabaseRepository _databaseRepository;
        private DbPermissionRepository _dbPermissionRepository;
        private InstanceRepository _instanceRepository;
        private InstPermissionRepository _instPermissionRepository;
        private UserRepository _userRepository;

        public UnitOfWork(MsSqlMonitorEntities context)
        {
            this.context = context;
        }

        public AssignRepository Assigns
        {
            get
            {
                if (_assignRepository == null)
                    _assignRepository = new AssignRepository(context);
                return _assignRepository;
            }
        }

        public DatabaseRepository Databases
        {
            get
            {
                if (_databaseRepository == null)
                    _databaseRepository = new DatabaseRepository(context);
                return _databaseRepository;
            }
        }

        public DbPermissionRepository DbPermissions
        {
            get
            {
                if (_dbPermissionRepository == null)
                    _dbPermissionRepository = new DbPermissionRepository(context);
                return _dbPermissionRepository;
            }
        }

        public InstanceRepository Instances
        {
            get
            {
                if (_instanceRepository == null)
                    _instanceRepository = new InstanceRepository(context);
                return _instanceRepository;
            }
        }

        public InstPermissionRepository InstPermissions
        {
            get
            {
                if (_instPermissionRepository == null)
                    _instPermissionRepository = new InstPermissionRepository(context);
                return _instPermissionRepository;
            }
        }

        public UserRepository Users
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(context);
                return _userRepository;
            }
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
