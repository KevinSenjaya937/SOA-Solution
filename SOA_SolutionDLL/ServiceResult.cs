using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOA_SolutionDLL
{
    public class ServiceResult : Result
    {
        public IEnumerable<Service> Services { get; set; }
    }
}
