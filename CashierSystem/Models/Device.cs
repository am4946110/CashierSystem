using System.ComponentModel.DataAnnotations;

namespace CashierSystem.Models;
public partial class Device
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string UserAgent { get; set; } = null!;

    public string Platform { get; set; } = null!;

    public string Language { get; set; } = null!;

    public int ScreenWidth { get; set; }

    public int ScreenHeight { get; set; }

    public string IpAddress { get; set; } = null!;

    public string Port { get; set; } = null!;

    public DateTime Timestamp { get; set; }
}
