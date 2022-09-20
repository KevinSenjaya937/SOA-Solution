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
        [Route("get/{token}")]
        [Route("get")]
        public IEnumerable<Service> Get(int token)
        {
            Result authResult = authValidator.Validate(token);
            if(authResult.Status == Result.ResultCodes.Success)
            {
                return ServiceToFile.GetAllServices();
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        // GET: api/Services/5 - BY ID
        public IEnumerable<Service> Get(string id)
        {
            return ServiceToFile.SearchFile(id);
        }

        // POST: api/Services
        [HttpPost]
        public Result PublishService([FromBody]Service value)
        {
            return ServiceToFile.ToFile(value);
        }

        // PUT: api/Services/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Services/5
        public void Delete(string endPoint)
        {
            ServiceToFile.RemoveFromFile(endPoint);
        }
    }
}
