using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bi_dashboard_api.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]


        public Customer Customer { get; set; }  

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        public DateTime Placed { get; set; }
        public DateTime? Completed { get; set; }
    }
}
