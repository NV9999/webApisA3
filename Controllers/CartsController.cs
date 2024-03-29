using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAssignment3.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace WebAssignment3.Controllers
{
    public class CartsController : Controller
    {
        private readonly WebAssignment3Context _context;

        public CartsController(WebAssignment3Context context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            try
            {
                var carts = await _context.Carts
                    //.Include(o => o.Product)
                    //.Include(o => o.User)
                    .ToListAsync();

                if (carts == null || carts.Count == 0)
                {
                    return NotFound(new { success = false, message = "No carts found" });
                }

                var options = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                var json = JsonConvert.SerializeObject(new { success = true, data = carts}, options);
                return new OkObjectResult(json);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Failed to retrieve carts: {ex.Message}" });
            }
        }

        // GET: Carts/Details/5
        [HttpGet("getCartById/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(o => o.Product)
                    .Include(o => o.User)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (cart == null)
                {
                    return NotFound(new { success = false, message = "Cart not found" });
                }

                var options = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                var json = JsonConvert.SerializeObject(new { success = true, message = "Cart retrieved successfully", data = cart }, options);
                return new OkObjectResult(json);


            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Failed to retrieve cart: {ex.Message}" });
            }

        }

        // GET: Carts/Create


        // POST: Carts/Create
        /* [HttpPost]
         public async Task<IActionResult> Create([Bind("ProductIds,Quantities,UserId")] Cart cart)
         {
             try
             {
                 if (ModelState.IsValid)
                 {
                     _context.Add(cart);
                     await _context.SaveChangesAsync();
                     return Json(new { success = true, message = "Cart created successfully", cart });
                 }
                 else
                 {
                     // Setup ViewData for ProductIds and UserId
                     ViewData["product_ids"] = new SelectList(_context.Products, "Id", "Id", cart.ProductIds);
                     ViewData["user_id"] = new SelectList(_context.Users, "Id", "Id", cart.UserId);

                     // If ModelState is not valid, return JSON with error message
                     return Json(new { success = false, message = "Failed to create cart", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                 }
             }
             catch (Exception ex)
             {
                 // Include inner exception message if available
                 string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                 return Json(new { success = false, message = $"Failed to create cart: {errorMessage}" });
             }
         }*/

        [HttpPost]
        public async Task<IActionResult> PostCart([FromBody] Cart cart)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(cart);
                    await _context.SaveChangesAsync();
                    return Ok(new { success = true, message = "Cart created successfully", cart });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Invalid model state", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Failed to create cart: {ex.Message}" });
            }
        }



        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["ProductIds"] = new SelectList(_context.Products, "Id", "Id", cart.ProductIds);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", cart.UserId);
            return View(cart);
        }


        // PUT: Carts/Edit/5
        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] Cart cart)
        {
            if (id != cart.Id)
            {
                return BadRequest(new { success = false, message = "Invalid ID in the request body" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid model state", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            try
            {
                var existingCart = await _context.Carts.FindAsync(id);
                if (existingCart == null)
                {
                    return NotFound(new { success = false, message = "Cart not found" });
                }

                // Update the existing cart with the new values
                existingCart.ProductIds = cart.ProductIds;
                existingCart.Quantities = cart.Quantities;
                existingCart.UserId = cart.UserId;

                _context.Update(existingCart);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Cart updated successfully", cart = existingCart });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Failed to update cart: {ex.Message}" });
            }
        }



        // GET: Carts/Delete/5
        /*public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Product)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }*/

        // DELETE: Cart/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cart = await _context.Carts.FindAsync(id);
                if (cart == null)
                {
                    return NotFound(new { success = false, message = "Cart not found" });
                }

                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Cart deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Failed to delete cart: {ex.Message}" });
            }
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
