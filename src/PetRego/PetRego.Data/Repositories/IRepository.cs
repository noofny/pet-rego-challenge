using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Nest;
using Nest.JsonNetSerializer;
using Elasticsearch.Net;

namespace PetRego.Data
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<T> Get(string id);
        Task<List<T>> List();
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(string id);
        Task DeleteAll();
    }

}
