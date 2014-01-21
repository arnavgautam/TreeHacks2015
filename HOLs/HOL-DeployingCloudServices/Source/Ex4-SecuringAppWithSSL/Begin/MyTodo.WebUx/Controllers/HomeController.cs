namespace MyTodo.WebUx.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;
    using MyTodo.WebUx.Models;

    [HandleError]
    public class HomeController : Controller
    {
        private readonly TaskRepository repository;

        public HomeController()
            : this(null)
        {
        }

        public HomeController(TaskRepository taskRepository)
        {
            this.repository = taskRepository ?? new TaskRepository();
        }

        public ActionResult Index()
        {
            if (this.repository.TablesExist() && Roles.RoleExists("Owner"))
            {
                ViewData.Add("AuthenticatedUser", User.IsInRole("Owner"));
                return this.View("Lists");
            }
            else
            {
                return this.View("Welcome");
            }
        }

        public ActionResult List(string listId, string page, string row)
        {
            TaskList list = this.repository.GetTaskList(listId);

            if (list != null && (Request.IsAuthenticated || list.IsPublic))
            {
                ViewData.Add("ListId", list.ListId);
                ViewData.Add("ListName", list.Name);
                ViewData.Add("AuthenticatedUser", User.IsInRole("Owner"));
                return View("Tasks");
            }

            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            return this.View();
        }
    }
}
