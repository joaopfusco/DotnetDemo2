using DotnetDemo2.Domain.Models;
using DotnetDemo2.Repository.Data;
using DotnetDemo2.Service.Interfaces;
using System.Linq.Expressions;

namespace DotnetDemo2.Service.Services
{
    public class BaseService<TModel>(AppDbContext db) : IBaseService<TModel> where TModel : BaseModel
    {
        protected readonly AppDbContext _db = db;

        public virtual IQueryable<TModel> Get(Expression<Func<TModel, bool>>? predicate = null)
        {
            var query = _db.Set<TModel>().AsQueryable();
            if (predicate != null) query = query.Where(predicate);
            return query;
        }

        public virtual IQueryable<TModel> Get(int id)
        {
            return Get(p => p.Id.Equals(id))
                .AsQueryable();
        }

        public virtual async Task<int> Insert(TModel model)
        {
            try
            {
                await _db.AddAsync(model);
                return await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<int> Update(TModel model)
        {
            try
            {
                _db.Update(model);
                return await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<int> Delete(int id)
        {
            try
            {
                TModel model = Get(id).FirstOrDefault() ?? throw new Exception("Entity not found!");
                _db.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                return await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
