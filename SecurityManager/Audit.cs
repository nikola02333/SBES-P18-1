using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace SecurityManager
{
	public class Audit : IDisposable
	{
		
		private static EventLog customLog = null;
		const string SourceName = "nikola023";
		const string LogName = "LogLb";

		static Audit()
		{
			try
			{
				if(!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }

                customLog = new EventLog(LogName, Environment.MachineName, SourceName);
                /// create customLog handle
			}
			catch (Exception e)
			{
				customLog = null;
				Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
			}
		}
        /// <summary>
        /// da bi se posle ubacivale u log moj fajl
        /// AuthenticationSuccess -> value da se izvuce - preko auditEvents.Usernesto.  usrename zamenjujemo umesto {0}
        /// customlog.
        /// </summary>
        /// <param name="userName"></param>
        /// 

        public static void UserAuthenticationFailed(string username)
        {
            if (customLog != null)
            {
                customLog.WriteEntry(string.Format(AuditEvents.UserAuthorizationFailed, username));
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserAuthenticationSuccess));
            }

        }
        public static void Dos_Attack_Report(string username)
        {
            // string UserAuthenticationSuccess -> read string format from .resx file
            if (customLog != null)
            {

                // string message -> create message based on UserAuthenticationSuccess and params
                // write message in customLog, EventLogEntryType is Information or SuccessAudit 

                customLog.WriteEntry(string.Format(AuditEvents.DOS_attack_report,username));
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserAuthenticationSuccess));
            }
        }

        public static void AuthenticationSuccess(string userName)
		{
			// string UserAuthenticationSuccess -> read string format from .resx file
			if (customLog != null)
			{
                customLog.WriteEntry(String.Format(AuditEvents.UserAuthenticationSuccess, userName));
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserAuthenticationSuccess));
			}
		}

		public static void AuthorizationSuccess(string userName,string accesTo)
		{
            
            customLog.WriteEntry(String.Format(AuditEvents.UserAuthorizationSuccess, userName,accesTo));

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="serviceName"> should be read from the OperationContext as follows: OperationContext.Current.IncomingMessageHeaders.Action</param>
		/// <param name="reason">permission name</param>
		public static void AuthorizationFailed(string userName, string serviceName, string reason)
		{
            customLog.WriteEntry(String.Format(AuditEvents.UserAuthorizationFailed, userName, serviceName, reason));
        }

		public void Dispose()
		{
			if (customLog != null)
			{
				customLog.Dispose();
				customLog = null;
			}
		}
	}
}
