namespace MobileService.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.OData;

    using Microsoft.WindowsAzure.Mobile.Service;

    using MobileService.DataObjects;
    using MobileService.Models;

    public class TodoItemController : TableController<TodoItem>
    {
        // GET tables/TodoItem
        public IQueryable<TodoItem> GetAllTodoItem()
        {
            return this.Query(); 
        }

        // GET tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<TodoItem> GetTodoItem(string id)
        {
            return this.Lookup(id);
        }

        // PATCH tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<TodoItem> PatchTodoItem(string id, Delta<TodoItem> patch)
        {
             return this.UpdateAsync(id, patch);
        }

        // POST tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task<IHttpActionResult> PostTodoItem(TodoItem item)
        {
            TodoItem current = await InsertAsync(item);
            return this.CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTodoItem(string id)
        {
             return this.DeleteAsync(id);
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            this.DomainManager = new EntityDomainManager<TodoItem>(context, Request, Services);
        }
    }
}