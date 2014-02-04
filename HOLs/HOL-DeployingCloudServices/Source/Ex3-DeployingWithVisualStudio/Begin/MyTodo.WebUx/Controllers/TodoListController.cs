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

    public class TodoListController : ApiController
    {
        private TodoItemContext db = new TodoItemContext();

        // GET api/TodoList
        public IQueryable<TodoList> GetTodoLists()
        {
            return this.db.TodoLists.Include("Todos")
                .Where(u => u.UserId == User.Identity.Name)
                .OrderByDescending(u => u.TodoListId);
        }

        // GET api/TodoList/5
        [ResponseType(typeof(TodoList))]
        public async Task<IHttpActionResult> GetTodoList(int id)
        {
            TodoList todolist = await this.db.TodoLists.FindAsync(id);
            if (todolist == null)
            {
                return this.NotFound();
            }

            if (todolist.UserId != User.Identity.Name)
            {
                // Trying to modify a record that does not belong to the user
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Unauthorized));
            }

            return this.Ok(todolist);
        }

        // PUT api/TodoList/5
        public async Task<IHttpActionResult> PutTodoList(int id, TodoList todolist)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (id != todolist.TodoListId)
            {
                return this.BadRequest();
            }

            this.db.Entry(todolist).State = EntityState.Modified;

            try
            {
                await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.TodoListExists(id))
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

        // POST api/TodoList
        [ResponseType(typeof(TodoList))]
        public async Task<IHttpActionResult> PostTodoList(TodoList todolist)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            todolist.UserId = User.Identity.Name;
            this.db.TodoLists.Add(todolist);
            await this.db.SaveChangesAsync();

            return this.CreatedAtRoute("DefaultApi", new { id = todolist.TodoListId }, todolist);
        }

        // DELETE api/TodoList/5
        [ResponseType(typeof(TodoList))]
        public async Task<IHttpActionResult> DeleteTodoList(int id)
        {
            TodoList todolist = await this.db.TodoLists.FindAsync(id);
            if (todolist == null)
            {
                return this.NotFound();
            }

            this.db.TodoLists.Remove(todolist);
            await this.db.SaveChangesAsync();

            return this.Ok(todolist);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }

            base.Dispose(disposing);
        }

        private bool TodoListExists(int id)
        {
            return this.db.TodoLists.Count(e => e.TodoListId == id) > 0;
        }
    }
}