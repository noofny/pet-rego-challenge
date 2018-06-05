using System;
using System.Collections.Generic;

namespace PetRego.Models
{
    public class MultiResponse : Response, IHaveMultiResult
    {
        public List<IModel> Data { get; }
        public int CurrentPage { get; }
        public int TotalPages { get; }
        public int PageSize { get; }

        public MultiResponse(Result result, Metadata metadata, List<IModel> data) : base(result, metadata)
        {
            Data = data;
        }
        public MultiResponse(Result result, Metadata metadata, List<IModel> data, int currentPage, int totalPages, int pageSize) : base(result, metadata)
        {
            Data = data;
            CurrentPage = currentPage;
            TotalPages = totalPages;
            PageSize = pageSize;
        }
        public MultiResponse(Result result, Metadata metadata, params string[] errors) : base(result, metadata, errors)
        {
        }

    }
}
