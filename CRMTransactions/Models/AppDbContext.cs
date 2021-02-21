using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMTransactions.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<MissedCall> MissedCalls { get; set; }

        public DbSet<ValidCall> ValidCalls { get; set; }
    }
}
