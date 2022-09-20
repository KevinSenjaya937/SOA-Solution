using SOA_SolutionDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceProviderData.Models
{
    public class ControllerMath
    {
        /// <summary>
        /// Adds the two inputs togethor and returns them in a Result Obj
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public Result AddTwo(int num1, int num2) 
        {
            Result result = new Result();
            result.Value = num1 + num2;
            result.Status = Result.ResultCodes.Success;
            return result;
        }

        /// <summary>
        /// Adds the three inputs togethor and returns them in a Result Obj
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <param name="num3"></param>
        /// <returns></returns>
        public Result AddThree(int num1, int num2, int num3)
        {
            Result result = new Result();
            result.Value = num1 + num2 + num3;
            result.Status = Result.ResultCodes.Success;
            return result;
        }

        /// <summary>
        /// Multiplys the two inputs togethor and returns them in a Result Obj
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public Result MultiplyTwo(int num1, int num2)
        {
            Result result = new Result();
            result.Value = num1 * num2;
            result.Status = Result.ResultCodes.Success;
            return result;
        }

        /// <summary>
        /// Multiplys the three inputs togethor and returns them in a Result Obj
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <param name="num3"></param>
        /// <returns></returns>
        public Result MultiplyThree(int num1, int num2, int num3)
        {
            Result result = new Result();
            result.Value = num1 * num2 * num3;
            result.Status = Result.ResultCodes.Success;
            return result;
        }

    }
}