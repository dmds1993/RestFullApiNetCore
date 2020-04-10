using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWebFull.Models
{
    public class CustomerQueryParameters
    {
        private const int maxPageCount = 100;

        public int Page { get; set; } = 1;

        private int _pageCount = 100;

        public int PageCount
        {
            get => _pageCount;
            set
            {
                _pageCount = (value > maxPageCount) ? maxPageCount : value;
            }
        }

        public bool HasQuery { get => !string.IsNullOrEmpty(Query); }
        public string Query { get; set; }
    }
}
