using CFeedback.Infrastructure;
using CFeedback.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CFeedback.Services.Repositories
{
    public class FeedbackRepository : BaseRespository<Feedback>
    {
        public ILogger<FeedbackRepository> _logger { get; set; }
        public FeedbackRepository(CFeedbackContext context,
            ILogger<FeedbackRepository> logger) :base(context)
        {
            _logger = logger;
        }
        public List<Feedback> GetLastMonthByCategory(int month, int categoryId)
        {
            _logger.LogTrace("FeedbackRepository GetLastMonthByCategory");
            return this.DbSet.Include(f => f.Category).Where(x => x.SubmissionDate.Month == month && x.CategoryId == categoryId).ToList();
        }

        public List<Feedback> GetAllLastMonth(int month)
        {
            _logger.LogTrace("FeedbackRepository GetAllLastMonth");
            return this.DbSet.Include(f => f.Category).Where(x => x.SubmissionDate.Month == month).OrderBy(x => x.Category.Name).ToList();
        }

        public IQueryable<Feedback> GetAllWithCategories()
        {
            _logger.LogTrace("FeedbackRepository GetAllWithCategories");
            return _context.Feedbacks.Include(f => f.Category);
        }

        public Feedback GetById(int feedbackId)
        {
            _logger.LogTrace("FeedbackRepository GetById");
            return _context.Feedbacks
                .Include(f => f.Category)
                .FirstOrDefault(x => x.FeedbackId == feedbackId);
        }

        public bool FeedbackExists(int feedbackId)
        {
            _logger.LogTrace("FeedbackRepository FeedbackExists");
            return _context.Feedbacks.Any(e => e.FeedbackId == feedbackId);
        }
    }
}
