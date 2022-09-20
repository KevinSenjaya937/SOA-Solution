using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Authenticator_Project
{
    internal class Server
    {
        static void Main(string[] args)
        {
            //int clearInterval;
            //bool done = false;

            //Task task = new Task();

            //This should *definitely* be more descriptive.
            Console.WriteLine("Authenticator Server!");
            //This is the actual host service system
            ServiceHost host;
            //This represents a tcp/ip binding in the Windows network stack
            NetTcpBinding tcp = new NetTcpBinding();
            //Bind server to the implementation of DataServer
            host = new ServiceHost(typeof(Authenticator_Server));
            //Present the publicly accessible interface to the client. 0.0.0.0 tells .net to
            //accept on any interface. :8100 means this will use port 8100. DataService is a name for the
            //actual service, this can be any string.

            host.AddServiceEndpoint(typeof(IAuthenticator_Server), tcp, "net.tcp://0.0.0.0:8100/AuthenticationService");
            //And open the host for business!
            host.Open();
            Console.WriteLine("System Online");
            Console.ReadLine();
            //Console.Write("Please Enter the token clear interval: ");
            //clearInterval = Int16.Parse(Console.ReadLine()); //TODO Validate input

            //while (!done) 
            //{

            //}

            host.Close();
        }
    }
}
