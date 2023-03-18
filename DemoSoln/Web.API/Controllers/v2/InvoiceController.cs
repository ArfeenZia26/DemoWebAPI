using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using webApi.DAL;
using webApi.Models;

namespace Web.API.Controllers.v2
{
    public class InvoiceController : BaseApiControllerV2
    {
        private readonly WebApiInMemoryDB _context;
        private readonly ILogger<InvoiceController> _logger;
        public InvoiceController(WebApiInMemoryDB context, ILogger<InvoiceController> logger)
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
                var invoices = await Task.Run(() =>
                {
                    return _context.Invoices.ToList();
                });
                if (invoices != null && invoices?.Count > 0)
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
        [MapToApiVersion("2.0")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var customers = await Task.Run(() =>
            {
                return _context.Invoices.FirstOrDefault(x => x.Id == id);
            });
            if (customers != null)
                return Ok(customers);
            else
                return NotFound();
        }
        [MapToApiVersion("2.0")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Invoice invoice)
        {
            try
            {
                if (invoice != null)
                {
                    await Task.Run(() =>
                    {
                        _context.Invoices.Add(invoice);
                    });
                }
                else
                    return BadRequest("Customer data is incomplete");

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            return Ok(invoice);

        }
        [MapToApiVersion("2.0")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] Invoice invoice)
        {
            if (invoice == null || id != invoice.Id)
                return BadRequest();

            await Task.Run(() =>
            {
                _context.Invoices.Remove(invoice);
                _context.Invoices.Add(invoice);
            });
            
            
            return Ok(invoice);

        }
        [MapToApiVersion("2.0")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (id == 0)
                return BadRequest();

            var invoice = await Task.Run(() =>
            {
                return _context.Invoices.FirstOrDefault(x => x.Id == id);
            });
            if (invoice == null)
                return NotFound();

            _context.Invoices.Remove(invoice);
            return NoContent();
        }
    }
}
