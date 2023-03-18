using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using webApi.DAL;
using webApi.Models;

namespace Web.API.Controllers.v2
{
    public class CustomerController : BaseApiControllerV2
    {
        private readonly WebApiInMemoryDB _context;
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(WebApiInMemoryDB context, ILogger<CustomerController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [MapToApiVersion("2.0")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var customers = await Task.Run(() =>
                {
                    return _context.Customers.ToList();
                });
                if (customers != null && customers?.Count > 0)
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
        [MapToApiVersion("2.0")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var customers = await Task.Run(() =>
            {
                return _context.Customers.FirstOrDefault(x => x.Id == id);
            });
            if (customers != null)
                return Ok(customers);
            else
                return NotFound();
        }
        [MapToApiVersion("2.0")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            try
            {
                if (customer != null)
                {
                    await Task.Run(() =>
                    {
                        _context.Customers.Add(customer);
                    });
                }
                else
                    return BadRequest("Customer data is incomplete");

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            return Ok(customer);

        }
        [MapToApiVersion("2.0")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Customer customer)
        {
            if (customer == null || id != customer.Id)
                return BadRequest();

            await Task.Run(() =>
            {
                _context.Customers.Remove(customer);
                _context.Customers.Add(customer);
            });
            
            
            return Ok(customer);

        }
        [MapToApiVersion("2.0")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
                return BadRequest();

            var cust = await Task.Run(() =>
            {
                return _context.Customers.FirstOrDefault(x => x.Id == id);
            });
            if (cust == null)
                return NotFound();

            _context.Customers.Remove(cust);
            return NoContent();
        }
    }
}
