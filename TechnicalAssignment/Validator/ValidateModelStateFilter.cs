using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility.Response;

namespace TechnicalAssignment.Validator
{
    public class ValidateModelStateFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var fluentErrors = new BadRequestObjectResult(context.ModelState).Value;
                context.Result = new BadRequestObjectResult(GetError(fluentErrors));
            }

            base.OnResultExecuting(context);
        }
        private static ErrorResponse GetError(object errResult)
        {
            var errorModel = new ErrorResponse
            {
                Errors = errResult
            };
            return errorModel;
        }
    }
}
