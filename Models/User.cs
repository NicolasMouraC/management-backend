using Microsoft.AspNetCore.Identity;

namespace Backend.Models
{
  public class User : IdentityUser<int>
  {
    public string Name { get; set; }

    public ICollection<Client> Clients { get; set; } = new List<Client>();
  }
}
