using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApi.DAL;
using webApi.Models;

namespace Web.API.Controllers.v1
{
    public class InvoiceController : BaseApiControllerV1
    {
        private readonly WebApiDBContext _context;
        private readonly ILogger<InvoiceController> _logger;
        public InvoiceController(WebApiDBContext context, ILogger<InvoiceController> logger)
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
                var invoices = await _context.Invoices.ToListAsync();
                if (invoices != null && invoices.Count > 0)
                    return Ok(invoices);
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
                var invoice = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
                if (invoice != null)
                    return Ok(invoice);
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
        public async Task<IActionResult> Post([FromBody] Invoice invoice)
        {
            try
            {
                if (invoice != null)
                {
                    await _context.Invoices.AddAsync(invoice);
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
            return Ok(invoice);

        }
        [MapToApiVersion("1.0")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Invoice invoice)
        {
            try
            {
                if (invoice == null || id != invoice.Id)
                    return BadRequest();

                _context.Invoices.Update(invoice);
                await _context.SaveChangesAsync();

                return Ok(invoice);
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

                var invoice = await _context.Invoices.FirstOrDefaultAsync(u => u.Id == id);
                if (invoice == null)
                    return NotFound();

                _context.Invoices.Remove(invoice);
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
