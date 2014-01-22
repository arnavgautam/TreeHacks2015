namespace BuildClips.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using BuildClips.Services;
    using BuildClips.Services.Models;
   
    [Authorize]
    public class HomeController : AsyncController
    {
        private readonly VideoService service;

        public HomeController()
        {
            this.service = new VideoService();
        }
        
        // GET: /MyVideos/
        public ActionResult Index()
        {
            return this.View(this.service.GetAll().ToList());
        }

        // GET: /MyVideos/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Video video = await this.service.GetVideoAsync(id);
            if (video == null)
            {
                return this.HttpNotFound();
            }

            return this.View(video);
        }

        // GET: /MyVideos/Create
        public ActionResult Create()
        {
            return this.View();
        }

        // POST: /MyVideos/Create
        [HttpPost]
        public async Task<ActionResult> Create(string title, string description, HttpPostedFileBase videoFile)
        {
            if (ModelState.IsValid && videoFile != null)
            {
                await this.service.CreateVideoAsync(title, description, videoFile.FileName, videoFile.ContentType, videoFile.InputStream);
                return this.RedirectToAction("Index");
            }

            return this.View(new Video { Title = title, Description = description });
        }

        // GET: /MyVideos/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Video video = await this.service.GetVideoAsync(id);
            if (video == null)
            {
                return this.HttpNotFound();
            }

            return this.View(video);
        }

        // POST: /MyVideos/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await this.service.DeleteVideoAsync(id);
            return this.RedirectToAction("Index");
        }

        public ActionResult Ping()
        {
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }
    }
}