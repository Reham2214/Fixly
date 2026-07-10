using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fixly.Data;
using Fixly.Models;

namespace Fixly.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerRequestsController : Controller
    {
        private readonly AppDbContext _context;

        public CustomerRequestsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> MyRequests(string statusFilter, string searchTerm)
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
                    r.Service.Contains(searchTerm) ||
                    r.Provider.FullName.Contains(searchTerm));
            }

            var myRequests = await query
                .OrderByDescending(r => r.RequestedDate)
                .ToListAsync();

            ViewBag.StatusFilter = statusFilter;
            ViewBag.SearchTerm = searchTerm;

            return View(myRequests);
        }
    }
}