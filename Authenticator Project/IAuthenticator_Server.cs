using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Authenticator_Project
{
    [ServiceContract]
    public interface IAuthenticator_Server
    {
        [OperationContract]
        string Register(string userName, string password);

        [OperationContract]
        //[FaultContract(typeof(ArgumentOutOfRangeException))] - Implement later
        int Login(string userName, string password);

        [OperationContract]
        string Validate(int token);
    }
}
