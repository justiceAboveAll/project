using SQLInfoCollectionService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLInfoCollectionService.InstanceInfoUpdating
{
    using Contracts;
    using Entities;

    public interface ILocalStorage
    {
        void SaveInstances(InstanceInfo[] newInfo,ILogger logger);

        void SaveStatusOnly(InstanceInfo[] newInfo, ILogger logger);
    }
}
