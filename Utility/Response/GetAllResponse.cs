using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Response
{
    public class GetAllResponse
    {
        public GetAllResponse(int total)
        {
            this.Total = total;
            this.List = new List<object>();
            this.CurrentPage = 1;
            this.TotalPage = 1;
        }

        public GetAllResponse(int total, int currentPage, int recordDisplay)
        {
            this.Total = total;
            this.CurrentPage = currentPage;
            this.TotalPage = ((total - 1) / recordDisplay) + 1;
            this.List = new List<object>();
        }

        public int Total { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public List<object> List { get; set; }
    }
}
