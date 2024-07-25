using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quartz;
using ExpenseTrackerApp.Data;


namespace MasrafTakipAPI.Jobs
{
    public class ExpenseAggregationJob : IJob
    {
        private readonly ExpenseTrackerDbContext _context;

        public ExpenseAggregationJob(ExpenseTrackerDbContext context)
        {
            _context = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var now = DateTime.Now;
            var startOfWeek = now.AddDays(-(int)now.DayOfWeek);
            var startOfMonth = new DateTime(now.Year, now.Month, 1);

            var dailyExpenses = await _context.Expenses
                .Where(t => t.Date.Date == now.Date)
                .GroupBy(t => t.CustomerId)
                .Select(g => new { CustomerId = g.Key, Total = g.Sum(t => t.Amount) })
                .ToListAsync();

            var weeklyExpenses = await _context.Expenses
                .Where(t => t.Date >= startOfWeek && t.Date <= now)
                .GroupBy(t => t.CustomerId)
                .Select(g => new { CustomerId = g.Key, Total = g.Sum(t => t.Amount) })
                .ToListAsync();

            var monthlyExpenses = await _context.Expenses
                .Where(t => t.Date >= startOfMonth && t.Date <= now)
                .GroupBy(t => t.CustomerId)
                .Select(g => new { CustomerId = g.Key, Total = g.Sum(t => t.Amount) })
                .ToListAsync();


        }
    }
}
