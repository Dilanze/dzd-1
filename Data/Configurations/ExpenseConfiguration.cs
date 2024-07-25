using ExpenseTrackerApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTrackerApp.Data.Configurations
{
    public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ToTable("Expense");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
            builder.Property(t => t.Category).IsRequired().HasMaxLength(150);
            builder.Property(t => t.Amount).IsRequired().HasMaxLength(300);
            builder.Property(t => t.Amount).IsRequired().HasMaxLength(300);
            builder.Property(t => t.PaymentMethod).IsRequired().HasMaxLength(300);





        }

    }
}