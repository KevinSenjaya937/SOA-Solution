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
        private static string servicePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/ServiceData.txt").ToString();

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

        public static IEnumerable<Service> SearchFile(string inS) 
        {
            //Search logic to return services descriptions matching the inS
            string[] servVals;
            List<Service> services = new List<Service>();

            string[] lines = File.ReadAllLines(servicePath);
            foreach (string line in lines)
            {
                servVals = line.Split(',');
                if (servVals[1].Contains(inS))
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
            return services;
        }


        public static IEnumerable<Service> GetAllServices()
        {
            string[] servVals;
            List<Service> services = new List<Service>();

            string[] lines = File.ReadAllLines(servicePath);

            foreach (string line in lines)
            {
                servVals=line.Split(',');
                services.Add(new Service()
                {
                    Name = servVals[0],
                    Description = servVals[1],
                    APIEndPoint = servVals[2],
                    NumOfOperands = Int16.Parse(servVals[3]),
                    OperandType = servVals[4]
                });
            }
            return services;
        }

        public static Result RemoveFromFile(string endPoint)
        {
            IEnumerable<Service> services = GetAllServices();
            List<Service> validServices = new List<Service>();

            foreach (Service service in services)
            {
                if (service.APIEndPoint != endPoint)
                {
                    validServices.Add(service);
                }
            }

            using(FileStream fileStream = File.Open(servicePath,FileMode.OpenOrCreate,FileAccess.ReadWrite))
            {
                lock (fileStream)
                {
                    fileStream.SetLength(0);
                }
            }

            foreach (Service service in validServices)
            {
                ToFile(service);
            }

            return new Result() { Status = Result.ResultCodes.Success };
        }
    }
}