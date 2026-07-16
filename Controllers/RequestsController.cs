using Microsoft.AspNetCore.Mvc;
using project.Models; // لكي يتعرف على كود ServiceRequest

namespace project.Controllers
{
    public class RequestsController : Controller
    {
        // قائمة مؤقتة في الذاكرة لحفظ الطلبات وتجربتها
        private static List<ServiceRequest> _requests = new List<ServiceRequest>();
        private static int _nextId = 1;

        // 1. عرض صفحة تقديم الطلب (GET: /Requests/Create)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 2. استلام البيانات المرسلة من النموذج وحفظها (POST: /Requests/Create)
        [HttpPost]
        public IActionResult Create(ServiceRequest request)
        {
            if (ModelState.IsValid)
            {
                request.Id = _nextId++;
                request.RequestDate = DateTime.Now;
                _requests.Add(request);

                // الانتقال لصفحة التأكيد مع إرسال بيانات الطلب لها
                return RedirectToAction("Confirmation", request);
            }

            // إذا كانت البيانات غير صالحة، تظل في نفس الصفحة وتظهر أخطاء المدخلات
            return View(request);
        }

        // 3. عرض صفحة تأكيد نجاح الطلب (GET: /Requests/Confirmation)
        public IActionResult Confirmation(ServiceRequest request)
        {
            return View(request);
        }
    }
}