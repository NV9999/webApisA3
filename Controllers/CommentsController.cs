using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using WebAssignment3.Data;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace WebAssignment3.Controllers
{
    public class CommentsController : Controller
    {
        private readonly WebAssignment3Context _context;

        public CommentsController(WebAssignment3Context context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            try
            {
                var comment = await _context.Comments
                    //.Include(o => o.Product)
                   // .Include(o => o.User)
                    .ToListAsync();

                if (comment == null || comment.Count == 0)
                {
                    return NotFound(new { success = false, message = "No carts found" });
                }

                var options = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                var json = JsonConvert.SerializeObject(new { success = true, data = comment }, options);
                return new OkObjectResult(json);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Failed to retrieve carts: {ex.Message}" });
            }
        }


        // GET: Comments/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var comment = await _context.Comments
                   // .Include(c => c.Product)
                   // .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (comment == null)
                {
                    return NotFound(new { success = false, message = "Comment not found" });
                }

                var options = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                var json = JsonConvert.SerializeObject(new { success = true, message = "Comment retrieved successfully", data = comment }, options);
                return new OkObjectResult(json);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Failed to retrieve comment: {ex.Message}" });
            }
        }



        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
             public async Task<IActionResult> Create([FromBody] Comment comment)

        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();

                // Return JSON response with success status, message, and the created comment object
                return Json(new { success = true, message = "Comment created successfully", comment });
            }

            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", comment.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", comment.UserId);

            // If ModelState is not valid, return JSON with error message
            return Json(new { success = false, message = "Failed to create comment", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }



        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", comment.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", comment.UserId);
            return View(comment);
        }

        // PUT: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        public async Task<IActionResult> Update(int id, [Bind("Id,ProductId,UserId,Rating,Image,Text")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();

                    // Return JSON response with success status, message, and the updated comment object
                    return Json(new { success = true, message = "Comment updated successfully", comment });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", comment.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", comment.UserId);
            return View(comment);
        }



        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Product)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // DELETE: Comments/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            // Return JSON response with success status and message
            return Json(new { success = true, message = "Comment deleted successfully" });
        }


        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
