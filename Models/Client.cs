using System.Text.Json.Serialization;

namespace Backend.Models
{
  public class Client
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Document { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
    
    [JsonIgnore]
    public ICollection<Billing> Billings { get; set; } = new List<Billing>();
  }
}