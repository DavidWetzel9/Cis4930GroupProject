namespace AdventureGuide.Models.Destinations
{
    public class Review
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int DestinationId { get; set; }

        public decimal Rating { get; set; }

        public string Comment { get; set; }
    }
}
