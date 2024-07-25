using ExpenseTrackerApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTrackerApp.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
            builder.Property(t => t.IdentificationNumber).IsRequired().HasMaxLength(11);
            builder.Property(t => t.Fullname).IsRequired().HasMaxLength(300);
            builder.HasMany(t => t.Expenses).WithOne(t => t.Customer).HasForeignKey(t => t.CustomerId);




        }

    }
}