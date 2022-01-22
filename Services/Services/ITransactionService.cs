using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Utility.Common;

namespace Services.Services
{
    public interface ITransactionService
    {
        void InsertTransaction(Transaction model);
        void UpdateTransaction(Transaction model);
        void DeleteTransaction(string id);
        Transaction GetTransaction(String id);
        object GetAllTransaction(Pager page);
    }
}
