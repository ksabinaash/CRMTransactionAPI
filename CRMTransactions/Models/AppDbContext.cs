using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMTransactions.Models;

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

        public DbSet<CallAction> CallAction { get; set; }

        public DbSet<CallPurpose> CallPurpose { get; set; }

        public DbSet<WhiteList> WhiteList { get; set; }
    }
}
