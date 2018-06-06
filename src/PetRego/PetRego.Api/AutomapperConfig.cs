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
                cfg.CreateMap<OwnerEntity, OwnerSummaryModel>()
                    .ForMember(dest => dest.NumberOfPets, opt => opt.Ignore());
                cfg.CreateMap<OwnerEntity, OwnerDetailModel>()
                    .ForMember(dest => dest.NumberOfPets, opt => opt.Ignore());
                cfg.CreateMap<PetEntity, PetModel>();

                // model to entity
                cfg.CreateMap<OwnerSummaryModel, OwnerEntity>()
                    .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                    .ForMember(dest => dest.DateOfBirth, opt => opt.Ignore())
                    .ForMember(dest => dest.Address, opt => opt.UseValue(new Address()));
                cfg.CreateMap<OwnerDetailModel, OwnerEntity>();
                cfg.CreateMap<PetModel, PetEntity>();

            });
            _isConfigured = true;
        }

    }
}
