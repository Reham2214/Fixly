using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fixly.Models
{
    public enum RequestStatus
    {
        Pending,
        Accepted,
        Rejected,
        Completed
    }

    public class ServiceRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public ApplicationUser Customer { get; set; }

        [Required]
        public string ProviderId { get; set; }

        [ForeignKey("ProviderId")]
        public ApplicationUser Provider { get; set; }

        [Required(ErrorMessage = "Requested date is required.")]
        [DataType(DataType.Date)]
        public DateTime RequestedDate { get; set; }

        [Required(ErrorMessage = "Requested time is required.")]
        [DataType(DataType.Time)]
        public TimeSpan RequestedTime { get; set; }

        [Required(ErrorMessage = "Problem description is required.")]
        [StringLength(500, ErrorMessage = "Problem description cannot exceed 500 characters.")]
        public string ProblemDescription { get; set; }

        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
    }
}