using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using SOA_SolutionDLL;
using System.IO;
using System.Threading;
using System.Timers;

namespace Authenticator_Project
{
    internal class Server
    {
        private static int clearInterval;
        static void Main(string[] args)
        {
            ServiceHost host;
            NetTcpBinding tcp = new NetTcpBinding();

            Console.WriteLine("Authenticator Server!");
            host = new ServiceHost(typeof(Authenticator_Server));
            host.AddServiceEndpoint(typeof(IAuthenticator_Server), tcp, "net.tcp://0.0.0.0:8100/AuthenticationService");
            host.Open();
            Console.WriteLine("System Online");

            do
            {
                Console.Write("Please Enter the token clear interval (seconds): ");
            }
            while (!Int32.TryParse(Console.ReadLine(), out clearInterval));
            clearTokens();

            Console.ReadLine();

            host.Close();
        }

        private static async void clearTokens() 
        {
            Task task = new Task(startTimer);
            task.Start();
            await task;
            Console.WriteLine("Timer Started");
        }

        private static void startTimer() 
        {
            int timer = 0;
            while (true)
            {
                Thread.Sleep(1000);
                timer++;
                if (timer >= clearInterval)
                {
                    Authenticator_Server.ClearTokens();
                    timer = 0;
                    Console.WriteLine("Tokens Cleared");
                }
            }
        }
    }
}
