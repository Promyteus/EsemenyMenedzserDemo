namespace EsemenyMenedzser.BLL.Modul.Esemeny.DTOs
{
    public class UpdateEventDto
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Location { get; set; }

        public string? Country { get; set; }

        public int? Capacity { get; set; }
    }
}
