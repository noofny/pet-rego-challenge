using System;
using AutoMapper;
using PetRego.Common;
using PetRego.Data;
using PetRego.Models;

namespace PetRego.Api
{
    public static class AutomapperConfig
    {
        static bool _isConfigured;

        public static void Configure()
        {
            if (_isConfigured)
            {
                return;
            }

            Mapper.Initialize(cfg => {

                // entity to model
                cfg.CreateMap<OwnerEntity, OwnerModel>();
                cfg.CreateMap<PetEntity, PetModel>();

                // model to entity
                cfg.CreateMap<OwnerModel, OwnerEntity>();
                cfg.CreateMap<PetModel, PetEntity>();

            });

            _isConfigured = true;
        }

    }
}
