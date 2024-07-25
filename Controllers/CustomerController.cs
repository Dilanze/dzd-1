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
    public class CustomerController : ControllerBase
    {
        private readonly ExpenseTrackerDbContext _dbContext;

        public CustomerController(ExpenseTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private void SetCustomer(Customer entity, CustomerModel model)
        {
            entity.IdentificationNumber = model.IdentificationNumber;
            entity.Fullname = model.Fullname;
        }

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var customers = await _dbContext.Customer.ToListAsync();
            return Ok(customers);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID");
            }

            var entity = await _dbContext.Customer.FindAsync(id);

            if (entity == null)
            {
                return NotFound("Customer not found");
            }

            var model = new CustomerModel
            {
                Id = entity.Id,
                IdentificationNumber = entity.IdentificationNumber,
                Fullname = entity.Fullname,
            };

            return Ok(model);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CustomerModel model)
        {
            var entity = new Customer();
            SetCustomer(entity, model);
            await _dbContext.Customer.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return Ok(entity.Id);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(CustomerModel model)
        {
            var entity = await _dbContext.Customer
            .FindAsync(model.Id);

            if (entity == null)
            {
                return NotFound("Customer not found");
            }

            SetCustomer(entity, model);
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

            var entity = await _dbContext.Customer.FindAsync(id);

            if (entity == null)
            {
                return NotFound("Customer not found");
            }

            _dbContext.Customer.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}




