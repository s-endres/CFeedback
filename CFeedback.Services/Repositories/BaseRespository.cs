using CFeedback.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CFeedback.Services.Repositories
{
    public class BaseRespository<T> where T : class
    {
        protected CFeedbackContext _context = null;
        protected DbSet<T> DbSet { get; set; }

        public BaseRespository(CFeedbackContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }

        public List<T> GetAll()
        {
            return DbSet.ToList();
        }

        public T Get(int? id)
        {
            return DbSet.Find(id);
        }

        public void Add(T pEntity)
        {
            DbSet.Add(pEntity);
        }

        public void Edit(T pEntity)
        {
            _context.Entry(pEntity).State = EntityState.Modified;
        }

        public void Remove(T pEntity)
        {
            DbSet.Remove(pEntity);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
