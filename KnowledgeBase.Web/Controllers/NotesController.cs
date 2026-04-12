using Microsoft.AspNetCore.Mvc;
using KnowledgeBase.Data;

namespace KnowledgeBase.Web.Controllers
{
    public class NotesController : Controller
    {
        private readonly NoteRepository _repo;

        public NotesController(IConfiguration config)
        {
            string connStr = config.GetConnectionString("DefaultConnection") ??
                throw new ArgumentException("Connection string 'DefaultConnection' is missing or empty.");
            _repo = new NoteRepository(connStr);
        }

        public IActionResult Index(string searchTerm = "")
        {
            var model = _repo.SearchNotes(searchTerm);
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(string header, string body)
        {
            _repo.SaveNote(header, body);
            return RedirectToAction("Index");
        }
    }
}
