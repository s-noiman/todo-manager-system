using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoManager.Models
{
    public class Todo
    {
        [Required]
        [Index(IsUnique = true)]
        public string id { get; set; }

        [Required]
        public string value { get; set; }

        [Required]
        public bool completed { get; set; } = false;
    }
}