using System.Threading.Tasks;
using System.Collections.Generic;
using PetRego.Models;

namespace PetRego.Data
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<T> Get(string id);
        Task<List<T>> Search(string field, string value);
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(string id);
        Task<List<Aggregate>> Count<F>(string groupBy);
    }

}
