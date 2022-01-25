using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Services.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Utility.Common;
using Utility.Response;
using Utility.Enums;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using TechnicalAssignment.Model;

namespace TechnicalAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        private IHostEnvironment _environment;
        public TransactionController(ITransactionService transactionService, IMapper mapper, IHostEnvironment environment)
        {
            _transactionService = transactionService;
            _mapper = mapper;
            _environment = environment;
        }

        [HttpPost("InsertTransaction")]
        public IActionResult InsertTransaction([FromForm] TransactionModel model)
        {


                //Setting up the directory of file upload
                var dir = string.Concat(_environment.ContentRootPath, @"\wwwroot\UploadedFile");
                var path = Path.Combine(dir, model.file.FileName);
                //File Upload process
                UploadFileProcess(model.file, path);

                List<TransactionModel> result = null;

                //Check File Extension
                string extension = Path.GetExtension(model.file.FileName).Replace(".", "").ToLower().ToString();
                if (extension == "csv")
                    result = ReadCSVBasedUpload(path);
                else
                    result = ReadXMLBasedUpload(path);




                if (result.Count > 0)
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        var mappedSchool = _mapper.Map<Transaction>(result[i]);
                        _transactionService.InsertTransaction(mappedSchool);
                    }
                }
            
          
            return Ok(
                new { Status = StatusResponse.OK }
            );
        }

        [HttpGet("GetAllTransaction")]
        public IActionResult GetAllTransaction([FromQuery] Pager page)
        {
            var result = _transactionService.GetAllTransaction(page);
            return Ok(result);
        }

        [HttpGet("GetCurrency")]
        public IActionResult GetCurrency()
        {
            var result = _transactionService.GetAllCurrency();
            return Ok(result);
        }

        [HttpGet("SampleReturn")]
        public IActionResult SampleReturn()
        {
            var result = "Success";
            return Ok(result);
        }
        public List<TransactionModel> ReadXMLBasedUpload(string path)
        {
            List<TransactionModel> tranList = new List<TransactionModel>();
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNodeList nodeList = doc.DocumentElement.SelectNodes("/Transactions/Transaction");
            foreach (XmlNode node in nodeList)
            {
                var result =new TransactionModel();
                result.TransactionId = node.Attributes[0].Value;
                String date = Convert.ToDateTime(node["TransactionDate"].InnerText).ToString("o");
                result.TransactionDate = Convert.ToDateTime(date);
                foreach (XmlNode node1 in node.SelectNodes("PaymentDetails"))
                {
                    result.Amount = Convert.ToDouble(node1.SelectSingleNode("Amount").InnerText);
                    result.CurrencyCode = node1.SelectSingleNode("CurrencyCode").InnerText;
                }
                result.TransactionStatus = Convert.ToInt32(Enum.Parse(typeof(EnumTransactionStatus), node.SelectSingleNode("Status").InnerText));
                tranList.Add(result);
            }

            return tranList;
        }

        public void UploadFileProcess(IFormFile file, string path)
        {


            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }

        public List<TransactionModel> ReadCSVBasedUpload(string path)
        {
            List<TransactionModel> tranList = new List<TransactionModel>();
            using (var reader = new StreamReader(path))
            {

                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter=",",
                    HasHeaderRecord = false
                };

                using (var csvReader = new CsvReader(reader, csvConfig))
                {

                    csvReader.Context.RegisterClassMap<CsvClassMapperModel>();
                    while (csvReader.Read())
                    {
                        var record = csvReader.GetRecord<TransactionModel>();
                        tranList.Add(record);
                    }
        
                }
            }
            return tranList;
        }
    }
}
