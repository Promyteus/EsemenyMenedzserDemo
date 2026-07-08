using System.ComponentModel.DataAnnotations;

namespace EsemenyMenedzser.DAL.Entities
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Location { get; set; } = string.Empty;

        public string? Country { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Cannot be a negative number.")]
        public int? Capacity { get; set; }
    }
}
