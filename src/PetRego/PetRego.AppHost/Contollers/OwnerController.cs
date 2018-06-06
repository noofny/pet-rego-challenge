using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetRego.Common;
using PetRego.Models;
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


        [HttpGet("{emailAddress}")]
        [Route("api/[controller]/search/{emailAddress}")]
        public async Task<IResponse> Search(string emailAddress)
        {
            var response = await OwnerService.Search(emailAddress);
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }

        [HttpGet("{id}")]
        [Route("api/[controller]")]
        public async Task<IResponse> Summary(string id)
        {
            var response = await OwnerService.Summary(id);
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }

        [HttpGet("{id}")]
        [Route("api/[controller]")]
        public async Task<IResponse> Detail(string id)
        {
            var response = await OwnerService.Detail(id);
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }

        [HttpPost]
        [Route("api/[controller]")]
        public async Task<IResponse> Create([FromBody]OwnerDetailModel owner)
        {
            var response = await OwnerService.Create(owner);
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }

        [HttpPut("{id}")]
        [Route("api/[controller]")]
        public async Task<IResponse> Edit(string id, [FromBody]OwnerDetailModel owner)
        {
            owner.Id = id;
            var response = await OwnerService.Update(owner);
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
