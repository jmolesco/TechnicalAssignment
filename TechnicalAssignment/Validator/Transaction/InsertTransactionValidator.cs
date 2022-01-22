using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalAssignment.Model;

namespace TechnicalAssignment.Validator.Transaction
{
    public class InsertTransactionValidator:AbstractValidator<TransactionModel>
    {
        public InsertTransactionValidator()
        {
            CascadeMode = CascadeMode.Stop;
           
        }
    }
}
