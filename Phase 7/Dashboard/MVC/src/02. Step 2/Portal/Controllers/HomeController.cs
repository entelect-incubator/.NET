namespace Portal.Controllers;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Common.Models.Base;
using Portal.Models;

public class HomeController : Controller
{
    public IActionResult Index() => this.View();

    public IActionResult Privacy() => this.View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });

    [HttpGet]
    public IActionResult GetAddress() => this.PartialView("_Address", new AddressBase());
}
