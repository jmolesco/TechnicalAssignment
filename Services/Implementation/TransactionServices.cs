using Domain;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility.Common;
using Utility.Extension;
using Utility.Response;
using Utility.Enums;
using Services.Services;

namespace Services.Implementation
{
    public class TransactionServices : ITransactionService
    {
        private readonly IBaseRepository<Transaction> _transactionRepository;
        public TransactionServices(IBaseRepository<Transaction> transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public void InsertTransaction(Transaction model)
        {
            _transactionRepository.Insert(model);
            _transactionRepository.SaveChanges();
        }

        public void UpdateTransaction(Transaction model)
        {
            var result = _transactionRepository.Get(obj => obj.TransactionId == model.TransactionId);
            if (result != null)
            {
                result.Amount = model.Amount;
                result.CurrencyCode = model.CurrencyCode;
                result.TransactionDate = model.TransactionDate;
                result.Status = model.Status;
                _transactionRepository.Update(model);
                _transactionRepository.SaveChanges();
            }
        }

        public void DeleteTransaction(string id)
        {
            var result = _transactionRepository.Get(obj => obj.TransactionId == id);
            _transactionRepository.Remove(result);
            _transactionRepository.SaveChanges();
        }

        public Transaction GetTransaction(String id)
        {
            var result = _transactionRepository.Get(obj => obj.TransactionId == id);
            return result;
        }

        public object GetAllTransaction(Pager page)
        {
            var records = _transactionRepository.Queryable().AsNoTracking().
                Select(p => new
                {
                    p.TransactionId,
                    Payment = Convert.ToDecimal(p.Amount) + " " + p.CurrencyCode,
                    Status = p.TransactionStatus.GetTransactionStatus().ToString().Substring(0,1),
                    p.DateCreated,
                    p.DateModified,
                    p.CurrencyCode,
                    p.TransactionStatus,
                    p.Amount,
                    TransactionDate = Convert.ToDateTime(p.TransactionDate).ToString("o"),
                    TranDate = p.TransactionDate
                });

            if (page.Keyword.HasValue())
            {
                records = records.Where(p => p.TransactionId.Contains(page.Keyword));
            }
            if (page.FilterBy.HasValue())
            {
                int value = Convert.ToInt32(page.FilterBy);
                if ((int)EnumFilterBy.CurrencyCode == value )
                {
                    records = records.Where(p => p.CurrencyCode == page.FilterByValue);
                }
                else if ((int)EnumFilterBy.DateRange == value)
                {
                    records = records.Where(p => p.TranDate <= Convert.ToDateTime(page.FilterByValue));
                }
                else if((int)EnumFilterBy.Status == value)
                {
                    int addedValFilter = Convert.ToInt32(page.FilterByValue) == 2 ? 4 : Convert.ToInt32(page.FilterByValue) == 3 ? 5 : 1;
                    records = records.Where(p => p.TransactionStatus == Convert.ToInt32(page.FilterByValue) || p.TransactionStatus==addedValFilter);
                }

            }
            records = records.OrderByDescending(x => x.DateCreated);

            GetAllResponse response = null;
            int maxRecord = 10;
            if (page.ShowAll == false)
            {
                response = new GetAllResponse(records.Count(), page.CurrentPage, maxRecord);
                records = records.Skip((page.CurrentPage - 1) * maxRecord);
            }
            else
            {
                response = new GetAllResponse(records.Count());
            }

            response.List.AddRange(records);


            return response;
        }
        

        public object GetAllCurrency()
        {
            GetAllResponse response = null;
            var records = _transactionRepository.Queryable().AsNoTracking()
                .Select(s => s.CurrencyCode).Distinct();
            response = new GetAllResponse(records.Count());
            response.List.AddRange(records);
            return response;
        }

    }
}
