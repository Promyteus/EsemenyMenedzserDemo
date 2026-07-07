namespace EsemenyMenedzser.Requests.Esemeny
{
    public class CreateEsemenyRequestModel
    {
        public string? Nev { get; set; }

        public string? Helyszin { get; set; }

        public string? Orszag { get; set; }

        public int? Kapacitas { get; set; }
    }
}
