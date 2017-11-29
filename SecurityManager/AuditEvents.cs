using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace SecurityManager
{
	public enum AuditEventTypes
	{
        DOS_attack_report=0,
        UserAuthenticationSuccess = 1,
		UserAuthorizationSuccess = 2,
		UserAuthorizationFailed = 3,
        UserAuthenticationFailed = 4,

    }

	public class AuditEvents
	{
		private static ResourceManager resourceManager = null;
		private static object resourceLock = new object();

		private static ResourceManager ResourceMgr
		{
			get
			{
				lock (resourceLock)
				{
					if (resourceManager == null)
					{
						resourceManager = new ResourceManager(typeof(AuditEventsFile).FullName, Assembly.GetExecutingAssembly());
					}
					return resourceManager;
				}
			}
		}
        /// <summary>
        /// ode u resursni fajl i vraca user{0} je autentifikovan
        /// </summary>
        public static string UserAuthenticationFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.UserAuthenticationFailed.ToString());
            }

        }
        public static string DOS_attack_report
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.DOS_attack_report.ToString());
            }
        }
        public static string UserAuthenticationSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.UserAuthenticationSuccess.ToString());
			}
		}

		public static string UserAuthorizationSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.UserAuthorizationSuccess.ToString());
			}
		}

		public static string UserAuthorizationFailed
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.UserAuthorizationFailed.ToString());
			}
		}
	}
}
