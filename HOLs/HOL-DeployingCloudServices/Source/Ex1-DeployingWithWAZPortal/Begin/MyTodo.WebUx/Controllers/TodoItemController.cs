using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MyTodo.WebUx.Models;

namespace MyTodo.WebUx.Controllers
{
    public class TodoItemController : ApiController
    {
        private TodoItemContext db = new TodoItemContext();

        // PUT api/TodoItem/5
        public async Task<IHttpActionResult> PutTodoItem(int id, TodoItem todoitem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != todoitem.TodoItemId)
            {
                return BadRequest();
            }

            db.Entry(todoitem).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/TodoItem
        [ResponseType(typeof(TodoItem))]
        public async Task<IHttpActionResult> PostTodoItem(TodoItem todoitem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TodoItems.Add(todoitem);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = todoitem.TodoItemId }, todoitem);
        }

        // DELETE api/TodoItem/5
        [ResponseType(typeof(TodoItem))]
        public async Task<IHttpActionResult> DeleteTodoItem(int id)
        {
            TodoItem todoitem = await db.TodoItems.FindAsync(id);
            if (todoitem == null)
            {
                return NotFound();
            }

            db.TodoItems.Remove(todoitem);
            await db.SaveChangesAsync();

            return Ok(todoitem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TodoItemExists(int id)
        {
            return db.TodoItems.Count(e => e.TodoItemId == id) > 0;
        }
    }
}