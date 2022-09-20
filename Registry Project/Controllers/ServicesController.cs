﻿using Registry_Project.Models;
using SOA_SolutionDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Registry_Project.Controllers
{
    public class ServicesController : ApiController
    {

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
        public void Delete(int id)
        {
        }
    }
}
