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
        TEntity GetById(string id);
        TEntity GetByName(string name);
        List<TEntity> GetAll();
        void Update(TEntity entity);
        void Delete(TEntity entity);
        List<TRelatedEntity> GetRelatedEntities(TEntity entity);
        void RemoveRelatedEntity(TEntity entity, TRelatedEntity relatedEntity);
    }
}
