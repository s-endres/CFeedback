using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFeedback.Infrastructure.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public string CustomerName { get; set; }
        public string Description { get; set; }
        public DateTime SubmissionDate { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
