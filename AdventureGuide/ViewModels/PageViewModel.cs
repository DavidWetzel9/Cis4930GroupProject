using System;

namespace AdventureGuide.ViewModels
{

    public class PageViewModel
    {
        public PageViewModel()
        {
            PageSize = 5;
        }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int PagesCount
        {
            get
            {
                if (PageSize > 0 && TotalCount > 0)
                {
                    return (int)Math.Ceiling(TotalCount / (double)PageSize);
                }
                return 0;
            }
        }

        public bool IsMapView { get; set; }

        public bool HasPreviousPage
        {
            get
            {
                return (PageNumber > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageNumber < PagesCount);
            }
        }
    }
}
