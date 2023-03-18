using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApi.DAL;
using webApi.Models;

namespace Web.API.Controllers.v1
{
    public class CustomerController : BaseApiControllerV1
    {
        private readonly WebApiDBContext _context;
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(WebApiDBContext context, ILogger<CustomerController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [MapToApiVersion("1.0")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var customers = await _context.Customers.ToListAsync();
                if (customers != null && customers.Count > 0)
                    return Ok(customers);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception(ex.Message);
            }
            
        }
        [MapToApiVersion("1.0")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
                if (customer != null)
                    return Ok(customer);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception(ex.Message);
            }
            
        }
        [MapToApiVersion("1.0")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            try
            {
                if (customer != null)
                {
                    await _context.Customers.AddAsync(customer);
                    await _context.SaveChangesAsync();

                }
                else
                    return BadRequest("Customer data is incomplete");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception(ex.Message);
            }
            return Ok(customer);

        }
        [MapToApiVersion("1.0")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Customer customer)
        {
            try
            {
                if (customer == null || id != customer.Id)
                    return BadRequest();

                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();

                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception(ex.Message);
            }
           

        }
        [MapToApiVersion("1.0")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id == 0)
                    return BadRequest();

                var cust = await _context.Customers.FirstOrDefaultAsync(u => u.Id == id);
                if (cust == null)
                    return NotFound();

                _context.Customers.Remove(cust);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception(ex.Message);
            }
            

            return NoContent();
        }
    }
}
