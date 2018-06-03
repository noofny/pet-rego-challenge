using Microsoft.AspNetCore.Mvc;
using PetRego.Models;
using System.Net.Http;

namespace PetRego.AppHost
{
    [Route("api")]
    public class RootController : Controller
    {
        [HttpGet]
        public BasicResponse Get()
        {
            var response = new BasicResponse(Result.Success);

            response.Links.Add(Link.Self($"{GetBaseUrl()}", Request.Method));
            response.Links.Add(Link.Custom("owners", $"{GetBaseUrl()}/owner", HttpMethod.Get.Method));
            response.Links.Add(Link.Custom("pets", $"{GetBaseUrl()}/pet", HttpMethod.Get.Method));

            return response;
        }


        string GetBaseUrl()
        {
            return $"{Request.Scheme}://{Request.Host}{Request.Path}";
        }

    }
}
