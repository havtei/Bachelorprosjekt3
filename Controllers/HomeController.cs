using Bachelorprosjekt.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.IO;
using GeneratePDF;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace Bachelorprosjekt.Controllers
{
    [Authorize(Roles = "Fagansvarlig, Admin, Bedrift, Student")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        
       
        public IActionResult Index()
        {
            //TODO
            
            if (User.IsInRole("Bedrift") || User.IsInRole("Student"))
            {
                return Redirect("ProsjektDescriptions/getmyprojects");
            }else if(User.IsInRole("Fagansvarlig"))
            {


                return View("Administrasjon");
            }
            return Redirect("Identity/Account/Login");
        }
        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}