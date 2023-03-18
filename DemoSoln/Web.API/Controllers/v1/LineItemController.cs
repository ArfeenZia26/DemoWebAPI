using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApi.DAL;
using webApi.Models;

namespace Web.API.Controllers.v1
{
    public class LineItemController : BaseApiControllerV1
    {
        private readonly WebApiDBContext _context;
        private readonly ILogger<LineItemController> _logger;
        public LineItemController(WebApiDBContext context, ILogger<LineItemController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [MapToApiVersion("1.0")]
        [HttpGet("{invoiceId}")]
        public async Task<IActionResult> Get(long invoiceId)
        {
            try
            {
                var lineItems = await _context.LineItems.Where(l => l.InvoiceId == invoiceId).ToListAsync();
                if (lineItems != null && lineItems.Count > 0)
                    return Ok(lineItems);
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
        [HttpGet("{invoiceId}/{id}")]
        public async Task<IActionResult> Get(long invoiceId, long id)
        {
            try
            {
                var lineItem = await _context.LineItems.FirstOrDefaultAsync(l => l.Id == id && l.InvoiceId == invoiceId) ;
                if (lineItem != null)
                    return Ok(lineItem);
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
        public async Task<IActionResult> Post([FromBody] LineItem lineItem)
        {
            try
            {
                if (lineItem != null)
                {
                    await _context.LineItems.AddAsync(lineItem);
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
            return Ok(lineItem);

        }
        [MapToApiVersion("1.0")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] LineItem lineItem)
        {
            try
            {
                if (lineItem == null || id != lineItem.Id)
                    return BadRequest();

                _context.LineItems.Update(lineItem);
                await _context.SaveChangesAsync();

                return Ok(lineItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception(ex.Message);
            }


        }
        [MapToApiVersion("1.0")]
        [HttpDelete("{invoiceId}/{id}")]
        public async Task<IActionResult> Delete(long invoiceId, long id)
        {
            try
            {
                if (id == 0)
                    return BadRequest();

                var lineItem = await _context.LineItems.FirstOrDefaultAsync(l => l.Id == id && l.InvoiceId == invoiceId);
                if (lineItem == null)
                    return NotFound();

                _context.LineItems.Remove(lineItem);
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
