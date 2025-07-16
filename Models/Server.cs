using System.ComponentModel.DataAnnotations;

namespace bi_dashboard_api.Models
{
    public class Server
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public bool isOnline { get; set; }
    }
}