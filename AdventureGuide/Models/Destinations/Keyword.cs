namespace AdventureGuide.Models.Destinations
{
    public class Keyword
    {
        public int Id { get; set; }

        public int DestinationId { get; set; }

        public DestinationKeyword DestinationKeyword { get; set; }
    }

    public enum DestinationKeyword
    {
        Museum,
        Park,
        Art,
        Architecture,
        Shopping,
        Bar,
        Club,
        Beach,
        Church
    }
}
