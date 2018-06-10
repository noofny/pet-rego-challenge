using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PetRego.Models
{
    public class FoodCount
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public FoodType Type { get; set; }
        public long Count { get; set; }

        public FoodCount(FoodType type, long count)
        {
            Type = type;
            Count = count;
        }
    }


}
