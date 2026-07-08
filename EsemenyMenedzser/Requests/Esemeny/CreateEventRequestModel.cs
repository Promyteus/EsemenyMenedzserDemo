namespace EsemenyMenedzser.Requests.Esemeny
{
    public class CreateEventRequestModel
    {
        public string? Name { get; set; }

        public string? Location { get; set; }

        public string? Country { get; set; }

        public int? Capacity { get; set; }
    }
}
