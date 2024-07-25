using System.Reflection;
using ExpenseTrackerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApp.Data
{
    public class ExpenseTrackerDbContext : DbContext
    {

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        public DbSet<User> User { get; set; }
        public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly(),
                p => p.GetInterfaces().Any(
                    c => c.IsGenericType && c.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));

        }



    }
}