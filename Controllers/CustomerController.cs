using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fixly.Data;
using Fixly.Models;

namespace Fixly.Controllers;

public class CustomerController : Controller
{
    private readonly AppDbContext _context;

    public CustomerController(AppDbContext context)
    {
        _context = context;
    }


    public async Task<IActionResult> Index(string? category, string? city)
    {
        var providers = _context.ServiceProviderProfiles
            .Include(p => p.User)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
        {
            providers = providers.Where(p => p.ServiceCategory == category);
        }

        if (!string.IsNullOrWhiteSpace(city))
        {
            providers = providers.Where(p => p.User.City == city);
        }

        return View(await providers.ToListAsync());
    }

     public async Task<IActionResult> MyRequests(
    string statusFilter,
    string searchTerm)
{
    var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    var query = _context.ServiceRequests
        .Include(r => r.Provider)
        .Where(r => r.CustomerId == customerId)
        .AsQueryable();

    if (!string.IsNullOrEmpty(statusFilter) &&
        Enum.TryParse<RequestStatus>(statusFilter, out var statusEnum))
    {
        query = query.Where(r => r.Status == statusEnum);
    }

    if (!string.IsNullOrEmpty(searchTerm))
    {
        query = query.Where(r =>
            r.Provider.FullName.Contains(searchTerm));
    }

    var myRequests = await query
        .Join(
            _context.ServiceProviderProfiles,
            request => request.ProviderId,
            profile => profile.UserId,
            (request, profile) => new MyRequestViewModel
            {
                Request = request,
                ServiceCategory = profile.ServiceCategory
            })
        .Where(x =>
            string.IsNullOrEmpty(searchTerm) ||
            x.ServiceCategory.Contains(searchTerm) ||
            x.Request.Provider.FullName.Contains(searchTerm))
        .OrderByDescending(x => x.Request.RequestedDate)
        .ToListAsync();

    ViewBag.StatusFilter = statusFilter;
    ViewBag.SearchTerm = searchTerm;

    return View(myRequests);
}
    

    [HttpGet]
    public IActionResult RequestService(string providerId)
    {
        var request = new ServiceRequest
        {
            ProviderId = providerId
        };

        return View(request);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RequestService(ServiceRequest request)
    {
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        request.CustomerId = customerId;
        request.Status = RequestStatus.Pending;

        ModelState.Remove("Customer");
        ModelState.Remove("Provider");
        ModelState.Remove("CustomerId");

        if (!ModelState.IsValid)
        {
            return View(request);
        }

        _context.ServiceRequests.Add(request);
        await _context.SaveChangesAsync();

        return RedirectToAction("MyRequests");
    }
}