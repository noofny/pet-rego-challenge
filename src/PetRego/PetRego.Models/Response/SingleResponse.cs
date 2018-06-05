using System;
using System.Collections.Generic;

namespace PetRego.Models
{
    public class SingleResponse : Response, IHaveSingleResult
    {
        public IModel Data { get; }

        public SingleResponse(Result result, Metadata metadata, IModel data) : base(result, metadata)
        {
            Data = data;
        }
        public SingleResponse(Result result, Metadata metadata, params string[] errors) : base(result, metadata, errors)
        {
        }
    }
}
