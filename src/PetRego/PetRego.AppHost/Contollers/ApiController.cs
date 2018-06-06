using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetRego.Common;
using PetRego.Models;
using System.Web;
using System.Net;
using System.Net.Http;

namespace PetRego.AppHost
{
    /// <summary>
    /// todo - find a nice wayto mock the HttpContext (ControllerContext) so these controllers can be unit tested.
    /// </summary>
    public class ApiController : Controller
    {
        readonly IAppConfig AppConfig;

        public ApiController(IAppConfig appConfig)
        {
            AppConfig = appConfig;
        }

        protected void SetResponseCode(Result result)
        {
            switch (result)
            {
                case Result.BadRequest:
                    ControllerContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case Result.InternalError:
                    ControllerContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;

                default:
                    // For POST/PUT/DELETE methods, it is common to respond with 204(NoContent).
                    // Because I am returning a response with at least metadata/hypermedia however, 
                    // I am choosing to return 200(OK) instead.
                    ControllerContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    break;

            }
        }

        protected void ReplaceUrlTokens(IResponse response)
        {
            var baseUrl = GetBaseUrl();

            foreach(var link in response.Metadata.Links)
            {
                link.Href = link.Href.Replace(AppConfig.TokenizedBaseUrl, baseUrl);
            }
        }

        string GetBaseUrl()
        {
            var request = ControllerContext.HttpContext.Request;

            var scheme = request.Scheme;
            var host = request.Host.HasValue ? request.Host.Value : string.Empty;
            var path = Request.Path.HasValue ? Request.Path.Value.TrimEnd('/') : string.Empty;
            return $"{scheme}://{host}{path}";
        }


    }
}
