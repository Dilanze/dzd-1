using ExpenseTrackerApp.Models;

namespace ExpenseTrackerApp.Dto
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string IdentificationNumber { get; set; }
        public string Fullname { get; set; }
        public ICollection<Expense> Expenses { get; set; }

    }
}