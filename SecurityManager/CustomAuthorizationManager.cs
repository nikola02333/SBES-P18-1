using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Security.Principal;
using System.Threading;

namespace SecurityManager
{
	public class CustomAuthorizationManager : ServiceAuthorizationManager
	{
		protected override bool CheckAccessCore(OperationContext operationContext)
		{
           /* bool authorized = false;

			IPrincipal principal = operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as IPrincipal;

			if (principal != null)
			{
				authorized = (principal as CustomPrincipal).IsInRole(Permissions.View.ToString());

				if (authorized == false)
				{


                    /// audit authorization failed event	
                //    Audit.AuthorizationFailed(principal.Identity.Name, Environment.MachineName, operationContext.IncomingMessageHeaders.Action);
				}
				else
				{
                    /// Audit.AuthorizationSuccess();
                    /// audit successfull authorization event
				}
			}

			return authorized;
            */
            return true;

		}
	}
}
