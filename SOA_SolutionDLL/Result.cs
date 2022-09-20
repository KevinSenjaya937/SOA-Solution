using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOA_SolutionDLL
{
    public class Result
    {
        public enum ResultCodes
        {
            Success,
            Denied
        }
        public int Value { get; set; }
        public ResultCodes Status { get; set; }
        public string Reason { get; set; }
    }
}
