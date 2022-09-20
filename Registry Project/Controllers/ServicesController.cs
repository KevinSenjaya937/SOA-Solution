using Registry_Project.Models;
using SOA_SolutionDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Registry_Project.Controllers
{
    [RoutePrefix("api")]
    public class ServicesController : ApiController
    {
        AuthValidator authValidator = new AuthValidator();

        // GET: api/Services - Returns all
        [Route("Services/{token}")]
        [Route("Services")]
        [HttpGet]
        public ServiceResult Get(int token)
        {
            ServiceResult serviceResult = new ServiceResult();
            Result authResult = authValidator.Validate(token);

            if(authResult.Status == Result.ResultCodes.Success)
            {
                serviceResult.Services = ServiceToFile.GetAllServices();
            }
            else
            {
                serviceResult.Status = Result.ResultCodes.Denied;
                serviceResult.Reason = authResult.Reason;
            }

            return serviceResult;
        }

        // GET: api/Services/5 - BY ID
        [Route("Services/{token}/{id}")]
        [Route("Services")]
        [HttpGet]
        public ServiceResult SearchByDescription(int token, string id)
        {
            ServiceResult serviceResult = new ServiceResult();
            Result authResult = authValidator.Validate(token);

            if (authResult.Status == Result.ResultCodes.Success)
            {
                serviceResult.Services = ServiceToFile.SearchFile(id);
            }
            else
            {
                serviceResult.Status = Result.ResultCodes.Denied;
                serviceResult.Reason = authResult.Reason;
            }

            return serviceResult;
        }

        // POST: api/Services
        [Route("Services/{token}/{value}")]
        [Route("Services")]
        [HttpPost]
        public ServiceResult PublishService(int token, [FromBody]Service value)
        {
            ServiceResult serviceResult = new ServiceResult();
            Result authResult = authValidator.Validate(token);

            if (authResult.Status == Result.ResultCodes.Success)
            {
                serviceResult.Status = ServiceToFile.ToFile(value).Status;
            }
            else
            {
                serviceResult.Status = Result.ResultCodes.Denied;
                serviceResult.Reason = authResult.Reason;
            }

            return serviceResult;
        }

        // DELETE: api/Services/5
        [Route("Services/{token}/{endPoint}")]
        [Route("Services")]
        [HttpPost]
        [HttpDelete]
        public ServiceResult Delete(int token, string endPoint)
        {
            ServiceResult serviceResult = new ServiceResult();
            Result authResult = authValidator.Validate(token);

            if (authResult.Status == Result.ResultCodes.Success)
            {
                serviceResult.Status = ServiceToFile.RemoveFromFile(endPoint).Status;
            }
            else
            {
                serviceResult.Status = Result.ResultCodes.Denied;
                serviceResult.Reason = authResult.Reason;
            }

            return serviceResult;
        }
    }
}
