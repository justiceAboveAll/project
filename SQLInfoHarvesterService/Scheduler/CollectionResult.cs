using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using SQLInfoCollectionService.InstanceInfoUpdating.Entities;
using DALLib.Models;

namespace SQLInfoCollectionService.Scheduler
{
    public class CollectionResult
    {

        public DALLib.Models.Instance Instance { get; set; }
        public JobType.UpdateInfoType UpdateInfoType { get; set; }

       public  SQLInfoCollectorService.Scheduler.Scheduler Scheduler { get; set; }

        public InstanceInfo InstanceInfo { get; set; }


        //public InstanceVersion InstanceVersion { get; set; }
        //public InstanceDetails InstanceDetails { get; set; }
        //public ICollection<InstancePrincipal> InstancePrincipals { get; set; }
        //public ICollection<InstancePermission> InstancePermissions { get; set; }
        //public ICollection<InstanceRoleMember> InstanceRoleMembers { get; set; }
        //public ICollection<DatabaseDataPack> DatabaseDataPacks { get; set; }





    }



}
