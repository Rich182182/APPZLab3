namespace FitnessClub.BLL.DTOs
{
    public class SubscriptionTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsNetworkAccess { get; set; }
    }
}