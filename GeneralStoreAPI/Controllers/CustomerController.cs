using GeneralStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeneralStoreAPI.Controllers
{
    public class CustomerController : ApiController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        //POST
        public async Task<IHttpActionResult> Post(Customer customer)
        {
            if (customer == null)
            {
                return BadRequest("Request body was empty.  Please provide a model");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return Ok("Customer added successfully.");
        }
        //GET All

        public async Task<IHttpActionResult> GetAll()
        {
            return Ok(await _context.Customers.ToListAsync());
        }
        //GET by ID
        public async Task<IHttpActionResult> GetById(int id)
        {
            Customer customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return BadRequest("Customer does not exist");
            return Ok(customer);
        }

        //PUT by Id

        public async Task<IHttpActionResult> PutById([FromUri] int id, [FromBody] Customer customer)
        {
            if (customer == null)
                return BadRequest("Customer empty. Please submit valid customer information.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Customer existingCustomer = await _context.Customers.FindAsync(id);
            if (existingCustomer == null)
                return NotFound();
            existingCustomer.Address = customer.Address;
            existingCustomer.Email = customer.Email;
            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            await _context.SaveChangesAsync();
            return Ok("Customer updated successfully");
        }

        //DELETE by Id
        public async Task<IHttpActionResult> DeleteByID(int id)
        {
            Customer existingCustomer = await _context.Customers.FindAsync(id);
            if (existingCustomer == null)
                return NotFound();
            _context.Customers.Remove(existingCustomer);
            if (await _context.SaveChangesAsync() == 1)
                return Ok("Customer Removed");
            return InternalServerError();
        }


    }
}
