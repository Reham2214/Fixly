using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Fixly.Models;
using Fixly.Data;

namespace Fixly.Controllers;

public class ProviderController : Controller
{

    // private static dynamic[] _request =
    // {
    //    new { Id = 0, CustomerId = "2" , Customer = "Sara Al-Otaibi" ,
    //     ProviderId = "3" ,
    //     Provider = "Yaqoub" ,
    //     Service = "Plumbing" ,
    //     RequestedDate = "2026-07-02",
    //     RequestedTime = "10.00" ,
    //     ProblemDescription = "Leaking kitchen sink pipe." ,
    //     Status = "Pending"} ,

    //    new { Id = 1, CustomerId = "3" , Customer = "Lena Al-Otaibi" ,
    //     ProviderId = "1" ,
    //     Provider = "Mohammed" ,
    //     Service = "Plumbing" ,
    //     RequestedDate = "2026-07-02",
    //     RequestedTime = "10.00" ,
    //     ProblemDescription = "Leaking Bathroom sink pipe." ,
    //     Status = "Pending"}

    // };



    // private static dynamic[] _customer =
    // {
    //    new { CustomerId = 2, FullName = "Sara Al-Otaibi" , City = "Jeddah"},
    //    new { CustomerId = 3, FullName = "Lena Al-Otaibi" , City = "Jeddah"}
    // };

    private readonly AppDbContext _context;

    public ProviderController(AppDbContext context)  //يسوي رفرش (دالة بناء)
    {
        _context = context;
    }

    [Route("request")]
    public IActionResult Index()
    {

        //ViewBag.ServiceRequest = _request;
        var requests = _context.ServiceRequests.ToList();
        return View(requests);
    }

    public IActionResult RequestService(int id)
    {
         var filter = _context.ServiceRequests
        .Where(s => s.CustomerId == id.ToString()) .ToList();

        var filtered = _context.ServiceRequests
        .Where(s => s.CustomerId == id.ToString()) 
        .ToList();

        ViewBag.filterReq = filter;
        ViewBag.Customer = _context.Customers;

        // var filter = _customer
        // .Where(s => s.CustomerId == id)
        // .ToList();

        // ViewBag.filterReq = filter;
        // var request = _request.FirstOrDefault(r => r.Id == id);
        // ViewBag.Customer = request;
        return View();
    }

}
