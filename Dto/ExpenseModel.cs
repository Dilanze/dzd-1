using ExpenseTrackerApp.Models;

namespace ExpenseTrackerApp.Dto
{
    public class ExpenseModel
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }

        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }



    }
}