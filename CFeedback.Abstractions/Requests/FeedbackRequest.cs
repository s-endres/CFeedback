using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFeedback.Abstractions.Requests
{
    public class FeedbackRequest
    {
        public int FeedbackId { get; set; }
        public string CustomerName { get; set; }
        public string Description { get; set; }
        public DateTime SubmissionDate { get; set; }
        public int CategoryId { get; set; }
    }
}
