using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Security.Principal;

namespace SecurityManager
{
	public class CustomAuthorizationManager : ServiceAuthorizationManager
	{
		protected override bool CheckAccessCore(OperationContext operationContext)
		{
             bool authorized = false;

			IPrincipal principal = operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as IPrincipal;

			if (principal != null)
			{
				authorized = (principal as CustomPrincipal).IsInRole(Permissions.execute.ToString());

				if (authorized == false)
				{
					// false/// audit authorization failed event					
				}
				else
				{
                   // return true;	/// audit successfull authorization event
				}
			}

			return authorized;
            //return true;
		}
	}
}
