using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApi.DAL;
using webApi.Models;

namespace Web.API.Controllers.v2
{
    public class AddressController : BaseApiControllerV2
    {
        private readonly WebApiInMemoryDB _context;
        private readonly ILogger<AddressController> _logger;
        public AddressController(WebApiInMemoryDB context, ILogger<AddressController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [MapToApiVersion("2.0")]
        [HttpGet("{customerId}")]
        public async Task<IActionResult> Get(long customerId)
        {
            try
            {
                var addresses = await Task.Run(() => 
                { 
                    return _context.Addresses.Where(a => a.CustomerId == customerId).ToList(); 
                });

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
        [MapToApiVersion("2.0")]
        [HttpGet("{customerId}/{id}")]
        public async Task<IActionResult> Get(long customerId, long id)
        {
            try
            {
                var address = await Task.Run(() =>
                {
                    return _context.Addresses.FirstOrDefault(a => a.Id == id && a.CustomerId == customerId);
                }); 
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
        [MapToApiVersion("2.0")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Address address)
        {
            try
            {
                if (address != null)
                {
                    await Task.Run(() =>
                    {
                        _context.Addresses.Add(address);

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
            return Ok(address);

        }
        [MapToApiVersion("2.0")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] Address address)
        {
            try
            {
                if (address == null || id != address.Id)
                    return BadRequest();
                var Address = _context.Addresses.FirstOrDefault(a => a.Id == id);
                if (Address == null) return NotFound();

                await Task.Run(() =>
                {
                    _context.Addresses.Remove(Address);
                    _context.Addresses.Add(address);
                });

                return Ok(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception(ex.Message);
            }

        }
        [MapToApiVersion("2.0")]
        [HttpDelete("{customerId}/{id}")]
        public async Task<IActionResult> Delete(long customerId, long id)
        {
            try
            {
                if (id == 0)
                    return BadRequest();

                var address = _context.Addresses.FirstOrDefault(a => a.Id == id && a.CustomerId == customerId);
                if (address == null)
                    return NotFound();

                await Task.Run(() =>
                {
                    _context.Addresses.Remove(address);
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
