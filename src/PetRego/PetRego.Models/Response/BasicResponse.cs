using System;
using System.Collections.Generic;

namespace PetRego.Models
{
    public class BasicResponse : Response
    {
        public BasicResponse(Result result) : base(result) 
        {
        }
        public BasicResponse(string error, Result result) : base(error, result)
        {
        }
    }
}
