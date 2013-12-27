namespace MyFixIt.Controllers
{
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    using MyFixIt.Persistence;

    [Authorize]
    public class TasksController : Controller
    {
        private IFixItTaskRepository fixItRepository = null;
        private IPhotoService photoService = null;

        public TasksController(IFixItTaskRepository repository, IPhotoService photoStore)
        {
            this.fixItRepository = repository;
            this.photoService = photoStore;
        }

        // GET: /FixIt/
        public async Task<ActionResult> Status()
        {
            string currentUser = User.Identity.Name;
            var result = await this.fixItRepository.FindTasksByCreatorAsync(currentUser);

            return this.View(result);
        }

        // GET: /Tasks/Create
        public ActionResult Create()
        {
            return this.View();
        }

        // POST: /Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FixItTask fixittask, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                fixittask.CreatedBy = User.Identity.Name;
                fixittask.PhotoUrl = this.photoService.UploadPhoto(photo);

                await this.fixItRepository.CreateAsync(fixittask);

                return this.RedirectToAction("Success");
            }

            return this.View(fixittask);
        }

        // GET: /Tasks/Success
        public ActionResult Success()
        {
            return this.View();
        }
    }
}