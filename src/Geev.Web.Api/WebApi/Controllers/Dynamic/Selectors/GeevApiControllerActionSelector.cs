using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Controllers;
using Geev.WebApi.Configuration;
using Geev.WebApi.Controllers.Dynamic.Builders;

namespace Geev.WebApi.Controllers.Dynamic.Selectors
{
    /// <summary>
    /// This class overrides ApiControllerActionSelector to select actions of dynamic ApiControllers.
    /// </summary>
    public class GeevApiControllerActionSelector : ApiControllerActionSelector
    {
        private readonly IGeevWebApiConfiguration _configuration;

        public GeevApiControllerActionSelector(IGeevWebApiConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// This class is called by Web API system to select action method from given controller.
        /// </summary>
        /// <param name="controllerContext">Controller context</param>
        /// <returns>Action to be used</returns>
        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            object controllerInfoObj;
            if (!controllerContext.ControllerDescriptor.Properties.TryGetValue("__GeevDynamicApiControllerInfo", out controllerInfoObj))
            {
                return GetDefaultActionDescriptor(controllerContext);
            }

            //Get controller information which is selected by GeevHttpControllerSelector.
            var controllerInfo = controllerInfoObj as DynamicApiControllerInfo;
            if (controllerInfo == null)
            {
                throw new GeevException("__GeevDynamicApiControllerInfo in ControllerDescriptor.Properties is not a " + typeof(DynamicApiControllerInfo).FullName + " class.");
            }

            //No action name case
            var hasActionName = (bool)controllerContext.ControllerDescriptor.Properties["__GeevDynamicApiHasActionName"];
            if (!hasActionName)
            {
                return GetActionDescriptorByCurrentHttpVerb(controllerContext, controllerInfo);
            }

            //Get action name from route
            var serviceNameWithAction = (controllerContext.RouteData.Values["serviceNameWithAction"] as string);
            if (serviceNameWithAction == null)
            {
                return GetDefaultActionDescriptor(controllerContext);
            }

            var actionName = DynamicApiServiceNameHelper.GetActionNameInServiceNameWithAction(serviceNameWithAction);

            return GetActionDescriptorByActionName(
                controllerContext,
                controllerInfo,
                actionName
                );
        }

        private HttpActionDescriptor GetActionDescriptorByCurrentHttpVerb(HttpControllerContext controllerContext, DynamicApiControllerInfo controllerInfo)
        {
            //Check if there is only one action with the current http verb
            var actionsByVerb = controllerInfo.Actions.Values
                .Where(action => action.Verb == controllerContext.Request.Method.ToHttpVerb())
                .ToArray();

            if (actionsByVerb.Length == 0)
            {
                throw new HttpException(
                    (int)HttpStatusCode.NotFound,
                    "There is no action" +
                    " defined for api controller " + controllerInfo.ServiceName +
                    " with an http verb: " + controllerContext.Request.Method
                );
            }

            if (actionsByVerb.Length > 1)
            {
                throw new HttpException(
                    (int)HttpStatusCode.InternalServerError,
                    "There are more than one action" +
                    " defined for api controller " + controllerInfo.ServiceName +
                    " with an http verb: " + controllerContext.Request.Method
                );
            }

            //Return the single action by the current http verb
            return new DynamicHttpActionDescriptor(_configuration, controllerContext.ControllerDescriptor, actionsByVerb[0]);
        }

        private HttpActionDescriptor GetActionDescriptorByActionName(HttpControllerContext controllerContext, DynamicApiControllerInfo controllerInfo, string actionName)
        {
            //Get action information by action name
            DynamicApiActionInfo actionInfo;
            if (!controllerInfo.Actions.TryGetValue(actionName, out actionInfo))
            {
                throw new GeevException("There is no action " + actionName + " defined for api controller " + controllerInfo.ServiceName);
            }

            if (actionInfo.Verb != controllerContext.Request.Method.ToHttpVerb())
            {
                throw new HttpException(
                    (int) HttpStatusCode.BadRequest,
                    "There is an action " + actionName +
                    " defined for api controller " + controllerInfo.ServiceName +
                    " but with a different HTTP Verb. Request verb is " + controllerContext.Request.Method +
                    ". It should be " + actionInfo.Verb
                );
            }

            return new DynamicHttpActionDescriptor(_configuration, controllerContext.ControllerDescriptor, actionInfo);
        }

        private HttpActionDescriptor GetDefaultActionDescriptor(HttpControllerContext controllerContext)
        {
            return base.SelectAction(controllerContext);
        }
    }
}