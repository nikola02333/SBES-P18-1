using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SecurityManager
{
    public enum Permissions
    {
       execute=0,
       modify=1,
       addentity=2
    }
    public class RolesConfig
	{
        static string[] ClientPermissions = new string[] { };
        static string[] AdministratorPermissions = new string[] { };
        static string[] OperatorPermissions = new string[] { };

        static string[] Empty = new string[] { };
        public static void ReadXMLConfiguration()
        {
            string execute_value = String.Empty;
            string modify_value= String.Empty;
            string addentity_value= String.Empty;
        XmlDocument doc = new XmlDocument();
            doc.Load("RoleConfig.xml");
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/Configuration/Client");

                execute_value= nodes[0].SelectSingleNode("execute").InnerText;
            modify_value = nodes[0].SelectSingleNode("modify").InnerText;
                addentity_value = nodes[0].SelectSingleNode("addentity").InnerText;
            if(execute_value== "true")
            {
                ClientPermissions = new string[] { Permissions.execute.ToString() };
            }
            if (modify_value == "true")
            {
                ClientPermissions = new string[] { Permissions.execute.ToString(), Permissions.modify.ToString() };
            }
            if(addentity_value=="true")
            {
                ClientPermissions = new string[] { Permissions.execute.ToString(), Permissions.modify.ToString(),Permissions.addentity.ToString() };
            }

                nodes = doc.DocumentElement.SelectNodes("/Configuration/Operator");
                execute_value = nodes[0].SelectSingleNode("execute").InnerText;
            modify_value = nodes[0].SelectSingleNode("modify").InnerText;
                addentity_value = nodes[0].SelectSingleNode("addentity").InnerText;

            if (execute_value == "true")
            {
                OperatorPermissions = new string[] { Permissions.execute.ToString() };
            }
            if (modify_value == "true")
            {
                OperatorPermissions = new string[] { Permissions.execute.ToString(), Permissions.modify.ToString() };
            }
            if (addentity_value == "true")
            {
                OperatorPermissions = new string[] { Permissions.execute.ToString(), Permissions.modify.ToString(), Permissions.addentity.ToString() };
            }

            nodes = doc.DocumentElement.SelectNodes("/Configuration/Administrator");
            execute_value = nodes[0].SelectSingleNode("execute").InnerText;
            modify_value = nodes[0].SelectSingleNode("modify").InnerText;
            addentity_value = nodes[0].SelectSingleNode("addentity").InnerText;
            if (execute_value == "true")
            {
                AdministratorPermissions = new string[] { Permissions.execute.ToString() };
            }
            if (modify_value == "true")
            {
                AdministratorPermissions = new string[] { Permissions.execute.ToString(), Permissions.modify.ToString() };
            }
            if (addentity_value == "true")
            {
                AdministratorPermissions = new string[] { Permissions.execute.ToString(), Permissions.modify.ToString(), Permissions.addentity.ToString() };
            }
        }
        public RolesConfig()
        {
            ReadXMLConfiguration();
        }
		public static string[] GetPermissions(string role)
		{
			switch (role)
			{
                case "Client" : return ClientPermissions;
                case "ooo": return OperatorPermissions;
				case "ppp": return AdministratorPermissions;
				default: return Empty;
			}
		}
	}
}
