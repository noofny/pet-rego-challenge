using System.Threading.Tasks;
using System.Collections.Generic;

namespace PetRego.Data
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<T> Get(string id);
        Task<List<T>> List();
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(string id);
        Task<bool> DeleteAll();
    }

}
