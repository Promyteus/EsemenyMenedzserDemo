namespace EsemenyMenedzser.BLL.Modul.Esemeny.DTOs
{
    public record CreateEventDto
    {
        public string? Name { get; set; }

        public string? Location { get; set; }

        public string? Country { get; set; }

        public int? Capacity { get; set; }
    }
}
