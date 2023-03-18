using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApi.DAL;
using webApi.Models;

namespace Web.API.Controllers.v1
{
    public class AddressController : BaseApiControllerV1
    {
        private readonly WebApiDBContext _context;
        private readonly ILogger<AddressController> _logger;
        public AddressController(WebApiDBContext context, ILogger<AddressController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [MapToApiVersion("1.0")]
        [HttpGet("{customerId}")]
        public async Task<IActionResult> Get(long customerId)
        {
            try
            {
                var addresses = await _context.Addresses.Where(a => a.CustomerId == customerId).ToListAsync();
                if (addresses != null && addresses.Count > 0)
                    return Ok(addresses);
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
        [HttpGet("{customerId}/{id}")]
        public async Task<IActionResult> Get(long customerId, long id)
        {
            try
            {
                var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id && a.CustomerId == customerId) ;
                if (address != null)
                    return Ok(address);
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
        public async Task<IActionResult> Post([FromBody] Address address)
        {
            try
            {
                if (address != null)
                {
                    await _context.Addresses.AddAsync(address);
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
            return Ok(address);

        }
        [MapToApiVersion("1.0")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] Address address)
        {
            try
            {
                if (address == null || id != address.Id)
                    return BadRequest();

                _context.Addresses.Update(address);
                await _context.SaveChangesAsync();

                return Ok(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception(ex.Message);
            }


        }
        [MapToApiVersion("1.0")]
        [HttpDelete("{customerId}/{id}")]
        public async Task<IActionResult> Delete(long customerId, long id)
        {
            try
            {
                if (id == 0)
                    return BadRequest();

                var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id && a.CustomerId == customerId);
                if (address == null)
                    return NotFound();

                _context.Addresses.Remove(address);
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
