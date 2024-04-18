using CFeedback.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFeedback.Infrastructure
{
    public class CFeedbackContext : DbContext
    {
        public CFeedbackContext(DbContextOptions<CFeedbackContext> options): base(options)
        {

        }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Category> Categories { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(
              new Category { CategoryId = 1, Name = "Personel"},
              new Category { CategoryId = 2, Name = "Product" }
            );

            builder.Entity<Feedback>().HasData(
              new Feedback { FeedbackId = 1, CustomerName = "Karen Whales", Description = "Bob didn't treat me well", SubmissionDate = DateTime.UtcNow, CategoryId = 1},
              new Feedback { FeedbackId = 2, CustomerName = "Carlos Duran", Description = "The product was deffective, it didn't turn on", SubmissionDate = DateTime.UtcNow, CategoryId = 2 },

              new Feedback { FeedbackId = 3, CustomerName = "Chris Whales", Description = "Carlos didn't treat me well", SubmissionDate = DateTime.UtcNow.AddMonths(-1), CategoryId = 1 },
              new Feedback { FeedbackId = 4, CustomerName = "Tomy Duran", Description = "Broken part", SubmissionDate = DateTime.UtcNow.AddMonths(-1), CategoryId = 2 }
            );
        }

    }
}
