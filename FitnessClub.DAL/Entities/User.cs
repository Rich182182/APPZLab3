namespace FitnessClub.DAL.Entities
{
    public class User : BaseEntity
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int? ClientId { get; set; }
        public Client Client { get; set; }
    }
}