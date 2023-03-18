using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApi.DAL;
using webApi.Models;

namespace Web.API.Controllers.v2
{
    public class LineItemController : BaseApiControllerV2
    {
        private readonly WebApiInMemoryDB _context;
        private readonly ILogger<LineItemController> _logger;
        public LineItemController(WebApiInMemoryDB context, ILogger<LineItemController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [MapToApiVersion("2.0")]
        [HttpGet("{invoiceId}")]
        public async Task<IActionResult> Get(long invoiceId)
        {
            try
            {
                var lineItems = await Task.Run(() =>
                {
                    return _context.LineItems.Where(a => a.InvoiceId == invoiceId).ToList();
                });

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
        [MapToApiVersion("2.0")]
        [HttpGet("{invoiceId}/{id}")]
        public async Task<IActionResult> Get(long invoiceId, long id)
        {
            try
            {
                var lineItem = await Task.Run(() =>
                {
                    return _context.LineItems.FirstOrDefault(i => i.Id == id && i.InvoiceId == invoiceId);
                });
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
        [MapToApiVersion("2.0")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LineItem lineItem)
        {
            try
            {
                if (lineItem != null)
                {
                    await Task.Run(() =>
                    {
                        _context.LineItems.Add(lineItem);

                    });

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
        [MapToApiVersion("2.0")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] LineItem lineItem)
        {
            try
            {
                if (lineItem == null || id != lineItem.Id)
                    return BadRequest();
                var LineItem = _context.LineItems.FirstOrDefault(l => l.Id == id);
                if (LineItem == null) return NotFound();

                await Task.Run(() =>
                {
                    _context.LineItems.Remove(LineItem);
                    _context.LineItems.Add(lineItem);
                });

                return Ok(lineItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception(ex.Message);
            }

        }
        [MapToApiVersion("2.0")]
        [HttpDelete("{invoiceId}/{id}")]
        public async Task<IActionResult> Delete(long invoiceId, long id)
        {
            try
            {
                if (id == 0)
                    return BadRequest();

                var lineItem = _context.LineItems.FirstOrDefault(l => l.Id == id && l.InvoiceId == invoiceId);
                if (lineItem == null)
                    return NotFound();

                await Task.Run(() =>
                {
                    _context.LineItems.Remove(lineItem);
                });

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
