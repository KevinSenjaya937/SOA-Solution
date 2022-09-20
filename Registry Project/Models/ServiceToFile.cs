using SOA_SolutionDLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Registry_Project.Models
{
    public static class ServiceToFile
    {
        private static string root = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName; //Directory containing this solution
        private static string serviceFile = "Services.txt";
        private static string servicePath = Path.Combine(root, serviceFile);
        public static Result ToFile(Service inS)
        {
            Result result = new Result();

            using (StreamWriter sw = File.AppendText(servicePath))
            {
                sw.WriteLine(
                    inS.Name + "," +
                    inS.Description + "," +
                    inS.APIEndPoint + "," +
                    inS.NumOfOperands + "," +
                    inS.OperandType
                    );
            }
            result.Status = Result.ResultCodes.Success;
            return result;
        }

        public static IEnumerable<Service> SearchFile(Service inS) 
        {
            //Search logic to return services descriptions matching the inS
            string[] servVals;
            List<Service> services = new List<Service>();

            string[] lines = File.ReadAllLines(servicePath);
            foreach (string line in lines)
            {
                servVals = line.Split(' ');
                if (true) 
                {
                    services.Add(new Service() { 
                        Name = servVals[0],
                        Description = servVals[1],
                        APIEndPoint = servVals[2],
                        NumOfOperands = Int16.Parse(servVals[3]),
                        OperandType = servVals[4]
                    });
                }
            }
            return new List<Service>();
        }
    }
}