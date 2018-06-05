using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetRego.Common;
using PetRego.Models;
using System.Net;
using PetRego.Api;

namespace PetRego.AppHost
{
    public class OwnerController : ApiController
    {
        readonly OwnerService OwnerService;

        public OwnerController(OwnerService ownerService, IAppConfig appConfig): base(appConfig)
        {
            OwnerService = ownerService;
        }


        [HttpGet]
        [Route("api/[controller]")]
        public async Task<IResponse> GetAll()
        {
            var response = await OwnerService.GetAll();
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }

        [HttpGet("{id}")]
        [Route("api/[controller]")]
        public async Task<IResponse> Get(string id)
        {
            var response = await OwnerService.GetById(id);
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }

        [HttpPut("{id}")]
        [Route("api/[controller]")]
        public async Task<IResponse> Edit(string id, [FromBody]OwnerModel owner)
        {
            owner.Id = id;
            var response = await OwnerService.Update(owner);
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }


        [HttpPost]
        [Route("api/[controller]")]
        public async Task<IResponse> Create([FromBody]OwnerModel owner)
        {
            var response = await OwnerService.Create(owner);
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }

        [HttpDelete("{id}")]
        [Route("api/[controller]")]
        public async Task<IResponse> Delete(string id)
        {
            var response = await OwnerService.Delete(id);
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }


    }
}
