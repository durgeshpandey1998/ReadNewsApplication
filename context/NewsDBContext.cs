using Microsoft.EntityFrameworkCore;
using NewsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsApp.context
{
    public class NewsDBContext : DbContext
    {
        public NewsDBContext(DbContextOptions<NewsDBContext> options) : base(options)
        {

        }
        public DbSet<NewsContent> NewsContent { get; set; }
        public DbSet<Register> Registers { get; set; }
        public DbSet<PaymentModel> Payments {get;set;}
    }
}
