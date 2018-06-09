namespace PetRego.Models
{
    public class Aggregate
    {
        public string Field { get; set; }
        public string Value { get; set; }
        public long Count { get; set; }

        public Aggregate(string field, string value, long count)
        {
            Field = field;
            Value = value;
            Count = count;
        }

    }
}
