using CFeedback.Infrastructure;
using CFeedback.Infrastructure.Models;

namespace CFeedback.Services.Repositories
{
    public class CategoryRepository : BaseRespository<Category>
    {
        public CategoryRepository(CFeedbackContext context):base(context)
        {
            
        }
    }
}
