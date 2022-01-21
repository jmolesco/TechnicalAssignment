using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Response
{
    public class ErrorResponse
    {
        public ErrorResponse()
        {
            this.Status = StatusResponse.ER;
        }

        public string Status { get; set; }

        public object Errors { get; set; }
    }
}
