using CFeedback.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CFeedback.Services.Repositories
{
    public class FeedbackRepository : BaseRespository<Feedback>
    {
        public List<Feedback> GetLastMonthByCategory(int month, int categoryId)
        {
            return this.DbSet.Include(f => f.Category).Where(x => x.SubmissionDate.Month == month && x.CategoryId == categoryId).ToList();
        }

        public List<Feedback> GetAllLastMonth(int month)
        {
            return this.DbSet.Include(f => f.Category).Where(x => x.SubmissionDate.Month == month).OrderBy(x => x.Category.Name).ToList();
        }

        public IQueryable<Feedback> GetAllWithCategories()
        {
            return _context.Feedbacks.Include(f => f.Category);
        }

        public Feedback GetById(int feedbackId)
        {
            return _context.Feedbacks
                .Include(f => f.Category)
                .FirstOrDefault(x => x.FeedbackId == feedbackId);
        }

        public bool FeedbackExists(int feedbackId)
        {
            return _context.Feedbacks.Any(e => e.FeedbackId == feedbackId);
        }
    }
}
