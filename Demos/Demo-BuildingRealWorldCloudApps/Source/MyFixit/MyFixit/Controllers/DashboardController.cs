namespace MyFixIt.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using MyFixIt.Persistence;

    [Authorize]
    public class DashboardController : Controller
    {
        private IFixItTaskRepository fixItRepository = null;

        public DashboardController(IFixItTaskRepository repository)
        {
            this.fixItRepository = repository;
        }

        // GET: /Dashboard/
        public async Task<ActionResult> Index()
        {
            string currentUser = User.Identity.Name;
            var result = await this.fixItRepository.FindOpenTasksByOwnerAsync(currentUser);

            return this.View(result);
        }

        // GET: /Dashboard/Details/5
        public async Task<ActionResult> Details(int id)
        {
            FixItTask fixItTask = await this.fixItRepository.FindTaskByIdAsync(id);

            if (fixItTask == null)
            {
                return this.HttpNotFound();
            }

            return this.View(fixItTask);
        }

        // GET: /Dashboard/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            FixItTask fixittask = await this.fixItRepository.FindTaskByIdAsync(id);
            if (fixittask == null)
            {
                return this.HttpNotFound();
            }

            return this.View(fixittask);
        }

        // POST: /Dashboard/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, FormCollection form)
        {
            FixItTask fixittask = await this.fixItRepository.FindTaskByIdAsync(id);

            if (this.TryUpdateModel(fixittask, form))
            {
                await this.fixItRepository.UpdateAsync(fixittask);
                return this.RedirectToAction("Index");
            }

            return this.View(fixittask);
        }
    }
}
