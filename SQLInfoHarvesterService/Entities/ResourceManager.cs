using SQLInfoCollectionService.Contracts;
using SQLInfoCollectionService.Properties;
using System;

namespace SQLInfoCollectionService.Entities
{
    public class ResourceManager : IResourceManager
    {
        private string postFix;

        public ResourceManager(string version)
        {
            SetPoftfix(version);
        }

        public string GetDatabasesScript()
        {
            string resourceName = "databases";

            return Resources.ResourceManager.GetString(GenerateResourceFullName(resourceName));
        }

        public string GetDbPermissionsScript()
        {
            string resourceName = "dbPermissions";

            return Resources.ResourceManager.GetString(GenerateResourceFullName(resourceName));
        }

        public string GetDbUsersScript()
        {
            string resourceName = "dbUsers";

            return Resources.ResourceManager.GetString(GenerateResourceFullName(resourceName));
        }

        public string GetDbRolesScript()
        {
            string resourceName = "dbRoles";

            return Resources.ResourceManager.GetString(GenerateResourceFullName(resourceName));
        }

        public string GetInstanceDetailsScript()
        {
            string resourceName = "instanceDetails";

            return Resources.ResourceManager.GetString(GenerateResourceFullName(resourceName));
        }

        public string GetInstancePermissionsScript()
        {
            string resourceName = "instancePermissions";

            return Resources.ResourceManager.GetString(GenerateResourceFullName(resourceName));
        }

        public string GetInstanceLoginsScript()
        {
            string resourceName = "instanceLogins";

            return Resources.ResourceManager.GetString(GenerateResourceFullName(resourceName));
        }

        public string GetInstanceRolesScript()
        {
            string resourceName = "instanceRoles";

            return Resources.ResourceManager.GetString(GenerateResourceFullName(resourceName));
        }

        public void SetPoftfix(string version)
        {
            string major = version.Split('.')[0];

            switch (major)
            {
                case "9":
                    throw new NotImplementedException();//Not implemented 2005 version scripts
                case "10":
                case "11":
                case "12":
                    postFix =  "2008";
                    break;
                default:
                    postFix = null;//Throwing WrongVersionException here
                    break;
            }
        }

        private string GenerateResourceFullName(string resourceName)
        {
            return String.Concat(resourceName, postFix);
        }
    }
}
