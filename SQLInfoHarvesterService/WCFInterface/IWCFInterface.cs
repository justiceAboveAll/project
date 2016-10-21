using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;



namespace SQLInfoCollectionService
{
    [ServiceContract]
    public interface IWCFInterface
    {
        [OperationContract]
        void RefereshInstanceInfo(int instanceId);
    }
}
