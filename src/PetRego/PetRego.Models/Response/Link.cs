namespace PetRego.Models
{
    public class Link
    {
        public string Rel { get; private set; }
        public string Href { get; set; } // public to allow token replacements
        public string Action { get; private set; }

        public Link(string rel, string href, string action)
        {            
            Rel = rel;
            Href = href;
            Action = action;
        }

        public static Link Self(string href, string action)
        {
            return new Link("self", href, action);
        }

        public static Link Edit(string href, string action)
        {
            return new Link("edit", href, action);
        }

        public static Link Delete(string href, string action)
        {
            return new Link("delete", href, action);
        }

        public static Link Previous(string href, string action)
        {
            return new Link("previous", href, action);
        }

        public static Link Next(string href, string action)
        {
            return new Link("next", href, action);
        }

        public static Link Related(string href, string action)
        {
            return new Link("related", href, action);
        }

        public static Link Custom(string rel, string href, string action)
        {
            return new Link(rel, href, action);
        }

    }



}
