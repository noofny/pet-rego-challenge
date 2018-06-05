using System;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;

namespace PetRego.Models
{
    public interface IHaveMetadata
    {
        Metadata Metadata { get; }
    }
    public interface IHaveErrors
    {
        List<string> Errors { get; }
    }
    public interface IHaveSingleResult
    {
        IModel Data { get; }
    }
    public interface IHaveMultiResult
    {
        List<IModel> Data { get; }
        int CurrentPage { get; }
        int TotalPages { get; }
        int PageSize { get; }
    }
    public interface IResponse : IHaveMetadata, IHaveErrors
    {
        Result Result { get; }
    }


    public class Response : IResponse
    {
        public Metadata Metadata { get; }
        public List<string> Errors { get; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Result Result { get; }

        public Response(Result result, Metadata metadata)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }
            Result = result;
            Metadata = metadata;
        }
        public Response(Result result, Metadata metadata, params string[] errors)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }
            if (errors == null || errors.Length < 1)
            {
                throw new ArgumentException("One or more values must be provided!", nameof(errors));
            }
            Result = result;
            Metadata = metadata;
            Errors = errors.ToList();
        }
    }







}
