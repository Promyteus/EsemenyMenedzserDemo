using System.ComponentModel.DataAnnotations;

namespace EsemenyMenedzser.DAL.Entities
{
    public class Esemeny
    {
        public int Id { get; set; }

        [Required]
        public string Nev { get; set; }

        [Required]
        [MaxLength(100)]
        public string Helyszin { get; set; }

        public string? Orszag { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Cannot be a negative number.")]
        public int? Kapacitas { get; set; }
    }
}
