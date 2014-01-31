namespace MyTodo.WebUx.Controllers
{
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

    public class TodoItemController : ApiController
    {
        private TodoItemContext db = new TodoItemContext();

        // PUT api/TodoItem/5
        public async Task<IHttpActionResult> PutTodoItem(int id, TodoItem todoitem)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (id != todoitem.TodoItemId)
            {
                return this.BadRequest();
            }

            this.db.Entry(todoitem).State = EntityState.Modified;

            try
            {
                await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.TodoItemExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return this.StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/TodoItem
        [ResponseType(typeof(TodoItem))]
        public async Task<IHttpActionResult> PostTodoItem(TodoItem todoitem)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            this.db.TodoItems.Add(todoitem);
            await this.db.SaveChangesAsync();

            return this.CreatedAtRoute("DefaultApi", new { id = todoitem.TodoItemId }, todoitem);
        }

        // DELETE api/TodoItem/5
        [ResponseType(typeof(TodoItem))]
        public async Task<IHttpActionResult> DeleteTodoItem(int id)
        {
            TodoItem todoitem = await this.db.TodoItems.FindAsync(id);
            if (todoitem == null)
            {
                return this.NotFound();
            }

            this.db.TodoItems.Remove(todoitem);
            await this.db.SaveChangesAsync();

            return this.Ok(todoitem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }

            base.Dispose(disposing);
        }

        private bool TodoItemExists(int id)
        {
            return this.db.TodoItems.Count(e => e.TodoItemId == id) > 0;
        }
    }
}