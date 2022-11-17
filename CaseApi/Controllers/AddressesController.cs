using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CaseApi.Models;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace CaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly AddressContext _context;

        public AddressesController(AddressContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the list of all Addresses, or search for a specific one.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/Address
        ///     {        
        ///       "HouseNumber": "88",
        ///       "Street": "Examplestreet",
        ///       "PostalCode": "2000BC",    
        ///       "City": "Examplecity",     
        ///       "Country": "Examplecountry",     
        ///       "DescendingOrder": "false",     
        ///     }
        /// </remarks>
        /// <returns>The list of Addresses.</returns>
        // GET: api/Addresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddresses(int? HouseNumber, string Street = "", string PostalCode = "", string City = "", string Country = "", bool DescendingOrder = false)
        {
            //ToDo: Add sorting function

            //This uses the parameters to search for addresses
            List<Address> addresses = await _context.Addresses.Where(a =>
                a.Street.Contains(Street) &&
                a.HouseNumber == (!HouseNumber.HasValue ? a.HouseNumber : HouseNumber) &&
                a.PostalCode.Contains(PostalCode) &&
                a.City.Contains(City) &&
                a.Country.Contains(Country)
            ).ToListAsync();

            if (DescendingOrder)
            {
                addresses.Reverse();
            }

            return addresses;
        }

        // Tried to do this but I was unable to do it in time
        // GET: api/Addresses/Distance
        /*[HttpGet("Distance")]
        public async Task<ActionResult<IEnumerable<Address>>> GetDistance(AddressContext context)
        {
            return addresses;
        }*/

        /// <summary>
        /// Gets Address by Id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/Address/{Id}
        ///     {        
        ///       "Id": "0"
        ///     }
        /// </remarks>
        /// <returns>Address by Id.</returns>
        // GET: api/Addresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddress(int id)
        {
            var address = await _context.Addresses.FindAsync(id);

            if (address == null)
            {
                return NotFound();
            }

            return address;
        }

        /// <summary>
        /// Updates Address by Id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/Address/{Id}
        ///     {        
        ///       "Id": "0",   
        ///       "HouseNumber": "88",
        ///       "Street": "Examplestreet",
        ///       "PostalCode": "2000BC",    
        ///       "City": "Examplecity",     
        ///       "Country": "Examplecountry",     
        ///     }
        /// </remarks>
        // PUT: api/Addresses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddress(int id, Address address)
        {
            if (id != address.Id)
            {
                return BadRequest();
            }

            _context.Entry(address).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Post new Address.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Address
        ///     {        
        ///       "HouseNumber": "88",
        ///       "Street": "Examplestreet",
        ///       "PostalCode": "2000BC",    
        ///       "City": "Examplecity",     
        ///       "Country": "Examplecountry",     
        ///     }
        /// </remarks>
        /// <returns>The new Address.</returns>
        // POST: api/Addresses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Address>> PostAddress(Address address)
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAddress", new { id = address.Id }, address);
        }

        /// <summary>
        /// Deletes Address by Id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE api/Address/{Id}
        ///     {        
        ///       "Id": "0"
        ///     }
        /// </remarks>
        // DELETE: api/Addresses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AddressExists(int id)
        {
            return _context.Addresses.Any(e => e.Id == id);
        }
    }
}
