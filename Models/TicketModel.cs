using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class TicketModel
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string PerformerId { get; set; } = string.Empty; // FK → AspNetUsers.Id
        public DateTime ExpireDate { get; set; }
        public int Weight { get; set; }
        public string Zone { get; set; } = "Todo"; // Zone (Todo, Review, Testing, Done)
    }
}