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
        void Add(TEntity entity);
        Task AddAsync(TEntity entity);
        TEntity GetById(string id);
        Task<TEntity> GetByIdAsync(string id);
        TEntity GetByName(string name);
        Task<TEntity> GetByNameAsync(string name);
        List<TEntity> GetAll();
        Task<List<TEntity>> GetAllAsync();
        void Update(TEntity entity);
        Task UpdateAsync(TEntity entity);
        void Delete(TEntity entity);
        Task DeleteAsync(TEntity entity);
        List<TRelatedEntity> GetRelatedEntities(TEntity entity);
        Task<List<TRelatedEntity>> GetRelatedEntitiesAsync(TEntity entity);
        void RemoveRelatedEntity(TEntity entity, TRelatedEntity relatedEntity);
        Task RemoveRelatedEntityAsync(TEntity entity, TRelatedEntity relatedEntity);
    }
}
