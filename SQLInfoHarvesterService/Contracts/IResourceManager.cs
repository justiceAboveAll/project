using SQLInfoCollectionService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLInfoCollectionService.Contracts
{
    public interface IResourceManager
    {
        string GetInstanceDetailsScript();

        string GetInstanceRolesScript();

        string GetInstanceLoginsScript();

        string GetInstancePermissionsScript();

        string GetDatabasesScript();

        string GetDbRolesScript();

        string GetDbUsersScript();

        string GetDbPermissionsScript();

        void SetPoftfix(string version);
    }
}
