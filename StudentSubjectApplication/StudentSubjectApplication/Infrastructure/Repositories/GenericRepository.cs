using Azure;
using Microsoft.EntityFrameworkCore;
using StudentSubjectApplication.Domain.Entities;
using StudentSubjectApplication.Domain.Repositories;
using StudentSubjectApplication.Infrastructure.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StudentSubjectApplication.Infrastructure.Repositories
{
    public class GenericRepository<TEntity, TRelatedEntity> : IGenericRepository<TEntity, TRelatedEntity>
        where TEntity : class
        where TRelatedEntity : class
    {
        private readonly StudentContext context;
        private readonly DbSet<TEntity> dbSet1;
        private readonly DbSet<TRelatedEntity> dbSet2;


        public GenericRepository(StudentContext context)
        {
            this.context = context;
            dbSet1 = context.Set<TEntity>();
            dbSet2 = context.Set<TRelatedEntity>();
        }

        public void Add(TEntity entity)
        {
            dbSet1.Add(entity);
            context.SaveChanges();
        }

        public TEntity GetById(string id)
        {
            TEntity entity = dbSet1.Find(id);
            return entity;
        }

        public TEntity GetByName(string name)
        {
            TEntity entity = dbSet1
                .FirstOrDefault(e => EF.Property<string>(e, "name").ToLower().Trim() == name.ToLower().Trim());
            return entity;
        }

        public List<TEntity> GetAll()
        {
            return dbSet1.ToList();
        }

        public void Update(TEntity entity)
        {
            dbSet1.Update(entity);
            context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            dbSet1.Remove(entity);
            context.SaveChanges();
        }

        public List<TRelatedEntity> GetRelatedEntities(TEntity entity)
        {
            var entityId = entity.GetType().GetProperty("id")?.GetValue(entity)?.ToString();

            var relatedEntityList = dbSet1
                .Where(e => EF.Property<string>(e, "id") == entityId)
                .SelectMany(e => EF.Property<IEnumerable<TRelatedEntity>>(e, "relatedEntities"))
                .ToList();

            return relatedEntityList;
        }

        public void RemoveRelatedEntity(TEntity entity, TRelatedEntity relatedEntity)
        {
            
            var entityId = entity.GetType().GetProperty("id")?.GetValue(entity)?.ToString();

            var entityInDb = dbSet1
                .Include(e => EF.Property<IEnumerable<TRelatedEntity>>(e, "relatedEntities"))
                .FirstOrDefault(e => EF.Property<string>(e, "id") == entityId);

            var relatedList = entityInDb.GetType().GetProperty("relatedEntities").GetValue(entityInDb) as IList<TRelatedEntity>;

            if (relatedList != null && relatedList.Contains(relatedEntity))
            {
                relatedList.Remove(relatedEntity);
                context.SaveChanges();
            }
        }

    }
}
