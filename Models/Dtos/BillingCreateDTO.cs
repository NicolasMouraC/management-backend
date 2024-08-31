namespace Backend.Dtos
{
  public class BillingCreateDto
  {
      public string Description { get; set; }
      public decimal Value { get; set; }
      public DateTime dueDate { get; set; }
      public bool isPayed { get; set; }
      public int ClientId { get; set; }
  }
}