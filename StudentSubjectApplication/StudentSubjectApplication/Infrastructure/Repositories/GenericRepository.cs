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

        public async Task AddAsync(TEntity entity)
        {
            await dbSet1.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            TEntity entity = await dbSet1.FindAsync(id);
            return entity;
        }

        public async Task<TEntity> GetByNameAsync(string name)
        {
            TEntity entity = await dbSet1
                .FirstOrDefaultAsync(e => EF.Property<string>(e, "name").ToLower().Trim() == name.ToLower().Trim());
            return entity;
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await dbSet1.ToListAsync();
        }
        public async Task UpdateAsync(TEntity entity)
        {
            dbSet1.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            dbSet1.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<List<TRelatedEntity>> GetRelatedEntitiesAsync(TEntity entity)
        {
            var entityId = entity.GetType().GetProperty("id")?.GetValue(entity)?.ToString();
            var relatedEntityList = await dbSet1
                .Where(e => EF.Property<string>(e, "id") == entityId)
                .SelectMany(e => EF.Property<IEnumerable<TRelatedEntity>>(e, "relatedEntities"))
                .ToListAsync();
            return relatedEntityList;
        }


    }
}
