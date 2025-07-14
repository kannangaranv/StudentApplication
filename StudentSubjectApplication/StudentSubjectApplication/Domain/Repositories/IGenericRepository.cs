using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSubjectApplication.Domain.Repositories
{
    public interface IGenericRepository<TEntity, TRelatedEntity> 
        where TEntity : class 
        where TRelatedEntity : class
    {
        Task AddAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(string id);
        Task<TEntity> GetByNameAsync(string name);
        Task<List<TEntity>> GetAllAsync();
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<List<TRelatedEntity>> GetRelatedEntitiesAsync(TEntity entity);

        Task RemoveEntityFromRelatedEntity(
            TEntity entity, 
            TRelatedEntity relatedEntity);
    }
}
