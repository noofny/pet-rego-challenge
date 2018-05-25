using System;
using System.Collections.Generic;

namespace PetRego.Models
{
    public class PagedResponse : Response
    {
        public List<IModel> Values { get; set; }
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }

        public PagedResponse(List<IModel> values, int currentPage, int totalPages, Result result) : base(result)
        {
            Values = values;
            CurrentPage = currentPage;
            TotalPages = totalPages;
        }
        public PagedResponse(string error, Result result) : base(error, result)
        {
        }
    }
}
