using Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalAssignment.Model
{
    public class TransactionModel : Transaction
    {
        public IFormFile file { get; set; }
    }
}
