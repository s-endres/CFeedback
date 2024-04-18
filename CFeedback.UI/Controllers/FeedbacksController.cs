using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CFeedback.Infrastructure.Models;
using CFeedback.Abstractions.Requests;
using CFeedback.Services.Repositories;

namespace CFeedback.UI.Controllers
{
    public class FeedbacksController : Controller
    {
        private FeedbackRepository _feedbackRepository;
        private CategoryRepository _categoryRepository;

        public FeedbacksController(FeedbackRepository feedbackRepository, CategoryRepository categoryRepository)
        {
            _feedbackRepository = feedbackRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Filter()
        {
            ViewData["CategoryId"] = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");

            List<Feedback> result;

            var lastMonth = DateTime.UtcNow.AddMonths(-1).Month;

            result = _feedbackRepository.GetAllLastMonth(lastMonth);

            return View(result);
        }

        [HttpPost]
        public ActionResult Filter(int? CategoryId)
        {
            List<Feedback> result;

            var lastMonth = DateTime.UtcNow.AddMonths(-1).Month;

            if (CategoryId == null || CategoryId == 0)
            {
                result = _feedbackRepository.GetAllLastMonth(lastMonth);
                ViewData["CategoryId"] = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            }
            else
            {
                result = _feedbackRepository.GetLastMonthByCategory(lastMonth, (int)CategoryId);
                ViewData["CategoryId"] = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name", (int)CategoryId);
            }
            
            return PartialView("Filter", result);
        }

        // GET: Feedbacks
        public async Task<IActionResult> Index()
        {
            return View(await _feedbackRepository.GetAllWithCategories().ToListAsync());
        }

        // GET: Feedbacks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = _feedbackRepository.GetById((int)id);

            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // GET: Feedbacks/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FeedbackId,CustomerName,Description,SubmissionDate,CategoryId")] FeedbackRequest request)
        {
            if (ModelState.IsValid)
            {
                Feedback feedback = new Feedback();

                feedback.FeedbackId = request.FeedbackId;
                feedback.CustomerName = request.CustomerName;
                feedback.Description = request.Description;
                feedback.SubmissionDate = request.SubmissionDate;
                feedback.CategoryId = request.CategoryId;

                _feedbackRepository.Add(feedback);
                _feedbackRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name", request.CategoryId);
            return View(request);
        }

        // GET: Feedbacks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = _feedbackRepository.Get(id);
            if (feedback == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name", feedback.CategoryId);
            return View(feedback);
        }

        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FeedbackId,CustomerName,Description,SubmissionDate,CategoryId")] FeedbackRequest request)
        {
            if (id != request.FeedbackId)
            {
                return NotFound();
            }

            if (!FeedbackExists(request.FeedbackId))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Feedback feedback = new Feedback();

                feedback.FeedbackId = request.FeedbackId;
                feedback.CustomerName = request.CustomerName;
                feedback.Description = request.Description;
                feedback.SubmissionDate = request.SubmissionDate;
                feedback.CategoryId = request.CategoryId;

                try
                {
                    _feedbackRepository.Edit(feedback);
                    _feedbackRepository.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name", request.CategoryId);
            return View(request);
        }

        // GET: Feedbacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = _feedbackRepository.GetById((int)id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feedback = _feedbackRepository.GetById(id);
            if (feedback != null)
            {
                _feedbackRepository.Remove(feedback);
            }

            _feedbackRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedbackExists(int id)
        {
            return _feedbackRepository.FeedbackExists(id);
        }
    }
}
