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
    public class ProductController : ApiController
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        //POST

        public async Task<IHttpActionResult> Post(Product product)
        {
            if (product == null)
            {
                return BadRequest("Body empty. Please send a model.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok("Product added successfully.");
        }

        //GET All

        public async Task<IHttpActionResult> Get()
        {
            return Ok(await _context.Products.ToListAsync());
        }
        // GET By Id

        public async Task<IHttpActionResult> GetById(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            if (product != null)
                return Ok(product);
            return NotFound();
        }

        //Put by Id
        public async Task<IHttpActionResult> PutById([FromUri] int id, [FromBody] Product product)
        {
            if (product == null)
                return BadRequest("Product empty.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Product existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
                return NotFound();
            existingProduct.Name = product.Name;
            existingProduct.Brand = product.Brand;
            existingProduct.Price = product.Price;
            existingProduct.QuantityOnHand = product.QuantityOnHand;
            if (await _context.SaveChangesAsync() == 1)
                return Ok("Product Updated");
            return InternalServerError();
        }

        //Delete
        public async Task<IHttpActionResult> DeleteById(int id)
        {
            Product existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
                return BadRequest();
            _context.Products.Remove(existingProduct);
            if (await _context.SaveChangesAsync() == 1)
                return Ok("Product Removed");
            return InternalServerError();

        }
    }
}
