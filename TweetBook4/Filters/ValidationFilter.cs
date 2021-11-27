using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Contracts.v1.Responses;

namespace TweetBook4.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // before controller
            if (!context.ModelState.IsValid)
            {
                // get all Error in model state 
                var errorInModelState = context.ModelState
                       .Where(x => x.Value.Errors.Count > 0)
                       .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage)).ToArray();// return dictionary with error field and error name

                var errorRespone = new ErrorResponse();
                foreach (var error in errorInModelState)
                {
                    foreach (var subError in error.Value)
                    {
                        var errorModel = new ErrorModel
                        {
                            FieldName = error.Key,
                            Message = subError
                        };
                        errorRespone.Errors.Add(errorModel);
                    }
                }
                context.Result = new BadRequestObjectResult(errorRespone);
                return;
            }
            await next();
        }
    }
}
