using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data;

namespace CommonLib
{
    public class BrowsableInstance
    {
        public string ServerName { get; set; }
        public string InstanceName { get; set; }

        public string Version { get; set; }


        public BrowsableInstance (string server,string instance,string version)
        {
            ServerName = server;
            InstanceName = instance;
            Version = version;
        }

        //returns list of instances in network if "SQL Server Browser" service is runned
        public static List<BrowsableInstance> GetInstances()
        {
            List<BrowsableInstance> list = new List<BrowsableInstance>();

            SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
            DataTable table = instance.GetDataSources();

            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn dataColumn in table.Columns)
                {
                    Console.WriteLine("{0} = {1}", dataColumn.ColumnName, row[dataColumn]);
                }
                Console.WriteLine();
                list.Add(new BrowsableInstance(row["ServerName"].ToString(), row["InstanceName"].ToString(), row["Version"].ToString()));
            }

            return list;
        }

    }
}
