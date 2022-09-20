using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SOA_SolutionDLL;
using System.ServiceModel;

namespace Registry_Project.Models
{
    /// <summary>
    /// Sets up a connection to the validator server - Has a static connection between instances
    /// </summary>
    public class AuthValidator
    {
        private static IAuthenticator_Server authServer;

        public AuthValidator()
        {
            if (authServer == null)
            {
                ChannelFactory<IAuthenticator_Server> foobFactory;
                NetTcpBinding tcp = new NetTcpBinding();
                //Set the URL and create the connection!
                string URL = "net.tcp://localhost:8100/AuthenticationService";
                foobFactory = new ChannelFactory<IAuthenticator_Server>(tcp, URL);
                authServer = foobFactory.CreateChannel();
            }
        }

        public Result Validate(int token)
        {
            string validation = authServer.Validate(token);
            Result result = new Result();

            if (validation.Equals("not validated"))
            {
                result.Status = Result.ResultCodes.Denied;
                result.Reason = "Authentication Error";
            }
            else
            {
                result.Status = Result.ResultCodes.Success;
            }
            return result;
        }
    }
}