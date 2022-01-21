using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class DataContext : DbContext
    {

            public DbSet<Transaction> Transaction { get; set; }

            public DataContext(DbContextOptions<DataContext> options) : base(options)
            {

            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
            }

            private void SetDefaultValues(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Transaction>().Property(p => p.DateCreated).
                    HasDefaultValue("getDate()").ValueGeneratedOnAdd();

                
            }

            private void SetIndexColumn(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Transaction>().HasIndex(p => new { p.TransactionId });
  
            }
        }

    
}
