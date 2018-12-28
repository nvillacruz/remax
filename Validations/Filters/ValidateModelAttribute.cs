using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Remax.Web.Server
{
    /// <summary>
    /// Action filter to check the model state before the controller action is invoked
    /// Dont forget to add [ValidateModel] above the Controller 
    /// </summary>
    public class ValidateModelAttribute: ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
                actionContext.Result = new BadRequestObjectResult(actionContext.ModelState);

        }
    }
}
