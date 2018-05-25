using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetRego.Models;
using System.Net;
using PetRego.Api;

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

    }
}
