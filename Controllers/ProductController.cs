using Microsoft.AspNetCore.Mvc;
using mvc_1.Data;

using mvc_1.Models.Domain;
using mvc_1.Models.View_Models;
using mvc_1.Repositories;


namespace mvc_1.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult viewbag()
        {
            ViewBag.Title = "Welcome";
            return View();
        }

        public ActionResult index()
        {
            List<Product> products = new List<Product>();
            ViewData["heading"] = products;

            return View();

        }
        
        public IActionResult Index1()
        {
            TempData["Firstvalue"] = "hello world";
            return View();
        }
        public IActionResult About()
        {
            ViewBag.Message = "This is about page";
            if (TempData["Firstvalue"]!=null)
            {
                TempData.Keep();
                return RedirectToAction("Contact");
            }
            return View();
        }
    }
}
