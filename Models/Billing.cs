using System.Text.Json.Serialization;

namespace Backend.Models
{
  public class Billing
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public decimal Value { get; set; }
    public DateTime dueDate { get; set; }
    public bool isPayed { get; set; }

    public int ClientId { get; set; }

    [JsonIgnore]
    public Client Client { get; set; }
  }
}