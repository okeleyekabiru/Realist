using System;
using System.Collections.Generic;
using System.Text;

namespace Realist.Data.Model
{
  public  class PaginationModel
    {

     
            const int maxPageSize = 50;
            public int PageNumber { get; set; } = 1;

            private int _pageSize = 10;
            public int Category { get; set; }
            public int SubCategory { get; set; }
            public int PageSize
            {
                get
                {
                    return _pageSize;
                }
                set
                {
                    _pageSize = (value > maxPageSize) ? maxPageSize : value;
                }
            }
        }
	}

