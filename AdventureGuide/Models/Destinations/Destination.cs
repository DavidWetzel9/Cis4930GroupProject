using System.Collections.Generic;

namespace AdventureGuide.Models.Destinations
{
    public class Destination
    {
        public Destination()
        {
            ImagePaths = new List<ImagePath>();
            Reviews = new List<Review>();
            Keywords = new List<Keyword>();
        }

        public int Id { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string Zip { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public decimal? RatingSum { get; set; }

        public int? RatingCount { get; set; }

        public decimal? Rating => (RatingSum / RatingCount);

        public List<ImagePath> ImagePaths { get; set; }

        public List<Review> Reviews { get; set; }

        public List<Keyword> Keywords { get; set; }

    }
}
