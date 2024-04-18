using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CFeedback.Infrastructure.Models;
using CFeedback.Abstractions.Requests;
using CFeedback.Services.Repositories;
using CFeedback.Abstractions.Constants;
using ZstdSharp.Unsafe;

namespace CFeedback.UI.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly FeedbackRepository _feedbackRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly ILogger<FeedbacksController> _logger;

        public FeedbacksController(FeedbackRepository feedbackRepository,
            CategoryRepository categoryRepository,
            ILogger<FeedbacksController> logger)
        {
            _feedbackRepository = feedbackRepository;
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public IActionResult Filter()
        {
            _logger.LogTrace("FeedbacksController Filter Get");

            try
            {
                ViewData["CategoryId"] = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");

                List<Feedback> result;

                var lastMonth = DateTime.UtcNow.AddMonths(-1).Month;

                result = _feedbackRepository.GetAllLastMonth(lastMonth);

                return View(result);

            }
            catch (Exception ex)
            {
                _logger.LogError("Handler: {0}, Method: {1}, Description: {2}",
                    "FeedbacksController",
                    "Filter Get",
                    ex.InnerException);

                throw new BadHttpRequestException(Global.BadRequest);
            }

        }

        [HttpPost]
        public ActionResult Filter(int? CategoryId)
        {
            _logger.LogTrace("FeedbacksController Filter Post");

            try { 

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
            catch (Exception ex)
            {
                _logger.LogError("Handler: {0}, Method: {1}, Description: {2}",
                    "FeedbacksController",
                    "Filter Post",
                    ex.InnerException);

                throw new BadHttpRequestException(Global.BadRequest);
            }
        }

        // GET: Feedbacks
        public async Task<IActionResult> Index()
        {
            _logger.LogTrace("FeedbacksController Index Get");

            try { 
                return View(await _feedbackRepository.GetAllWithCategories().ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError("Handler: {0}, Method: {1}, Description: {2}",
                    "FeedbacksController",
                    "Index Get",
                    ex.InnerException);

                throw new BadHttpRequestException(Global.BadRequest);
            }
        }

        // GET: Feedbacks/Details/5
        public IActionResult Details(int? id)
        {
            _logger.LogTrace("FeedbacksController Details Get");

            try
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
            catch (Exception ex)
            {
                _logger.LogError("Handler: {0}, Method: {1}, Description: {2}",
                    "FeedbacksController",
                    "Details Get",
                    ex.InnerException);

                throw new BadHttpRequestException(Global.BadRequest);
            }
        }

        // GET: Feedbacks/Create
        public IActionResult Create()
        {
            _logger.LogTrace("FeedbacksController Create Get");

            try 
            { 
                ViewData["CategoryId"] = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError("Handler: {0}, Method: {1}, Description: {2}",
                    "FeedbacksController",
                    "Create Get",
                    ex.InnerException);

                throw new BadHttpRequestException(Global.BadRequest);
            }
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FeedbackId,CustomerName,Description,SubmissionDate,CategoryId")] FeedbackRequest request)
        {
            _logger.LogTrace("FeedbacksController Create Post");

            try 
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
            catch (Exception ex)
            {
                _logger.LogError("Handler: {0}, Method: {1}, Description: {2}",
                    "FeedbacksController",
                    "Create Post",
                    ex.InnerException);

                throw new BadHttpRequestException(Global.BadRequest);
            }
        }

        // GET: Feedbacks/Edit/5
        public IActionResult Edit(int? id)
        {
            _logger.LogTrace("FeedbacksController Edit Get");

            try
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
            catch (Exception ex)
            {
                _logger.LogError("Handler: {0}, Method: {1}, Description: {2}",
                    "FeedbacksController",
                    "Edit Get",
                    ex.InnerException);

                throw new BadHttpRequestException(Global.BadRequest);
            }
        }

        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("FeedbackId,CustomerName,Description,SubmissionDate,CategoryId")] FeedbackRequest request)
        {
            _logger.LogTrace("FeedbacksController Edit Post");

            try
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

                    _feedbackRepository.Edit(feedback);
                    _feedbackRepository.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
                ViewData["CategoryId"] = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name", request.CategoryId);
                return View(request);
            }
            catch (Exception ex)
            {
                _logger.LogError("Handler: {0}, Method: {1}, Description: {2}",
                    "FeedbacksController",
                    "Edit Post",
                    ex.InnerException);

                throw new BadHttpRequestException(Global.BadRequest);
            }
        }

        // GET: Feedbacks/Delete/5
        public IActionResult Delete(int? id)
        {
            _logger.LogTrace("FeedbacksController Delete Get");

            try
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
            catch (Exception ex)
            {
                _logger.LogError("Handler: {0}, Method: {1}, Description: {2}",
                    "FeedbacksController",
                    "Delete Get",
                    ex.InnerException);

                throw new BadHttpRequestException(Global.BadRequest);
            }
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _logger.LogTrace("FeedbacksController Delete Post");

            try
            {
                var feedback = _feedbackRepository.GetById(id);
                if (feedback != null)
                {
                    _feedbackRepository.Remove(feedback);
                }

                _feedbackRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError("Handler: {0}, Method: {1}, Description: {2}",
                    "FeedbacksController",
                    "Delete Post",
                    ex.InnerException);

                throw new BadHttpRequestException(Global.BadRequest);
            }
        }

        private bool FeedbackExists(int id)
        {
            return _feedbackRepository.FeedbackExists(id);
        }
    }
}
