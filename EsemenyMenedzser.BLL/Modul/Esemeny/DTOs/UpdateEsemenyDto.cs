namespace EsemenyMenedzser.BLL.Modul.Esemeny.DTOs
{
    public class UpdateEsemenyDto
    {
        public int? Id { get; set; }

        public string? Nev { get; set; }

        public string? Helyszin { get; set; }

        public string? Orszag { get; set; }

        public int? Kapacitas { get; set; }
    }
}
