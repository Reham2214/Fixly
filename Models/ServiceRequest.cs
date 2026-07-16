using System.ComponentModel.DataAnnotations;

namespace project.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى إدخال اسم العميل")]
        [Display(Name = "اسم العميل")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "يرجى إدخال بريدك الإلكتروني")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        [Required(ErrorMessage = "يرجى اختيار الخدمة المطلوبة")]
        [Display(Name = "نوع الخدمة")]
        public string ServiceType { get; set; }

        [Required(ErrorMessage = "يرجى كتابة تفاصيل الطلب")]
        [MinLength(10, ErrorMessage = "يجب أن تكون التفاصيل 10 أحرف على الأقل")]
        [Display(Name = "تفاصيل الطلب")]
        public string Description { get; set; }

        [Display(Name = "تاريخ الطلب")]
        public DateTime RequestDate { get; set; } = DateTime.Now;
    }
}