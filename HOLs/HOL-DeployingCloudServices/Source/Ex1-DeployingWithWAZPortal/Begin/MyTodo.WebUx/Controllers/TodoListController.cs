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
    public class TodoListController : ApiController
    {
        private TodoItemContext db = new TodoItemContext();

        // GET api/TodoList
        public IQueryable<TodoList> GetTodoLists()
        {
            return db.TodoLists.Include("Todos")
                .Where(u => u.UserId == User.Identity.Name)
                .OrderByDescending(u => u.TodoListId);
        }

        // GET api/TodoList/5
        [ResponseType(typeof(TodoList))]
        public async Task<IHttpActionResult> GetTodoList(int id)
        {
            TodoList todolist = await db.TodoLists.FindAsync(id);
            if (todolist == null)
            {
                return NotFound();
            }

            if (todolist.UserId != User.Identity.Name)
            {
                // Trying to modify a record that does not belong to the user
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Unauthorized));
            }

            return Ok(todolist);
        }

        // PUT api/TodoList/5
        public async Task<IHttpActionResult> PutTodoList(int id, TodoList todolist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != todolist.TodoListId)
            {
                return BadRequest();
            }

            db.Entry(todolist).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoListExists(id))
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

        // POST api/TodoList
        [ResponseType(typeof(TodoList))]
        public async Task<IHttpActionResult> PostTodoList(TodoList todolist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            todolist.UserId = User.Identity.Name;
            db.TodoLists.Add(todolist);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = todolist.TodoListId }, todolist);
        }

        // DELETE api/TodoList/5
        [ResponseType(typeof(TodoList))]
        public async Task<IHttpActionResult> DeleteTodoList(int id)
        {
            TodoList todolist = await db.TodoLists.FindAsync(id);
            if (todolist == null)
            {
                return NotFound();
            }

            db.TodoLists.Remove(todolist);
            await db.SaveChangesAsync();

            return Ok(todolist);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TodoListExists(int id)
        {
            return db.TodoLists.Count(e => e.TodoListId == id) > 0;
        }
    }
}