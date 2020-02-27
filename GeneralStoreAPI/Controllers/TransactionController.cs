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
    public class TransactionController : ApiController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        //POST
        public async Task<IHttpActionResult> Post(Transaction transaction)
        {
            //if statements with only 1 line of code you can ignore the curly brackets
            if (transaction == null)
                return BadRequest("The request body was empty");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Customer customer = await _context.Customers.FindAsync(transaction.CustomerId);
            if (customer == null)
                return BadRequest("Invalid CustomerId.  No customer by that ID");
            Product product = await _context.Products.FindAsync(transaction.ProductId);
            if (product == null)
                return BadRequest("Invalid ProductId.  No product by that ID");
            transaction.DateOfTransaction = DateTime.Now;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return Ok();
        }

        //GET all
        public async Task<IHttpActionResult> GetAll()
        {
            return Ok(await _context.Transactions.ToListAsync());
        }

        //GET by id
        public async Task<IHttpActionResult> GetById(int id)
        {
           Transaction transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        //PUT by id
        public async Task<IHttpActionResult> Put([FromUri] int id, [FromBody]Transaction transaction)
        {
            if (transaction == null)
                return BadRequest("Transaction body empty");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Transaction existingTransaction = await _context.Transactions.FindAsync(id);
            if (existingTransaction == null)
                return NotFound();
            existingTransaction.CustomerId = transaction.CustomerId;
            existingTransaction.ProductId = transaction.ProductId;
            existingTransaction.ProductCount = transaction.ProductCount;
            existingTransaction.DateOfTransaction = DateTime.Now;
            if (await _context.SaveChangesAsync() == 1)
                return Ok("Transaction Updated");
            return InternalServerError();
        }
        //DELETE
        public async Task<IHttpActionResult> Delete(int id)
        {
            Transaction transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
                return NotFound();
            _context.Transactions.Remove(transaction);
            if (await _context.SaveChangesAsync() == 1)
                return Ok("Transaction Removed");
            return InternalServerError();
        }
    }
}
