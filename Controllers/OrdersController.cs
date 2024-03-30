using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebAssignment3.Data;

namespace WebAssignment3.Controllers
{
    public class OrdersController : Controller
    {
        private readonly WebAssignment3Context _context;

        public OrdersController(WebAssignment3Context context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            try
            {
                var orders = await _context.Orders
                  //  .Include(o => o.Cart)
                    .ToListAsync();

                if (orders == null || orders.Count == 0)
                {
                    return NotFound(new { success = false, message = "No orders found" });
                }

                var options = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                var json = JsonConvert.SerializeObject(new { success = true, data = orders }, options);
                return new OkObjectResult(json);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Failed to retrieve orders: {ex.Message}" });
            }
        }

        // GET: Orders/Details/5
        [HttpGet("getOrderById/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.Cart)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (order == null)
                {
                    return NotFound(new { success = false, message = "Order not found" });
                }

                var options = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                var json = JsonConvert.SerializeObject(new { success = true, message = "Order retrieved successfully", data = order }, options);
                return new OkObjectResult(json);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Failed to retrieve order: {ex.Message}" });
            }
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CartId"] = new SelectList(_context.Carts, "Id", "Id");
            return View();
        }


        // POST: Orders/Create
        [HttpPost]
            
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Order object is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Order created successfully", order });
            }
            catch (DbUpdateException ex)
            {
                // Include inner exception message if available
                string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, new { success = false, message = $"Failed to create order: {errorMessage}" });
            }
        }

        /* public async Task<IActionResult> Create([FromBody] Order order)
         {
             if (order == null)
             {
                 return BadRequest("Order object is null");
             }

             if (!ModelState.IsValid)
             {
                 return BadRequest(ModelState);
             }

             try
             {
                 _context.Orders.Add(order);
                 await _context.SaveChangesAsync();
                 return Ok(new { success = true, message = "Order created successfully", order });
             }
             catch (DbUpdateException ex)
             {
                 // Include inner exception message if available
                 string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                 return StatusCode(500, new { success = false, message = $"Failed to create order: {errorMessage}" });
             }
         }*/



        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CartId"] = new SelectList(_context.Carts, "Id", "Id", order.CartId);
            return View(order);
        }

        // PUT: Orders/Update/5
        [HttpPut]
        public async Task<IActionResult> Edit(int id, [FromBody] Order order)
        {
            if (id != order.Id)
            {
                return BadRequest("Invalid order ID");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Order updated successfully", order });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.Id))
                {
                    return NotFound(new { success = false, message = "Order not found" });
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Failed to update order: {ex.Message}" });
            }
        }


        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Cart)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    return NotFound(new { success = false, message = "Order not found" });
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Order deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Failed to delete order: {ex.Message}" });
            }
        }


        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
