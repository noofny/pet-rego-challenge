﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetRego.Common;
using PetRego.Models;
using PetRego.Api;

namespace PetRego.AppHost
{
    public class PetController : ApiController
    {
        readonly PetService PetService;

        public PetController(PetService petService, IAppConfig appConfig): base(appConfig)
        {
            PetService = petService;
        }


        [HttpGet]
        [Route(Constants.API_ROUTE_BASE_PATH + Constants.API_ROUTE_CONTROLLER_PATH + "search/{field}/{value}")]
        public async Task<IResponse> Search(string field, string value)
        {
            var response = await PetService.Search(field, value);
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }

        [HttpGet]
        [Route(Constants.API_ROUTE_BASE_PATH + Constants.API_ROUTE_CONTROLLER_PATH + "{id}/detail")]
        public async Task<IResponse> Detail(string id)
        {
            var response = await PetService.Detail(id);
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }

        [HttpPost]
        [Route(Constants.API_ROUTE_BASE_PATH + Constants.API_ROUTE_CONTROLLER_PATH)]
        public async Task<IResponse> Create([FromBody]PetModel pet)
        {
            var response = await PetService.Create(pet);
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }

        [HttpPut]
        [Route(Constants.API_ROUTE_BASE_PATH + Constants.API_ROUTE_CONTROLLER_PATH + "{id}")]
        public async Task<IResponse> Edit(string id, [FromBody]PetModel pet)
        {
            pet.Id = id;
            var response = await PetService.Update(pet);
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }

        [HttpDelete]
        [Route(Constants.API_ROUTE_BASE_PATH + Constants.API_ROUTE_CONTROLLER_PATH + "{id}")]
        public async Task<IResponse> Delete(string id)
        {
            var response = await PetService.Delete(id);
            ReplaceUrlTokens(response);
            SetResponseCode(response.Result);
            return response;
        }


    }
}
