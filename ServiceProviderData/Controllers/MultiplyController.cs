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
    public class MultiplyController : ApiController
    {
        AuthValidator authValidator = new AuthValidator();

        [Route("multiply/{token}/{firstNumber}/{secondNumber}")]
        [Route("multiply")]
        [HttpGet]
        public Result Multiply(int token, int firstNumber, int secondNumber)
        {
            Result result;
            Result authResult = authValidator.Validate(token);
            if (authResult.Status == Result.ResultCodes.Success)
            {
                ControllerMath myMath = new ControllerMath();
                result = myMath.MultiplyTwo(firstNumber, secondNumber);
            }
            else 
            {
                result = authResult;
            }
            return result;
        }

        [Route("multiply/{token}/{firstNumber}/{secondNumber}/{thirdNumber}")]
        [Route("multiply")]
        [HttpGet]
        public Result Multiply(int token, int firstNumber, int secondNumber, int thirdNumber)
        {
            Result result;
            Result authResult = authValidator.Validate(token);
            if (authResult.Status == Result.ResultCodes.Success)
            {
                ControllerMath myMath = new ControllerMath();
                result = myMath.MultiplyThree(firstNumber, secondNumber, thirdNumber);
            }
            else
            {
                result = authResult;
            }
            return result;
        }
    }
}
