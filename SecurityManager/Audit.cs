﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SecurityManager
{
	public class Audit : IDisposable
	{
		
		private static EventLog customLog = null;
		const string SourceName = "Nikola.Audit";
		const string LogName = "XXX";

		static Audit()
		{
			try
			{
				if(!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }
                customLog = new EventLog(LogName, Environment.MachineName, SourceName);
			}
			catch (Exception e)
			{
				customLog = null;
				Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
			}
		}

		
		public static void AuthenticationSuccess(string userName)
		{
			// string UserAuthenticationSuccess -> read string format from .resx file
			if (customLog != null)
			{
                // string message -> create message based on UserAuthenticationSuccess and params
                // write message in customLog, EventLogEntryType is Information or SuccessAudit 
                customLog.WriteEntry(String.Format(AuditEvents.UserAuthenticationSuccess, userName));
			}
			else
			{
               
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserAuthenticationSuccess));
			}
		}

		public static void AuthorizationSuccess(string userName, string serviceName)
		{
            customLog.WriteEntry(string.Format(AuditEvents.UserAuthorizationSuccess,userName, serviceName));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="serviceName"> should be read from the OperationContext as follows: OperationContext.Current.IncomingMessageHeaders.Action</param>
		/// <param name="reason">permission name</param>
		public static void AuthorizationFailed(string userName, string serviceName, string reason)
		{
            customLog.WriteEntry(string.Format(AuditEvents.UserAuthorizationFailed, userName, serviceName, reason));	
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
