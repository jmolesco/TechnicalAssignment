using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Utility.Enums;

namespace TechnicalAssignment.Model
{
    public class CsvClassMapperModel : ClassMap<TransactionModel>
    {
        public CsvClassMapperModel()
        {
            string format = "dd/MM/yyyy hh:mm:ss";
            var msMY = CultureInfo.GetCultureInfo("ms-MY");

            Map(m => m.TransactionId).Index(0).Validate(field => !field.Equals(null) || !field.Equals(string.Empty));
            Map(m => m.Amount).Index(1).Validate(field => !field.Equals(null) || !field.Equals(string.Empty));
            Map(m => m.CurrencyCode).Index(2).Validate(field => !field.Equals(null) || !field.Equals(string.Empty));
            Map(m => m.TransactionDate).Index(3).TypeConverterOption.Format(format).TypeConverterOption.CultureInfo(msMY);
            Map(m => m.TransactionStatus).Index(4).Convert(row=>
            {
                String value = row.Row[4].ToString();
                return Convert.ToInt32(Enum.Parse(typeof(EnumTransactionStatus),value));
            });
 
        }
    }
}
