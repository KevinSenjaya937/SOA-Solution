using ServiceProviderData.Models;
using SOA_SolutionDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ServiceProviderData.Controllers
{
    [RoutePrefix("api")]
    public class AddController : ApiController
    {
        AuthValidator authValidator = new AuthValidator();

        [Route("add/{token}/{firstNumber}/{secondNumber}")]
        [Route("add")]
        [HttpGet]
        public Result Add(int token, int firstNumber, int secondNumber)
        {
            Result result;
            Result authResult = authValidator.Validate(token);
            if (authResult.Status == Result.ResultCodes.Success)
            {
                ControllerMath myMath = new ControllerMath();
                result = myMath.AddTwo(firstNumber, secondNumber);
            }
            else
            {
                result = authResult;
            }
            return result;
        }

        [Route("add/{token}/{firstNumber}/{secondNumber}/{thirdNumber}")]
        [Route("add")]
        [HttpGet]
        public Result Add(int token, int firstNumber, int secondNumber, int thirdNumber)
        {
            Result result;
            Result authResult = authValidator.Validate(token);
            if (authResult.Status == Result.ResultCodes.Success)
            {
                ControllerMath myMath = new ControllerMath();
                result = myMath.AddThree(firstNumber, secondNumber, thirdNumber);
            }
            else
            {
                result = authResult;
            }
            return result;
        }
    }
}
