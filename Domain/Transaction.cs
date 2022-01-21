using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Transaction : BaseEntity
    {
        public string TransactionId { get; set; }
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public int TransactionStatus{ get; set; }
    }
}
