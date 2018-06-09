using System;
using System.Net;
using AutoMapper;
using PetRego.Common;
using PetRego.Data;
using PetRego.Models;
using Nest;

namespace PetRego.AppHost
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

            Mapper.Initialize(cfg =>
            {

                // entity to model
                cfg.CreateMap<OwnerEntity, OwnerSummaryModel>();
                cfg.CreateMap<OwnerEntity, OwnerDetailModel>();
                cfg.CreateMap<PetEntity, PetModel>();

                // model to entity
                cfg.CreateMap<OwnerSummaryModel, OwnerEntity>()
                    .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                    .ForMember(dest => dest.DateOfBirth, opt => opt.Ignore())
                    .ForMember(dest => dest.Address, opt => opt.UseValue(new Address()));
                cfg.CreateMap<OwnerDetailModel, OwnerEntity>();
                cfg.CreateMap<PetModel, PetEntity>();

                // nest result to api result
                cfg.CreateMap<Nest.Result, Models.Result>().ConvertUsing(src =>
                {
                    switch (src)
                    {
                        case Nest.Result.Created:
                            return Models.Result.Created;

                        case Nest.Result.Deleted:
                            return Models.Result.Deleted;

                        case Nest.Result.Error:
                            return Models.Result.InternalError;

                        case Nest.Result.NotFound:
                            return Models.Result.NotFound;

                        case Nest.Result.Updated:
                            return Models.Result.Updated;

                        default:
                            return Models.Result.Unsupported;
                    }
                });

                // result to status code
                cfg.CreateMap<Models.Result, HttpStatusCode>().ConvertUsing(src =>
                {
                    switch (src)
                    {
                        case Models.Result.BadRequest:
                            return HttpStatusCode.BadRequest;

                        // todo - given more time I would implement a middleware handler for more
                        // concise exception types, returning finer grain detail about the type
                        // of error that occurred on the server.
                        case Models.Result.InternalError:
                            return HttpStatusCode.InternalServerError;

                        case Models.Result.NotFound:
                            return HttpStatusCode.NotFound;

                        case Models.Result.Unsupported:
                            return HttpStatusCode.UnsupportedMediaType;

                        case Models.Result.Created:
                            return HttpStatusCode.Created;

                        // For these it is common to respond with 204 (NoContent).
                        // Because this is a REST API which therefore returns hypermedia,
                        // technically I feel it is more correct to return 202 (Accepted).
                        case Models.Result.Updated:
                        case Models.Result.Deleted:
                            return HttpStatusCode.Accepted;

                        default:
                            return HttpStatusCode.OK;
                    }
                });

                _isConfigured = true;
            });
        }



    }
}
