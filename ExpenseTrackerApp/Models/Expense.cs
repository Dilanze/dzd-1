namespace ExpenseTrackerApp.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }



    }
}