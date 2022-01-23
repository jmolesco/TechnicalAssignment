using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
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
            RuleFor(x => x.file.Length).NotNull().LessThanOrEqualTo(1042157)
                .WithMessage("File size is max 1 MB").OverridePropertyName("File");
            RuleFor(x => Path.GetExtension(x.file.FileName).Replace(".", "").ToLower().ToString()).NotNull().Must(x => x.Equals("csv") || x.Equals("xml"))
                .WithMessage("Unknown format").OverridePropertyName("File");
        }
    }
}
