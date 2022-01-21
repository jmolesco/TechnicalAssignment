using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Common
{
    public class Pager
    {
        public Pager()
        {
            CurrentPage = 1;
        }
        public int CurrentPage { get; set; }
        public bool ShowAll { get; set; }

        public string Keyword { get; set; }

        public string FilterBy { get; set; }
    }
}
