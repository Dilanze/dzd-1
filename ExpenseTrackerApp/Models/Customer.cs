namespace ExpenseTrackerApp.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string IdentificationNumber { get; set; }
        public string Fullname { get; set; }
        public int ExpensesId { get; set; }
        public ICollection<Expense> Expenses { get; set; }

    }
}