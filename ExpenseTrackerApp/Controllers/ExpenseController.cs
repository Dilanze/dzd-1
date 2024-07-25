using ExpenseTrackerApp.Data;
using ExpenseTrackerApp.Models;
using Microsoft.AspNetCore.Mvc;
using ExpenseTrackerApp.Dto;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ExpenseTrackerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : BaseController
    {
        private readonly ExpenseTrackerDbContext _dbContext;

        public ExpenseController(ExpenseTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private void SetExpense(Expense entity, ExpenseModel model)
        {
            entity.Category = model.Category;
            entity.Amount = model.Amount;
            entity.PaymentMethod = model.PaymentMethod;
            entity.Date = model.Date;
        }

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var expenses = await _dbContext.Expenses.ToListAsync();
            return Ok(expenses);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID");
            }

            var entity = await _dbContext.Expenses.FindAsync(id);

            if (entity == null)
            {
                return NotFound("Expense not found");
            }

            var model = new ExpenseModel
            {
                Id = entity.Id,
                Category = entity.Category,
                Amount = entity.Amount,
                PaymentMethod = entity.PaymentMethod,
                Date = entity.Date
            };

            return Ok(model);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(ExpenseModel model)
        {
            var entity = new Expense();
            SetExpense(entity, model);
            await _dbContext.Expenses.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return Ok(entity.Id);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(ExpenseModel model)
        {
            var entity = await _dbContext.Expenses.FindAsync(model.Id);

            if (entity == null)
            {
                return NotFound("Expense not found");
            }

            SetExpense(entity, model);
            await _dbContext.SaveChangesAsync();
            return Ok(entity.Id);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID");
            }

            var entity = await _dbContext.Expenses.FindAsync(id);

            if (entity == null)
            {
                return NotFound("Expense not found");
            }

            _dbContext.Expenses.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }


        [HttpGet("TotalExpenses/{customerId}")]
        public async Task<IActionResult> TotalExpenses(int customerId)
        {
            if (customerId <= 0)
            {
                return BadRequest("Invalid ID");
            }

            var totalExpenses = await _dbContext.Expenses
                .Where(e => e.CustomerId == customerId)
                .SumAsync(e => e.Amount);

            return Ok(totalExpenses);
        }
    }
}
