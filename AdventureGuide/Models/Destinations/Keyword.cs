using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdventureGuide.Models.Destinations
{
    public class Keyword
    {
        private string _keywordString;
        private DestinationKeyword _keywordEnum;

        public int Id { get; set; }

        public int DestinationId { get; set; }

        public DestinationKeyword KeywordEnum
        {
            get
            {
                return _keywordEnum;
            }
            set
            {
                _keywordEnum = value;
                _keywordString = _keywordEnum.ToString();
            }
        }

        [NotMapped]
        public string KeywordString
        {
            get
            {
                return _keywordEnum.ToString();
            }
            set
            {
                _keywordString = value;
                _keywordEnum = (DestinationKeyword)Enum.Parse(typeof(DestinationKeyword), _keywordString);
            }
        }
    }

    public enum DestinationKeyword
    {
        [DisplayName("Museum")]
        Museum,

        [DisplayName("Park")]
        Park,

        [DisplayName("Art")]
        Art,

        [DisplayName("Architecture")]
        Architecture,

        [DisplayName("Shopping")]
        Shopping,

        [DisplayName("Bar")]
        Bar,

        [DisplayName("Club")]
        Club,

        [DisplayName("Beach")]
        Beach,

        [DisplayName("Church")]
        Church
    }
}
