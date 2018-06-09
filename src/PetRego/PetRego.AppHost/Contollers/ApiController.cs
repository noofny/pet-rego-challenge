using Microsoft.AspNetCore.Mvc;
using PetRego.Common;
using PetRego.Models;
using System.Net;
using AutoMapper;

namespace PetRego.AppHost
{
    /// <summary>
    /// todo - provide a constructor that takes a HttpContext/ControllerContext so these controllers can be unit tested.
    /// </summary>
    public abstract class ApiController : Controller
    {
        protected readonly IAppConfig AppConfig;

        protected ApiController(IAppConfig appConfig)
        {
            AppConfig = appConfig;
        }


        protected void SetResponseCode(Result result)
        {
            var statusCode = Mapper.Map<HttpStatusCode>(result);
            ControllerContext.HttpContext.Response.StatusCode = (int)statusCode;
        }

        protected void ReplaceUrlTokens(IResponse response)
        {
            var baseUrl = GetBaseUrl();
            var currentUrl = GetCurrentUrl();
            var controllerPath = GetControllerPath();
            var actionPath = GetActionPath();

            foreach(var link in response.Metadata.Links)
            {
                link.Href = link.Href.Replace(Constants.TOKENIZED_CURRENT_URL, currentUrl);
                link.Href = link.Href.Replace(Constants.TOKENIZED_BASE_URL, baseUrl);
                link.Href = link.Href.Replace(Constants.TOKENIZED_CONTROLLER_PATH, controllerPath);
                link.Href = link.Href.Replace(Constants.TOKENIZED_ACTION_PATH, actionPath);
            }
        }

        string GetBaseUrl()
        {
            var request = ControllerContext.HttpContext.Request;

            var scheme = request.Scheme;
            var host = request.Host.HasValue ? request.Host.Value : string.Empty;
            return $"{scheme}://{host}";
        }

        string GetCurrentUrl()
        {
            var baseUrl = GetBaseUrl();
            var request = ControllerContext.HttpContext.Request;

            var path = request.Path.HasValue ? request.Path.Value.TrimEnd('/') : string.Empty;
            return $"{baseUrl}{path}";
        }

        string GetControllerPath()
        {
            var actionDescriptor = ControllerContext.ActionDescriptor;

            return actionDescriptor.ControllerName.ToLower();
        }

        string GetActionPath()
        {
            var actionDescriptor = ControllerContext.ActionDescriptor;

            return actionDescriptor.ActionName.ToLower();
        }


    }
}
