using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class BaseEntity
    {
            public bool Status { get; set; }
            public DateTime DateCreated { get; set; }
            public DateTime? DateModified { get; set; }

    }
}
