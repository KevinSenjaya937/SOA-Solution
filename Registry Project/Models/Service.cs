using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Registry_Project.Models
{
    public class Service
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string APIEndPoint { get; set; }
        public int NumOfOperands { get; set; }
        public string OperandType { get; set; }
    }
}