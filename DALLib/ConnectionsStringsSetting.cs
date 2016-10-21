using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using System.Collections;

//Used to store connection strings
//Where key is local machine name
//and value is connection string to base


/*
 Usage example: 

                if (ConnectionsStringsSetting.Default.DataHashtable == null)
            {
                Console.WriteLine("Initializing Hashtable...");

                ConnectionsStringsSetting.Default.DataHashtable = new Hashtable();

                ConnectionsStringsSetting.Default.DataHashtable.Add("YURA-PC", @"Data Source=YURA-PC\SQLEXPRESS;Initial Catalog=db1;Integrated Security=True");
               

                ConnectionsStringsSetting.Default.Save();
            }

            foreach (DictionaryEntry entry in ConnectionsStringsSetting.Default.DataHashtable)
            {
                Console.WriteLine(entry.Key + ": " + entry.Value);
            }


 */

namespace DALLib
{
    class ConnectionsStringsSetting : ApplicationSettingsBase
    {
        private static ConnectionsStringsSetting defaultInstance = (
            (ConnectionsStringsSetting)
            (ApplicationSettingsBase.Synchronized(new ConnectionsStringsSetting())));

        public static ConnectionsStringsSetting Default
        {
            get { return defaultInstance; }
        }


        [UserScopedSettingAttribute()]
        [DebuggerNonUserCodeAttribute()]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        public Hashtable DataHashtable
        {
            get { return ((Hashtable)(this["DataHashtable"])); }
            set { this["DataHashtable"] = value; }
        }
    
    }
}



/*
This part must be in App.config file

          <section
          name="DALLib.ConnectionsStringsSetting"
          type="System.Configuration.ClientSettingsSection, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
          allowExeDefinition="MachineToLocalUser"
          requirePermission="false" /> 
 */
