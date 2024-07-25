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
    public class CustomerController : BaseController
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


// using ExpenseTrackerApp.Data;
// using ExpenseTrackerApp.Dto;
// using ExpenseTrackerApp.Models;
// using Microsoft.AspNetCore.Mvc;

// namespace ExpenseTrackerApp.Controllers;

// public class CustomerController : BaseController
// {
//     readonly ExpenseTrackerDbContext dbContext;

//     public CustomerController(ExpenseTrackerDbContext dbContext)
//     {
//         this.dbContext = dbContext;
//     }


//     public IActionResult Index()
//     {
//         var result = dbContext.Customer.OrderBy(p => p.Fullname).Select(p => new CustomerModel
//         {
//             Id = p.Id,
//             IdentificationNumber =p.IdentificationNumber,
//             Fullname = p.Fullname


//         }).ToList();

//         return View(result);
//     }

//     public IActionResult Edit(int id)
//     {
//         var result = dbContext.Customer.Where(p => p.Id == id).Select(p => new CustomerModel
//         { Id = p.Id,
//             IdentificationNumber =p.IdentificationNumber,
//             Fullname = p.Fullname
//         }).FirstOrDefault();

//         ViewBag.Categories = dbContext.Customer.OrderBy(P => P.Fullname).ToList();

//         return View(result);
//     }

//     [HttpPost]
//     public IActionResult CreateOrUpdate(CustomerModel model)
//     {

//         var entity = new Customer();
//         if (model.Id == 0)
//         {
//             dbContext.Customer.Add(entity);
//         }
//         else
//         {
//             entity = dbContext.Customer.FirstOrDefault(p => p.Id == model.Id);
//         }
//         entity.IdentificationNumber = model.IdentificationNumber;
//         entity.Fullname = model.Fullname;


//         dbContext.SaveChanges();

//         return RedirectToAction("Index");
//     }
//     public IActionResult Delete(int id)
//     {
//         var entity = dbContext.Customer.FirstOrDefault(p => p.Id == id);
//         dbContext.Customer.Remove(entity);
//         dbContext.SaveChanges();
//         return RedirectToAction("Index");
//     }
// }



