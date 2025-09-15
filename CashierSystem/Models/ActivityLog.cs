
using System.ComponentModel.DataAnnotations;

namespace CashierSystem.Models
{
    public partial class ActivityLog
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string UserName { get; set; } = null!;

        public string IpAddress { get; set; } = null!;

        public string UrlAccessed { get; set; } = null!;

        public string RolseUser { get; set; } = null!;

        public DateTime Timestamp { get; set; }
    }

}
