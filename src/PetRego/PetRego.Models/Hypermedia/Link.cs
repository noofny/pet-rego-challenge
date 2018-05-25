using System;

namespace PetRego.Models
{
    public abstract class Link
    {
        public string Rel { get; private set; }
        public string Href { get; private set; }
        public string Title { get; private set; }

        public Link(string rel, string href, string title = null)
        {            
            Rel = rel;
            Href = href;
            Title = title;
        }
    }

    public class SelfLink : Link
    {
        public SelfLink(string href, string title = null) : base("self", href, title)
        {
        }
    }

    public class EditLink : Link
    {
        public EditLink(string href, string title = null) : base("edit", href, title)
        {
        }
    }

    public class DeleteLink : Link
    {
        public DeleteLink(string href, string title = null) : base("delete", href, title)
        {
        }
    }

    public class RelatedLink : Link
    {
        public RelatedLink(string href, string title = null) : base("related", href, title)
        {
        }
    }


}
