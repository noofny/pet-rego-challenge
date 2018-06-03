using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetRego.Models;
using System.Net;
using PetRego.Api;
using System.Net.Http;

namespace PetRego.AppHost
{
    [Route("api/[controller]")]
    public class OwnerController : Controller
    {
        readonly OwnerService OwnerService;

        public OwnerController(OwnerService ownerService)
        {
            OwnerService = ownerService;
        }

        [HttpGet]
        public async Task<MultiResponse> Get()
        {
            var response = await OwnerService.GetAll();
            response.Links.Add(Link.Self($"{GetBaseUrl()}", Request.Method));
            response.Links.Add(Link.Edit($"{GetBaseUrl()}/{{id}}", HttpMethod.Put.Method));
            response.Links.Add(Link.Delete($"{GetBaseUrl()}/{{id}}", HttpMethod.Delete.Method));
            response.Links.Add(Link.Custom("get", $"{GetBaseUrl()}/{{id}}", HttpMethod.Get.Method));
            response.Links.Add(Link.Custom("create", $"{GetBaseUrl()}", HttpMethod.Post.Method));

            if (response.Failed)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.OK;
            }
            return response;
        }

        [HttpGet("{id}")]
        public async Task<SingleResponse> Get(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new SingleResponse($"The request was missing a valid parameter : id", Result.Error);
            }

            var response = await OwnerService.GetById(id);
            response.Links.Add(Link.Self($"{GetBaseUrl()}/{id}", Request.Method));
            response.Links.Add(Link.Edit($"{GetBaseUrl()}/{id}", HttpMethod.Put.Method));
            response.Links.Add(Link.Delete($"{GetBaseUrl()}/{id}", HttpMethod.Delete.Method));
            response.Links.Add(Link.Custom("get", $"{GetBaseUrl()}", HttpMethod.Get.Method));
            response.Links.Add(Link.Custom("create", $"{GetBaseUrl()}", HttpMethod.Post.Method));

            if (response.Failed)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.OK;
            }
            return response;
        }

        [HttpPost]
        public async Task<BasicResponse> Post([FromBody]OwnerModel owner)
        {
            // todo - validate given request object and return 400(BadRequest) if it fails.  

            var response = await OwnerService.Create(owner);
            response.Links.Add(Link.Self($"{GetBaseUrl()}", Request.Method));
            response.Links.Add(Link.Edit($"{GetBaseUrl()}/{{id}}", HttpMethod.Put.Method));
            response.Links.Add(Link.Delete($"{GetBaseUrl()}/{{id}}", HttpMethod.Delete.Method));
            response.Links.Add(Link.Custom("get", $"{GetBaseUrl()}", HttpMethod.Get.Method));
            response.Links.Add(Link.Custom("get", $"{GetBaseUrl()}/{{id}}", HttpMethod.Get.Method));

            if (response.Failed)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Created;
            }
            return response;
        }

        [HttpPut("{id}")]
        public async Task<BasicResponse> Put(string id, [FromBody]OwnerModel owner)
        {
            // todo - validate given request object and return 400(BadRequest) if it fails.  

            if (string.IsNullOrEmpty(id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new BasicResponse($"The request was missing a valid parameter : id", Result.Error);
            }

            owner.Id = id;
            var response = await OwnerService.Update(owner);
            response.Links.Add(Link.Self($"{GetBaseUrl()}/{id}", Request.Method));
            response.Links.Add(Link.Delete($"{GetBaseUrl()}/{id}", HttpMethod.Delete.Method));
            response.Links.Add(Link.Custom("create", $"{GetBaseUrl()}", HttpMethod.Post.Method));
            response.Links.Add(Link.Custom("get", $"{GetBaseUrl()}", HttpMethod.Get.Method));
            response.Links.Add(Link.Custom("get", $"{GetBaseUrl()}/{id}", HttpMethod.Get.Method));

            if (response.Failed)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            else
            {
                // I could return 204(NoContent) here but I want the user to know the result (updated).
                Response.StatusCode = (int)HttpStatusCode.OK;
            }
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<BasicResponse> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new BasicResponse($"The request was missing a valid parameter : id", Result.Error);
            }

            var response = await OwnerService.Delete(id);
            response.Links.Add(Link.Self($"{GetBaseUrl()}/{id}", Request.Method));
            response.Links.Add(Link.Edit($"{GetBaseUrl()}/{id}", HttpMethod.Put.Method));
            response.Links.Add(Link.Custom("create", $"{GetBaseUrl()}", HttpMethod.Post.Method));
            response.Links.Add(Link.Custom("get", $"{GetBaseUrl()}", HttpMethod.Get.Method));
            response.Links.Add(Link.Custom("get", $"{GetBaseUrl()}/{id}", HttpMethod.Get.Method));

            if (response.Failed)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            else
            {
                // I could return 204(NoContent) here but I want the user to know the result (updated).
                Response.StatusCode = (int)HttpStatusCode.OK;
            }
            return response;
        }

        string GetBaseUrl()
        {
            return $"{Request.Scheme}://{Request.Host}{Request.Path}";
        }


    }
}
