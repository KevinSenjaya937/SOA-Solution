using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SOA_SolutionDLL;

namespace Authenticator_Project
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class Authenticator_Server : IAuthenticator_Server
    {
        private string root = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName; //Directory containing this solution
        private string loginFile = "User_Passwords.txt";
        private string tokenFile = "User_Tokens.txt";

        private static bool makeOnce = false;
        /// <summary>
        /// Clears the exsisting files
        /// </summary>
        public Authenticator_Server() 
        {
            if (!makeOnce) 
            {
                string loginPath = Path.Combine(root, loginFile);
                string tokenPath = Path.Combine(root, tokenFile);

                using (StreamWriter sw = File.CreateText(loginPath)) { }
                using (StreamWriter sw = File.CreateText(tokenPath)) { }

                makeOnce = true;
            }
        }

        /// <summary>
        /// Saves the userName/Password combo to a local File - User_Passwords.txt
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Register(string userName, string password) //TODO add checking logic, i.e same user/pass already exsits???
        {
            string path = Path.Combine(root, loginFile);

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(userName + " " + password);
            }

            return "successfully registered";
        }

        /// <summary>
        /// Check if the combo is in a file - User_Passwords.txt, if so creates and save a random int token into a file
        /// Return the generated token to the user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Returns generated token or -1 if not found</returns>
        public int Login(string userName, string password)
        {
            string loginPath = Path.Combine(root, loginFile);
            string tokenPath = Path.Combine(root, tokenFile);

            bool found = false;
            string[] usrPass;

            int token = -1;

            string[] lines = File.ReadAllLines(loginPath);
            foreach (string line in lines) 
            {
                usrPass = line.Split(' ');
                if (usrPass[0].Equals(userName) && usrPass[1].Equals(password)) 
                {
                    found = true;
                }
            }

            if (found) 
            {
                using (StreamWriter sw = File.AppendText(tokenPath)) //TODO new file isn't created each time
                {
                    Random rand = new Random();
                    token = rand.Next(100, 10000);
                    sw.WriteLine(token);
                }
            }
            return token;
        }

        /// <summary>
        /// Validates if token is valid
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Returns either "validated" OR "not validated"</returns>
        public string Validate(int token)
        {
            string tokenPath = Path.Combine(root, tokenFile);
            string tokenString = token.ToString();
            bool found = false;
            string rtrnStr = "";

            string[] lines = File.ReadAllLines(tokenPath);
            foreach (string line in lines)
            {
                if (line.Equals(tokenString)) 
                {
                    found = true;
                    rtrnStr = "validated";
                }
            }
            if (!found) 
            {
                rtrnStr = "not validated";
            }
            return rtrnStr;
        }

        /// <summary>
        /// Used to clear the token file
        /// </summary>
        private void ClearTokens() 
        {
            string tokenPath = Path.Combine(root, tokenFile);

            using (StreamWriter sw = File.CreateText(tokenPath)) 
            { }
        }
    }
}
