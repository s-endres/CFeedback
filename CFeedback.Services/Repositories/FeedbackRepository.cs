using CFeedback.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CFeedback.Services.Repositories
{
    public class FeedbackRepository : BaseRespository<Feedback>
    {
        public IList<Feedback> GetLastMontByCategory(int month, int categoryId)
        {
            return this.DbSet.Where(x => x.SubmissionDate.Month == month && x.CategoryId == categoryId).ToList();
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
