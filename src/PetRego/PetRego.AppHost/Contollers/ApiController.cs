using Microsoft.AspNetCore.Mvc;
using PetRego.Common;
using PetRego.Models;
using System.Net;

namespace PetRego.AppHost
{
    /// <summary>
    /// todo - find a nice wayto mock the HttpContext (on ControllerContext) so these controllers can be unit tested.
    /// </summary>
    public class ApiController : Controller
    {
        protected const string ApiBasePath = "api/";
        protected const string ApiControllerPath = "[controller]/";
        readonly IAppConfig AppConfig;

        public ApiController(IAppConfig appConfig)
        {
            AppConfig = appConfig;
        }


        protected void SetResponseCode(Result result)
        {
            var response = ControllerContext.HttpContext.Response;
            switch (result)
            {
                case Result.BadRequest:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case Result.InternalError:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;

                default:
                    // For POST/PUT/DELETE methods, it is common to respond with 204(NoContent).
                    // Because I am returning a response with at least metadata/hypermedia however, 
                    // I am choosing to return 200(OK) instead.
                    response.StatusCode = (int)HttpStatusCode.OK;
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
            var path = request.Path.HasValue ? request.Path.Value.TrimEnd('/') : string.Empty;
            return $"{scheme}://{host}{path}";
        }


    }
}
