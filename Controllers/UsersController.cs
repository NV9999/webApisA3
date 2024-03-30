﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAssignment3.Data;

namespace WebAssignment3.Controllers
{
    public class UsersController : Controller
    {
        private readonly WebAssignment3Context _context;

        public UsersController(WebAssignment3Context context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return Json(users);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return Json(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User object is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "User created successfully", user });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      //  [HttpPost]
       // [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password,Username,PurchaseHistory,ShippingAddress")] User user)
        //{
          //  if (id != user.Id)
           // {
            //    return NotFound();
            //}

          //  if (ModelState.IsValid)
          //  {
           //     try
            //    {
             //       _context.Update(user);
              //      await _context.SaveChangesAsync();
            //    }
          //      catch (DbUpdateConcurrencyException)
          //      {
           //         if (!UserExists(user.Id))
            //        {
             //           return NotFound();
              //      }
            //        else
          //          {
          //              throw;
          //          }
          //      }
        //        return RedirectToAction(nameof(Index));
        //    }
       //     return View(user);
     //   }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        /*  //DELETE: Users/Delete/5
          [HttpDelete]
          public async Task<IActionResult> Delete(int Id)
          {
              var user = await _context.Users.FindAsync(Id);
              if (user == null)
              {
                  return NotFound();
              }

              _context.Users.Remove(user);
              await _context.SaveChangesAsync();

              return Ok(new { message = "User deleted successfully." });
          }*/

        // DELETE: Users/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            var user = await _context.Users.FindAsync(Id);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully." });
        }





        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        // PUT: Users/Edit/5
        [HttpPut]
        public async Task<IActionResult> Edit(int Id, [FromBody] User user)
        {
            if (Id != user.Id)
            {
                return BadRequest("User ID does not match.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Json(user);
        }

    }
}
