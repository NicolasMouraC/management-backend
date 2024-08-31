namespace Backend.Dtos
{
  public class ClientCreateDto
  {
      public string Name { get; set; }
      public string Document { get; set; }
      public string Phone { get; set; }
      public string Address { get; set; }
      public int UserId { get; set; }
  }
}